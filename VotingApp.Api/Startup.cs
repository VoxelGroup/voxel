using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyWebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VotingApp.Lib;

namespace VotingApp.Api
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
            services.AddMvc();
            services.AddSingleton<Voting>();

            services.AddHealthChecks()
                .AddCheck("liveness", () => HealthCheckResult.Healthy(), new[] { "live" })
                .AddCheck("readiness", () => HealthCheckResult.Healthy(), new[] { "ready" });

            // HealthCheckResult FakeFailingCheck() => DateTime.Now.Second > 30 ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();

            if (Configuration["mongodb"] == null)
            {
                services.AddSingleton<IVotingService, InMemoryVotingService>();
            }
            else
            {
                services.AddScoped<IVotingService>(
                    x => new MongoDbVotingService(Configuration["mongodb"])
                );
            }

            services.AddEasyWebSockets();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHealthChecks("/hc/ready", new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("ready"),
            })
            .UseHealthChecks("/hc/live", new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("live"),
            });
            app.UseEasyWebSockets();
            app.UseMvc();
        }
    }
}
