using System.Collections.Generic;
using Extensions.Object;
using FC.TelegramBot.Core.Eventing;
using FC.TelegramBot.Core.Words;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace FC.TelegramBot.Core.Messaging.MessageHandlers
{
    public class StartMessageHandler : IMessageHandler
    {
        public bool Executable( MessageEventArgs message )
            => message.Message.Text.ToLower() == "/start";

        public bool Execute( ITelegramBotClient client, MessageEventArgs message )
        {
            var words = ExObject.FindObjectOfType<ExWordsObject>();
            var data = message.Message;

            var replyKeyboardMarkup = SendWriteAction( client, data.Chat.Id )
                .ExecuteEvent( data )
                .PrepareMarkup();

            client.SendTextMessageAsync(
                chatId: message.Message.Chat.Id,
                text: words.Word( "text__welcomeMessage", new WordPair( "username", data.From.Username ) ),
                replyMarkup: replyKeyboardMarkup
            );

            return true;
        }

        private StartMessageHandler SendWriteAction( ITelegramBotClient client, ChatId id )
        {
            client.SendChatActionAsync( id, ChatAction.Typing );
            return this;
        }

        private StartMessageHandler ExecuteEvent(Message msg)
        {
            ExObject.FindObjectOfType<ExEventObject>().Call( "OnStartMessageReceived", new CoreEvent
            (
                this,
                new List<object>()
                {
                    msg
                }
            ));

            return this;
        }

        private ReplyKeyboardMarkup PrepareMarkup()
        {
            var words = ExObject.FindObjectOfType<ExWordsObject>();

            return new ReplyKeyboardMarkup(
                new KeyboardButton[][]
                {
                    new KeyboardButton[] { words.Word( "menu__coffeeOrder" ), words.Word("menu__ordersHistory") },
                    new KeyboardButton[] { words.Word( "menu__freshPromotions" ), words.Word("menu__getGift") },
                    new KeyboardButton[] { words.Word( "menu__contacts" ), words.Word("menu__report") },
                },
                resizeKeyboard: true
            );
        }
    }
}
