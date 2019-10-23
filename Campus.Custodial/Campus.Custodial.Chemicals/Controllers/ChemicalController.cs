using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using static Campus.Custodial.Chemicals.Chemical;

namespace Campus.Custodial.Chemicals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChemicalController : ControllerBase
    {
        static private IDatabase datebase = new InMemoryDatabase();
        private IChemicalFactory ChemFactory = new ChemicalFactory(datebase);

        [Route("Get")]
        public string GetAll()
        {
            List<IChemical> temp = ChemFactory.ReadAllChemicals();
            List<IChemical> toReturn = new List<IChemical>();
            foreach (var x in temp)
            {
                if (!x.GetDeletedStatus())
                {
                    toReturn.Add(x);
                }
            }
            return JsonConvert.SerializeObject(toReturn);
        }

        [Route("Get/{name}")]
        public string Get(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new Chemical()
                {
                    name = null,
                    DB = datebase,
                    deleted = false
                }.ToJson();
            }
            IChemical chemicalToGet = ChemFactory.ReadChemical(name);
            if (chemicalToGet == null)
            {
                return new Chemical()
                {
                    name = null,
                    DB = datebase,
                    deleted = false
                }.ToJson();
            } else
            {
                return chemicalToGet.ToJson();
            }
        }

        [Route("Post/{name}")]
        public string Post(string name)
        {
            List<string> hazardStatements = new List<string>()
            {
                $"Place Holder"
            };
            List<string> precautionStatements = new List<string>()
            {
                $"Place Holder"
            };
            List<signalWords> sigWords = new List<signalWords>();
            String productIdentifier = $"Place Holder";
            Manufacturer manufacturer = new Manufacturer()
            {
                name = $"place Holder",
                address = $"place Holder",
                phoneNumber = $"place Holder"
            };
            string result = ChemFactory.CreateChemical(name, manufacturer, productIdentifier, sigWords, hazardStatements, precautionStatements).ToJson();
            if (result.Contains($"null"))
            {
                return $"Failed to Post";
            }
            return result;

        }

        [Route("Put/{nameOriginal}/{nameUpdated}")]
        public string Put(string nameOriginal, string nameUpdated)
        {
            if (string.IsNullOrEmpty(nameOriginal) && string.IsNullOrEmpty(nameUpdated))
            {
                return $"Failed to update {nameOriginal} to {nameUpdated}";
            } else
            {
                return ChemFactory.ReadChemical(nameOriginal).UpdateChemical(new Chemical()
                {
                    name = nameUpdated,
                    DB = ChemFactory.getDB()
                }).ToJson();
            }
        }

        [Route("Delete/{name}")]
        public string Delete(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return $"Failed to delete {name}";
            } else
            {
                return ChemFactory.ReadChemical(name).DeleteChemical().ToJson();
            }
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

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
        }
    }
}
