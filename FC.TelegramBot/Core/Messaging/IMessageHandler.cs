using Telegram.Bot;
using Telegram.Bot.Args;

namespace FC.TelegramBot.Core.Messaging
{
    public interface IMessageHandler
    {
        bool Executable( MessageEventArgs message );
        bool Execute( ITelegramBotClient client, MessageEventArgs message );
    }
}
