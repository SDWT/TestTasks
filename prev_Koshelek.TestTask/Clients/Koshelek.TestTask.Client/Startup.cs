using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Koshelek.TestTask.Client.Hubs;
using Koshelek.TestTask.DAL.DataBase;
using Koshelek.TestTask.Interfaces.Interfaces;
using Koshelek.TestTask.Interfaces.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Koshelek.TestTask.Client
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
            services.AddControllersWithViews();
            services.AddSignalR();

            services.AddSingleton<IMessageData>(opt => new PostgreSqlMessageData(Configuration.GetConnectionString("DefaultConnection"),
                opt.GetService<ILogger<PostgreSqlMessageData>>(), opt.GetService<ILogger<PostgreSqlDbContext>>()));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Application API",
                    Description = "Application Documentation"
                });
                options.IncludeXmlComments("Koshelek.TestTask.xml");
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
                app.UseBrowserLink();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors();

            //app.UseAuthentication();
            //app.UseAuthorization();

            //app.UseSwagger();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "TestUI/swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/TestUI/swagger/v1/swagger.json", "Koshelek.TestTask V1");
                opt.RoutePrefix = "TestUI";
                opt.DocumentTitle = "Интерфейс для тестрования | Swagger UI";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessagesHub>("/messages");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
