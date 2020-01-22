using System.Threading;
using IutInfo.ProgReseau.BuildBlocks.RabbitMQ;
using IutInfo.ProgReseau.RabbitServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IutInfo.ProgReseau.RabbitServer
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            Thread.Sleep(10000);
            HostingAbstractionsHostExtensions.Run(CreateHostBuilder(args).Build());
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRabbit(Configuration);

            services.AddHostedService<RabbitHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
}