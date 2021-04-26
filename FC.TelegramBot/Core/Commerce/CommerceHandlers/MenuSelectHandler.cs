using System.Collections.Generic;
using System.Linq;
using Extensions.Object;
using FC.TelegramBot.Core.Database;
using FC.TelegramBot.Core.Database.Models;
using FC.TelegramBot.Core.Words;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FC.TelegramBot.Core.Commerce.CommerceHandlers
{
    public class MenuSelectHandler : ICommerceHandler
    {
        private readonly ExDatabaseObject Database;
        private readonly ExWordsObject Words;

        public MenuSelectHandler()
        {
            Database = ExObject.FindObjectOfType<ExDatabaseObject>();
            Words = ExObject.FindObjectOfType<ExWordsObject>();
        }

        public void Execute( Telegram.Bot.ITelegramBotClient client, Message message )
        {
            var menu = GetRelationMenu( message.Text );

            if ( menu != null )
            {
                using var db = Database.Db();
                var items = db.OrderItems.Where( ( x ) => x.MenuId == menu.Id );

                if(items.Count() > 0)
                {
                    client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: menu.Description,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                        replyMarkup: BuildItemsMenu( items.ToList() )
                    );
                }
                else
                {
                    client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: Words.Word( "order__menuItemsEmpty" ),
                        replyMarkup: BuildOrderMenu()
                    );
                }
            }
            else
            {
                client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: Words.Word( "order__menuNotFound" ),
                    replyMarkup: BuildOrderMenu()
                );
            }
        }

        public bool Triggered( string text )
        {
            using var db = Database.Db();

            return db.OrderMenus
                .Where( ( x ) => x.Title.ToLower() == text.ToLower() )
                .Count() > 0;
        }

        private OrderMenu GetRelationMenu( string title )
            => Database.Db().OrderMenus.FirstOrDefault( ( x ) => x.Title.ToLower() == title.ToLower() );

        private ReplyKeyboardMarkup BuildOrderMenu()
        {
            var keyboardItems = new List<KeyboardButton[]>();
            using var context = Database.Db();
            var menus = context.OrderMenus;

            foreach ( var menu in menus )
            {
                keyboardItems.Add( new KeyboardButton[] { menu.Title } );
            }

            return new ReplyKeyboardMarkup(
                keyboard: keyboardItems.ToArray(),
                resizeKeyboard: true
            );
        }

        private ReplyKeyboardMarkup BuildItemsMenu(List<OrderItem> items)
        {
            var keyboardItems = new List<KeyboardButton[]>();
            
            foreach ( var item in items )
            {
                keyboardItems.Add( new KeyboardButton[] { item.Title } );
            }

            return new ReplyKeyboardMarkup(
                keyboard: keyboardItems.ToArray(),
                resizeKeyboard: true
            );
        }
    }
}
