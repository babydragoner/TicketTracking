// Unused usings removed
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using TicketTracking.Models;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using System;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using TicketTracking.Utility;

namespace TicketTracking
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
            services.AddControllers();

            services.AddDbContext<DBContext>(opt =>
                                               opt.UseInMemoryDatabase("Tickets"));

            services.AddDistributedMemoryCache();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tickets Api", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Description = "Token",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                };
                c.AddSecurityDefinition("Bearer", securityScheme);

                // Security Requirement
                c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    { securityScheme, System.Array.Empty<string>() }
                });

                c.CustomSchemaIds(s => s.FullName);

                c.EnableAnnotations();
                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });


            services.AddSingleton<CacheHelper>();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DBContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoApi v1"));
                context.init();
            }

            //this._logger = logger;
            //app.UseExceptionHandler(errorApp =>
            //{
            //    errorApp.Run(async context =>
            //    {
            //        context.Response.ContentType = "application/json";
            //        var res = new BaseResultObj<string>();
            //        res.UpdateStatus(StatusMessage.Other, EnumLanguage.en_us);
            //        res.Message = "Internal Server Error";

            //        var conFeature = context.Features.Get<IExceptionHandlerFeature>();
            //        if (conFeature != null)
            //        {
            //            context.Response.StatusCode = 200;
            //            var ex = conFeature.Error;
            //            res.Message = ex.Message;

            //            if (ex.GetType() == typeof(MyException))
            //            {
            //                var ex2 = (MyException)ex;
            //                res.Message = env2.IsDevelopment() ? $"{ex2.StatusCode}_{res.Message}" : "";

            //                _logger.LogWarning(new EventId(ex2.HResult), ex, ex2.StatusCode + "_" + ex2.Message);
            //            }
            //            else
            //            {
            //                _logger.LogError($"Something went wrong: {ex}");
            //                //EMailHelper.Instance.SendMail(ex.Message, ex.StackTrace);

            //            }
            //        }
            //        else
            //        {
            //            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //        }
            //        await context.Response.WriteAsync(JsonContent.SerializeObject(res));
            //    });
            //});
            app.UseCors("MyPolicy");

            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}