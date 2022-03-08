using BL.API.Core.Abstractions.Repositories;
using BL.API.Core.Domain.User;
using BL.API.DataAccess.Data;
using BL.API.DataAccess.Repositories;
using BL.API.Services.MMR;
using BL.API.Services.Players.Commands;
using BL.API.Services.Seasons;
using BL.API.Services.Stats.Model;
using BL.API.WebHost.Middleware;
using BL.API.WebHost.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using BL.API.Core.Abstractions.Services;
using BL.API.Services.Authorization;

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
                option.UseSqlServer(sqlConnectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            });

            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));
            services.AddScoped(typeof(IMMRCalculationBuilder), typeof(MMRCalculationBuilder));
            services.AddScoped(typeof(IMMRCalculationService), typeof(MMRCalculationService));
            services.AddScoped(typeof(ISeasonResolverService), typeof(SeasonResolverService)); //temporary - must be changed to cache
            services.AddScoped<AuthSeeder>();

            services.AddHostedService<ResourceMonitorService>();

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

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\""
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                            {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddMediatR(typeof(AddPlayerCommand.AddPlayerCommandHandler).Assembly);

            services.AddIdentity<User, Role>()
                 .AddEntityFrameworkStores<EFContext>()
                 .AddDefaultTokenProviders();

            services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecretKey"]))
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AuthSeeder authSeeder)
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            authSeeder.Seed().Wait();
        }
    }
}
