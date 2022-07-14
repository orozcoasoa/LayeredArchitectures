using MessagingService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CartingService.BLL
{
    public class MQListener : BackgroundService
    {
        private readonly IMQClient _mqClient;
        private readonly IServiceScopeFactory _scopeFactory;

        public MQListener(IMQClient mqClient, IServiceScopeFactory scopeFactory)
        {
            _mqClient = mqClient;
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _mqClient.SubscribeToItemUpdates(ProcessItemUpdate);
            _mqClient.SubscribeToItemDeletes(ProcessItemDelete);

            return Task.CompletedTask;
        }

        public bool ProcessItemUpdate(MessagingService.Contracts.Item item)
        {
            var result = true;
            using (var scope = _scopeFactory.CreateScope())
            {
                var cartingService = scope.ServiceProvider.GetRequiredService<ICartingService>();
                try
                {
                    cartingService.ItemUpdated(item);
                }
                //TODO: Log failures
                catch(Exception) { result = false; }
            }
            return result;
        }
        public bool ProcessItemDelete(int itemId)
        {
            var result = true;
            using (var scope = _scopeFactory.CreateScope())
            {
                var cartingService = scope.ServiceProvider.GetRequiredService<ICartingService>();
                try
                {
                    cartingService.ItemDeleted(itemId);
                }
                //TODO: Log failures
                catch (Exception) { result = false; }
            }
            return result;
        }
    }
}
