using System.Collections.Generic;
using System.Linq;
using Extensions.Object;
using FC.TelegramBot.Core.Database;
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
            //using var db = ExObject.FindObjectOfType<ExDatabaseObject>().Db();
            //
            var data = message.Message;
            //if ( db.Users.FirstOrDefault( ( x ) => x.UserName == data.From.Username ) == default )
            //{
            //
            //}

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
