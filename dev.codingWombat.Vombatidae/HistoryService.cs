using System;
using System.Threading;
using System.Threading.Tasks;
using dev.codingWombat.Vombatidae.core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace dev.codingWombat.Vombatidae
{
    public class HistoryService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public HistoryService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var historyHandler = _serviceProvider.GetRequiredService<IHistoryHandler>();
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var historyHandler = _serviceProvider.GetRequiredService<IHistoryHandler>();
            await historyHandler.SaveOptionsAsync();
        }
    }
}