using BL.API.Core.Abstractions.Repositories;
using BL.API.DataAccess.Data;
using BL.API.DataAccess.Repositories;
using BL.API.Services.Players.Commands;
using BL.API.WebHost.Middleware;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace BL.API.WebHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var sqlConnectionString = Configuration.GetConnectionString("DefaultConnectionString");

            services.AddDbContext<EFContext>(option =>
            {
                option.UseSqlServer(sqlConnectionString);
                option.UseLazyLoadingProxies();
            });

            services
                .AddControllers()
                .AddNewtonsoftJson();

            services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BL.API.WebHost", Version = "v1" });
            });

            services.AddMediatR(typeof(AddPlayerCommand.AddPlayerCommandHandler).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BL.API.WebHost v1"));
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
