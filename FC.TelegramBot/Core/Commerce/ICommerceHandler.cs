using Telegram.Bot.Types;

namespace FC.TelegramBot.Core.Commerce
{
    interface ICommerceHandler
    {
        public string Trigger();
        public void Execute( Message message );
    }
}
