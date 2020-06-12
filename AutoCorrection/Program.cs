using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoCorrection.Searcher;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Practices.Unity.Configuration;
using Unity;
using Unity.Microsoft.DependencyInjection;

namespace AutoCorrection
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var startup = new Stopwatch();
            startup.Start();
            CreateHostBuilder(args).Build().Run();
            startup.Stop();
            Console.WriteLine("main, crate host builder: " + startup.ElapsedMilliseconds.ToString());
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseUnityServiceProvider();
    }
}
