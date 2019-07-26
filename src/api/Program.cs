using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using SerilogLog = Serilog.Log;

namespace api
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        private static readonly LoggingLevelSwitch LevelSwitch = new LoggingLevelSwitch(LogEventLevel.Information);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            SerilogLog.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(LevelSwitch)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                CreateWebHostBuilder(args)
                .Build()
                .Run();
                SerilogLog.Information("Start the web host!");
            }
            catch (System.Exception exp)
            {
                SerilogLog.Fatal(exp, "Host terminated unexpectedly");
            }
            finally
            {
                SerilogLog.Information("Ending the web host!");
                SerilogLog.CloseAndFlush();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, config) =>
                {
                    config.ClearProviders();
                })
                .ConfigureServices(s => { s.AddSingleton(LevelSwitch); })
                .UseStartup<Startup>();
    }
}
