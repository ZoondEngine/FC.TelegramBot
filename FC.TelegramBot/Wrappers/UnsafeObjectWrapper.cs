namespace FC.TelegramBot
{
    public static class UnsafeObjectWrapper
    {
        public static T Convert<T>( this object obj )
                => ( T ) obj;
    }
}
