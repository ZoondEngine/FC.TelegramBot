using Extensions.Object;
using FC.TelegramBot.Core.Logging;

namespace FC.TelegramBot.Core
{
    public static class Log
    {
        private static readonly ExLoggingObject LogObject;

        static Log()
        {
            LogObject = ExObject.Instantiate<ExLoggingObject>();
        }

        public static void Error( string message )
            => LogObject.Error( message );
        public static void Warning( string message )
            => LogObject.Warning( message );
        public static void Trace( string message )
            => LogObject.Debug( message );
    }
}
