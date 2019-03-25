using System;
using System.IO;
using Aigang.Platform.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using Newtonsoft.Json.Converters;
using Aigang.Platform.API.Middleware;
using System.Net;
using Microsoft.AspNetCore.Http;
using Aigang.Platform.Contracts;
using log4net;
using System.Threading.Tasks;
using Aigang.Platform.Handlers.Utils;

namespace Aigang.Platform.API
{
    public class Startup
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Startup));

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", reloadOnChange: true, optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            
            builder.AddEnvironmentVariables();
            ConfigurationManager.Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.OutputFormatters.RemoveType<TextOutputFormatter>();
                options.OutputFormatters.RemoveType<StringOutputFormatter>(); //remove plain text responses
                options.InputFormatters.RemoveType<TextInputFormatter>();
            }).AddJsonOptions(options =>
         {
             options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
             options.SerializerSettings.Converters.Add(new StringEnumConverter(camelCaseText: true));
         });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Aigang Platform API", Version = "v1" });
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "Aigang.Platform.API.xml");
                c.IncludeXmlComments(xmlPath);
            });

            MapperConfiguration.RegisterMappings();

            JsonConvert.DefaultSettings = (() =>
                    {
                        var settings = new JsonSerializerSettings();
                        settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                        return settings;
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler(
                     options =>
                     {
                         options.Run(
                         async context =>
                         {
                             await SendUnhandledError(context, true);
                         });
                     }
                    );
            }
            else
            {
                app.UseExceptionHandler(
                     options =>
                     {
                         options.Run(
                         async context =>
                         {
                             await SendUnhandledError(context, false);
                         });
                     }
                    );
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aigang API V1");
            });

            app.UseOptionsMiddleware(); //should be before authentication
            
            app.UseMiddleware<BannedCountriesMiddleware>();
           
            app.UseMvc();
            
            GlobalContext.Properties["Environment"] = env.EnvironmentName;
        }

        private async Task SendUnhandledError(HttpContext context, bool debug)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var ex = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

            var response = new BaseResponse();

            if (ex != null)
            {
                _logger.Error(ex.Error.Message, ex.Error);
                if (debug)
                {
                    response.Error = new Contracts.Errors.InternalServerErrorResponse(ex.Error.ToString());
                }
                else
                {
                    response.Error = new Contracts.Errors.InternalServerErrorResponse(ex.Error.Message);
                }

            }
            else
            {
                _logger.Error("Unhandled error without exception");
                response.Error = new Contracts.Errors.InternalServerErrorResponse("Unhandled");
            }

            var errorResponse = JsonConvert.SerializeObject(response);

            await context.Response.WriteAsync(errorResponse).ConfigureAwait(false);
        }



    }
}
