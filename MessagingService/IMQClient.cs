using MessagingService.Contracts;

namespace MessagingService
{
    public interface IMQClient
    {
        void SubscribeToItemUpdates(Func<Item,bool> action);
        void PublishItemUpdated(Item item);
        void SubscribeToItemDeletes(Func<int,bool> action);
        void PublishItemDeleted(int id);
    }
}
