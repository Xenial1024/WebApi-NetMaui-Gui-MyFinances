using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MyFinances.WebApi.Models;
using MyFinances.WebApi.Models.Domains;
using System;
using System.IO;
using System.Reflection;

namespace MyFinances.WebApi
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration => configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<UnitOfWork, UnitOfWork>();

            services.AddDbContext<MyFinancesContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("MyFinancesContext")));

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MyFinances",
                    Version = "v1",
                    Description = "Application description:<br>"+ 
                        "Single-instance ASP.NET Core Web API REST application that stores all user expenses in a database.<br>" +
                        "The user can add, update, delete, and view their expenses.<br>" +
                        "The repository includes a database dump \"Database.sql\", where the operation with ID 6 has been deleted,<br>" +
                        "so the first 10 operations have IDs from 1 to 11 (excluding 6)."

                });
                c.MapType<int>(() => new OpenApiSchema
                {
                    Type = "number",
                    Format = "int",
                    Example = new OpenApiInteger(1)
                });
                c.MapType<decimal>(() => new OpenApiSchema
                {
                    Type = "number",
                    Format = "decimal",
                    Example = new OpenApiDouble(1.0)
                });

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature?.Error;

                    var logger = NLog.LogManager.GetCurrentClassLogger();
                    logger.Error(exception, $"Unhandled exception: {exception.Message}");

                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("An unexpected error occurred.");
                });
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
