using System.Collections.Generic;
using System.Linq;
using Extensions.Object;
using FC.TelegramBot.Core.Commerce;
using FC.TelegramBot.Core.Database;
using FC.TelegramBot.Core.Words;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace FC.TelegramBot.Core.Messaging.MessageHandlers
{
    public class OrderMessageHandler : IMessageHandler
    {
        private ExCommerceObject Commerce;
        private ExWordsObject Words;
        private ExDatabaseObject Database;

        public bool Executable( MessageEventArgs message )
        {
            if(Words == null)
            {
                Words = ExObject.FindObjectOfType<ExWordsObject>();
            }

            return message.Message.Text.ToLower() == Words.Word( "menu__coffeeOrder" );
        }

        public bool Execute( ITelegramBotClient client, MessageEventArgs message )
        {
            if(Commerce == null)
            {
                Commerce = ExObject.FindObjectOfType<ExCommerceObject>();
            }

            var cart = Commerce.Cart();

            if ( !cart.UserOrderingNow( message.Message.From.Id ) )
            {
                if(Database == null)
                {
                    Database = ExObject.FindObjectOfType<ExDatabaseObject>();
                }

                using var context = Database.Db();
                var user = context.Users.Where( ( x ) => x.ExternalId == message.Message.From.Id ).FirstOrDefault();
                
                if ( user != null )
                {
                    cart.AddCartItem( new Commerce.Models.CartItem( user.Id, message.Message.From.Id ) );
                    var orderId = cart.GetOrder( message.Message.From.Id )?.OrderId ?? 0;

                    client.SendTextMessageAsync(
                        chatId: message.Message.Chat.Id,
                        text: Words.Word( "order__orderStart", new WordPair( "orderId", orderId.ToString() ) ),
                        replyMarkup: BuildOrderMenu()
                    );

                    return true;
                }
                else
                {
                    Log.Error( $"User: {message.Message.From.Username}({message.Message.From.Id}) try to order without saving in database, core error?" );
                    return false;
                }
            }
            else
            {
                var orderId = cart.GetOrder( message.Message.From.Id )?.OrderId ?? 0;

                client.SendTextMessageAsync(
                    message.Message.Chat.Id,
                    Words.Word( "order__orderAlreadyExists", new WordPair( "order", orderId.ToString() ) )
                );

                return false;
            }
        }

        private ReplyKeyboardMarkup BuildOrderMenu()
        {
            var keyboardItems = new List<KeyboardButton[]>();
            using var context = Database.Db();
            var menus = context.OrderMenus;

            foreach(var menu in menus)
            {
                keyboardItems.Add( new KeyboardButton[]
                {
                    menu.Title
                });
            }

            if(keyboardItems.Count <= 0)
            {
                keyboardItems.Add( new KeyboardButton[] { "Раф", "Капучино", "Еще какаято фигня" } );
            }

            return new ReplyKeyboardMarkup(
                keyboard: keyboardItems.ToArray(),
                resizeKeyboard: true
            );
        }
    }
}
