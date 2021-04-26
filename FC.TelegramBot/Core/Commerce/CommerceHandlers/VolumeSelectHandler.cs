using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extensions.Object;
using FC.TelegramBot.Core.Database;
using FC.TelegramBot.Core.Database.Models;
using FC.TelegramBot.Core.Words;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FC.TelegramBot.Core.Commerce.CommerceHandlers
{
    public class VolumeSelectHandler : ICommerceHandler
    {
        private readonly ExDatabaseObject Database;
        private readonly ExWordsObject Words;

        public VolumeSelectHandler()
        {
            Database = ExObject.FindObjectOfType<ExDatabaseObject>();
            Words = ExObject.FindObjectOfType<ExWordsObject>();
        }

        public void Execute( Telegram.Bot.ITelegramBotClient client, Message message )
        {
            var item = GetRelationItem( message.Text );

            if ( item != null ) 
            {
                using var db = Database.Db();
                var volumes = db.OrderItemVolumes.Where( ( x ) => x.ItemId == item.Id );
                
                if ( volumes.Count() > 0 )
                {
                    var user = db.Users.FirstOrDefault( ( x ) => x.ExternalId == message.From.Id );
                    var cart = ExObject.FindObjectOfType<ExCommerceObject>().Cart();

                    if ( cart.UserOrderingNow( message.From.Id ) )
                    {
                        var order = cart.GetOrder( message.From.Id );
                        order.AddItem( new Models.OrderedItem( item.Title, 1, item.Price ) );

                        client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: item.Description,
                            parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                            replyMarkup: BuildVolumesMenu( volumes.ToList() )
                        );
                    }
                }
            }
        }

        public bool Triggered( string text )
        {
            using var db = Database.Db();

            return db.OrderItems
                .Where( ( x ) => x.Title.ToLower() == text.ToLower() )
                .Count() > 0;
        }

        private OrderItem GetRelationItem( string title )
            => Database.Db().OrderItems.FirstOrDefault( ( x ) => x.Title.ToLower() == title.ToLower() );

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

        private ReplyKeyboardMarkup BuildVolumesMenu( List<OrderItemVolume> items )
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
