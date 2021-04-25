namespace FC.TelegramBot.Core.Commerce.Models
{
    public class OrderedItem
    {
        public long Id;
        public long ModifierId;
        public long VolumeId;
        public int Count;
        public float Price;

        public OrderedItem(long id, long modId, long volumeId, int count, float price)
        {
            Id = id;
            ModifierId = modId;
            VolumeId = volumeId;
            Count = count;
            Price = price;
        }
    }
}
