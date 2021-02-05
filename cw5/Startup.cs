using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw5.Models;
using cw5.Services;
using cw6.Handlers;
using cw6.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace cw5
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
            
            services.AddAuthentication("AuthenticationBasic").AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("AuthenticationBasic", null);
            services.AddControllers().AddXmlSerializerFormatters();
            services.AddTransient<IStudentDbService, OracleStudentDbService>();
            services.AddControllers();
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Title = "Students App API", Version = "v1" });
            });
            services.AddScoped<IStudentDbService, SqlServerStudentDbService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IStudentDbService service)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseMiddleware<ExceptionMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Students App API");
            });

            app.UseMiddleware<LoggingMiddleware>();

            app.Use(async (context, next) =>
            {
                if (!context.Request.Headers.ContainsKey("Index"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    //await context.Response.WriteAsync("Musisz podac numer indeksu.");
                    context.Response.StatusCode = 401;
                    return;
                }
                string index = context.Request.Headers["Index"].ToString();
                Student stud = service.GetStudent(index);
                if (stud == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Student nie istnieje.");
                    return;
                }

                await next();
            });

            app.UseHttpsRedirection();



            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
