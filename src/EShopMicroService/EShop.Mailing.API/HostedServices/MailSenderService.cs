using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EShop.Mailing.API.HostedServices
{
    public class MailSenderService : BackgroundService
    {
        private readonly ILogger<MailSenderService> _logger;

        public MailSenderService(ILogger<MailSenderService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("MailSenderService started");

            stoppingToken.Register(() => _logger.LogDebug("MailSenderService is stoping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("MailSenderService is doing work.");

              

                
                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }

            _logger.LogDebug("MailSenderService ended");

            await Task.CompletedTask;
        }
    }
}