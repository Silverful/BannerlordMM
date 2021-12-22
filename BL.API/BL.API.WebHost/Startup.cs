using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Abstractions.Services;
using BL.API.DataAccess.Data;
using BL.API.DataAccess.Repositories;
using BL.API.Services.MMR;
using BL.API.Services.Players.Commands;
using BL.API.Services.Seasons;
using BL.API.Services.Stats.Model;
using BL.API.WebHost.Middleware;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;

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

            services.Configure<BasicMMRCalculationProperties>(options => Configuration.GetSection("MMRProps").Bind(options));
            services.Configure<StatsProps>(options => Configuration.GetSection("StatsProps").Bind(options));

            services.AddDbContext<EFContext>(option =>
            {
                option.UseSqlServer(sqlConnectionString);
            });

            services
                .AddControllers()
                .AddNewtonsoftJson()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));
            services.AddScoped(typeof(IMMRCalculationService), typeof(MMRCalculationService));
            services.AddScoped(typeof(ISeasonResolverService), typeof(SeasonResolverService)); //temporary - must be changed to cache

            services.AddCors(options =>
            {
                options.AddPolicy("AnyOrigin", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod();
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BL.API.WebHost", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddMediatR(typeof(AddPlayerCommand.AddPlayerCommandHandler).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) => {
                context.Request.EnableBuffering();
                await next();
            });

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BL.API.WebHost v1"));

            app.UseHttpsRedirection();

            app.UseCors("AnyOrigin");

            app.UseRouting();

            //global error handler
            app.UseMiddleware<ExceptionMiddleware>();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
