using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Configuration;
using Core.Extensions;
using Core.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SenderService.AsyncServices;
using SenderService.Configs;
using SenderService.Services;

namespace SenderService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<EmailServiceSettings>(
                Configuration.GetSection(nameof(EmailServiceSettings))
            );
            services.Configure<RabbitMqConfiguration>(
                Configuration.GetSection(nameof(RabbitMqConfiguration))
            );
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<ReceiverService>();
            services.AddSingleton<RabbitMqChannelFactory>();
            services.AddSingleton<NotificationQueueConsumer>(sp =>
            {
                var topology = new NotificationQueueTopology();
                var channelFactory = sp.GetRequiredService<RabbitMqChannelFactory>();
                return new NotificationQueueConsumer(channelFactory, topology);
            });

            services.AddConsulClient(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseConsulDiscovery();
        }
    }
}
