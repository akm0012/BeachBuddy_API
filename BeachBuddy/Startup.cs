using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeachBuddy.DbContexts;
using BeachBuddy.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BeachBuddy
{
    public class Startup
    {
        private IHostEnvironment CurrentEnvironment { get; set; }

        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            CurrentEnvironment = hostEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Library used to map objects to DTOs
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IBeachBuddyRepository, BeachBuddyRepository>();
            
            services.AddDbContext<BeachBuddyContext>(options =>
            {
            
                if (CurrentEnvironment.IsDevelopment())
                {
                    // Use local Docker DB
                    options.UseSqlServer(Configuration.GetConnectionString("BeachBuddyDB"));
                }
                else
                {
                    // In Prod use the Docker-Compose database
                    var connection = @"Server=db;Database=BeachBuddyDB;User=sa;Password=abcABC123;";
                    options.UseSqlServer(connection);
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        // Todo: This is where you could log this stuff
                        await context.Response.WriteAsync(
                            "Red Flag! Get out of the ocean as there was a server error!");
                    });
                });
            }

            // Check if the secret access header is in the request
            app.Use(async (context, next) =>
                {
                    if (!context.Request.Headers.ContainsKey("AppToken") ||
                        context.Request.Headers["AppToken"] != APIKeys.AppSecretHeader)
                    {
                        logger.LogWarning("Token was not found! Intruder alert!");
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("No seagulls allowed. We don't know who you are.");
                    }
                    else
                    {
                        await next.Invoke();
                    }
                }
            );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}