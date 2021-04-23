using System.Collections.Generic;
using Extensions.Object;
using FC.TelegramBot.Core.Eventing;
using FC.TelegramBot.Core.Words;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace FC.TelegramBot.Core.Messaging.MessageHandlers
{
    public class StartMessageHandler : IMessageHandler
    {
        public bool Executable( MessageEventArgs message )
            => message.Message.Text.ToLower().Contains( "/start" );

        public bool Execute( ITelegramBotClient client, MessageEventArgs message )
        {
            client.SendChatActionAsync( message.Message.Chat.Id, ChatAction.Typing );

            var words = ExObject.FindObjectOfType<ExWordsObject>();
            var data = message.Message;

            ExObject.FindObjectOfType<ExEventObject>().Call( "OnStartMessageReceived", new CoreEvent
            (
                this,
                new List<object>()
                {
                    data
                }
            ));

            var replyKeyboardMarkup = new ReplyKeyboardMarkup(
                new KeyboardButton[][]
                {
                    new KeyboardButton[] { words.Word( "menu__coffeeOrder" ), words.Word("menu__ordersHistory") },
                    new KeyboardButton[] { words.Word( "menu__freshPromotions" ), words.Word("menu__getGift") },
                    new KeyboardButton[] { words.Word( "menu__contacts" ), words.Word("menu__report") },
                },
                resizeKeyboard: true
            );

            client.SendTextMessageAsync(
                chatId: message.Message.Chat.Id,
                text: words.Word( "text__welcomeMessage", new WordPair( "username", data.From.Username ) ),
                replyMarkup: replyKeyboardMarkup
            );

            return true;
        }
    }
}
