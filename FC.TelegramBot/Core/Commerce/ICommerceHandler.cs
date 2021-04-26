using Telegram.Bot.Types;

namespace FC.TelegramBot.Core.Commerce
{
    interface ICommerceHandler
    {
        public bool Triggered( string text );
        public void Execute( Telegram.Bot.ITelegramBotClient client, Message message );
    }
}
