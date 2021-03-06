﻿using api.Models;
using api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using SerilogLog = Serilog.Log;

namespace api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        IHostingEnvironment _env;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MyOptions>(Configuration);
            services.Configure<EnvironmentConfig>(Configuration);

            services.AddTransient<IFileService, FileManager>();
            services.AddTransient<IFileUploadService, FileUploadManager>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCustomSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, LoggingLevelSwitch levelSwitch)
        {
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                levelSwitch.MinimumLevel = LogEventLevel.Debug;
                SerilogLog.Logger = new LoggerConfiguration()
                  .WriteTo.Sink((ILogEventSink)SerilogLog.Logger)
                  .WriteTo.File(
                    "log.txt",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    rollOnFileSizeLimit: true,
                    shared: true)
                  .CreateLogger();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                levelSwitch.MinimumLevel = LogEventLevel.Information;
                //app.UseHsts();
            }

            app.UseCustomSwagger();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
