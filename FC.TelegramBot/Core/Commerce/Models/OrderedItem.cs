namespace FC.TelegramBot.Core.Commerce.Models
{
    public class OrderedItem
    {
        public string Item;
        public string Modifier;
        public string Volume;
        public int Count;
        public float Price;

        public OrderedItem(string item, int count, float price)
        {
            Item = item;
            Count = count;
            Price = price;
        }

        public void SetVolume(string volume)
        {
            Volume = volume;
        }
        public void SetModifier(string mod)
        {
            Modifier = mod;
        }
    }
}
