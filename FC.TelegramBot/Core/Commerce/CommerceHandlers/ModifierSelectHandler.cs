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
    public class ModifierSelectHandler : ICommerceHandler
    {
        private readonly ExDatabaseObject Database;
        private readonly ExWordsObject Words;

        public ModifierSelectHandler()
        {
            Database = ExObject.FindObjectOfType<ExDatabaseObject>();
            Words = ExObject.FindObjectOfType<ExWordsObject>();
        }

        public void Execute( Telegram.Bot.ITelegramBotClient client, Message message )
        {
            using var db = Database.Db();
            var user = db.Users.FirstOrDefault( ( x ) => x.ExternalId == message.From.Id );
            var cart = ExObject.FindObjectOfType<ExCommerceObject>().Cart();
            var order = cart.GetOrder( message.From.Id );
            var volume = db.OrderItemVolumes.FirstOrDefault( ( x ) => x.Title.ToLower() == message.Text.ToLower() );

            if(order != null && volume != null)
            {
                var modifiers = db.OrderItemModifiers
                    .Where( ( x ) => x.Title.ToLower() == order.Order.Last().Item.ToLower() );

                if ( modifiers.Count() > 0 )
                {
                    if ( cart.UserOrderingNow( message.From.Id ) )
                    {
                        client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: volume.Description,
                            parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
                            replyMarkup: BuildModifiersMenu( modifiers.ToList() )
                        );
                    }
                }
                else
                {
                    // TODO: Modifiers not found, skip that step and go to finish
                }
            }
        }

        public bool Triggered( string text )
        {
            using var db = Database.Db();

            return db.OrderItemVolumes
                .Where( ( x ) => x.Title.ToLower() == text.ToLower() )
                .Count() > 0;
        }

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

        private ReplyKeyboardMarkup BuildModifiersMenu( List<OrderItemModifier> items )
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
