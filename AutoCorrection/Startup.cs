using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoCorrection.Searcher;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using Unity.Resolution;
using System.Diagnostics;

namespace AutoCorrection
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            string path = ConfigurationManager.AppSettings["DictionaryPath"].ToString();
            StaticVariables.Dictionary = Dictionary.LoadDictionary(path);
            //StaticVariables.NGgramIndex = (new NGramIndexer(new RussianAlphabet())).CreateIndex(StaticVariables.Dictionary);
        }
        public void ConfigureContainer(IUnityContainer container)
        {
            // container.LoadConfiguration();
            StaticVariables.Container = new UnityContainer();
            StaticVariables.Container.LoadConfiguration();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            var alphabet = new RussianAlphabet();
            var time = new Stopwatch();
            IIndexer indexer = StaticVariables.Container.Resolve<IIndexer>(new ResolverOverride[] {
                                                                      new ParameterOverride("alphabet", alphabet)
                                                                      });
            time.Start();
            StaticVariables.Index = indexer.CreateIndex(StaticVariables.Dictionary);
            time.Stop();
            Console.WriteLine("Cretae Index: " + time.ElapsedMilliseconds.ToString());
            Console.WriteLine(indexer.GetType().ToString());
        }
    }
}
