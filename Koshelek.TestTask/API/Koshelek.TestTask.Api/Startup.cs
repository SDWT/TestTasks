using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Koshelek.TestTask.DAL.DataBase;
using Koshelek.TestTask.Interfaces.Interfaces;
using Koshelek.TestTask.Interfaces.Services;
using Microsoft.OpenApi.Models;

namespace Koshelek.TestTask.Api
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
            //Add PostgreSQL support

            services.AddTransient<IMessageData>(opt => new PostgreSqlMessageData(Configuration["Data:DbContext:MessagesConnectionString"],
                opt.GetService<ILogger<PostgreSqlMessageData>>(), opt.GetService<ILogger<PostgreSqlDbContext>>()));

            services.AddControllers();

            services.AddCors(o => o.AddPolicy("AllowAllPolicy", options =>
            {
                options.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Application API",
                    Description = "Application Documentation"
                });
                options.IncludeXmlComments("Koshelek.TestTask.Api.xml");
                // Add XML comment document by uncommenting the following
                // var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "MyApi.xml");
                // options.IncludeXmlComments(filePath);

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Koshelek.TestTask V1");
                //opt.RoutePrefix = "TestUI";
                opt.DocumentTitle = "Интерфейс для тестрования | Swagger UI";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
