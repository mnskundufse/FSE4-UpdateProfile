﻿using Engineer.UpdateProfileService.Business.Contracts;
using Engineer.UpdateProfileService.Business.Implementation;
using Engineer.UpdateProfileService.Middleware;
using Engineer.UpdateProfileService.Models;
using Engineer.UpdateProfileService.Repository.Contracts;
using Engineer.UpdateProfileService.Repository.Implementation;
using Engineer.UpdateProfileService.ServiceDiscovery;
using Confluent.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Engineer.UpdateProfileService.Kafka;

namespace Engineer.UpdateProfileService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddScoped<IUpdateProfileBusiness, UpdateProfileBusiness>();
            services.AddScoped<IProducerWrapper, ProducerWrapper>();

            var config = new ServerConfig();
            Configuration.Bind(config);
            var customerContext = new UpdateProfileContext(config.MongoDB);
            var repo = new UpdateProfileRepository(customerContext);
            services.AddSingleton<IUpdateProfileRepository>(repo);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            var producerConfig = new ProducerConfig();
            Configuration.Bind("producer", producerConfig);

            services.AddSingleton<ProducerConfig>(producerConfig);

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddConsulConfig(Configuration);

            var contact = new OpenApiContact()
            {
                Name = "Manas Kundu",
                Email = "mnskundu@gmail.com",
                Url = new Uri("http://www.example.com")
            };

            var license = new OpenApiLicense()
            {
                Name = "My License",
                Url = new Uri("http://www.example.com")
            };

            var info = new OpenApiInfo()
            {
                Version = "v1",
                Title = "Swagger API: Update Expertise Level for Full Stack Engineer Profile",
                Description = "Full Stack Engineer Functionality: As a Full Stack Engineer, User can able to update the expertise level in Skill Tracker Application.",
                TermsOfService = new Uri("http://www.example.com"),
                Contact = contact,
                License = license
            };

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", info);

                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseConsul(Configuration);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                "Swagger API: Update Full Stack Engineer Profile v1");
            });
        }
    }
}