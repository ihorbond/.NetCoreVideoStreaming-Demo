using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VideoStreaming.Models;
using VideoStreaming.Services;

namespace VideoStreaming
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            IFileProvider physicalFileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());

            services.AddSingleton(physicalFileProvider);
            services.AddScoped<IStreamingService, StreamingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context => {
                        var ex = context.Features.Get<IExceptionHandlerPathFeature>();
                        if (ex?.Error is NullReferenceException)
                            context.Response.StatusCode = 404;
                        else if (ex?.Error is InvalidOperationException)
                            context.Response.StatusCode = 400;
                        else
                            context.Response.StatusCode = 500;

                        context.Response.ContentType = "application/json";

                        ApiError error = new ApiError()
                        {
                            Code = context.Response.StatusCode,
                            Message = env.IsDevelopment() ? ex?.Error.Message : "An unexpected error happened. Try again later."
                        };

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(error)).ConfigureAwait(false);
                    });
                });

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
