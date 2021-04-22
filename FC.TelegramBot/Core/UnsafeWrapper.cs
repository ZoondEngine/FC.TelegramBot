namespace FC.TelegramBot.Core
{
    public class UnsafeWrapper
    {
        public T As<T>()
            => ( T ) ( object ) this;
    }
}
