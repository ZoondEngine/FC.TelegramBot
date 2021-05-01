using System.Linq;
using Extensions.Object;
using FC.TelegramBot.Core.Commerce.Models;
using FC.TelegramBot.Core.Database;
using FC.TelegramBot.Core.Words;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace FC.TelegramBot.Core.Commerce.CommerceHandlers
{
    public class OrderFinishHandler : ICommerceHandler
    {
        private ExDatabaseObject Database;
        private ExWordsObject Words;

        public OrderFinishHandler()
        {
            Database = ExObject.FindObjectOfType<ExDatabaseObject>();
            Words = ExObject.FindObjectOfType<ExWordsObject>();
        }

        public void Execute(ITelegramBotClient client, Message message)
        {
            using var db = Database.Db();
            var user = db.Users.FirstOrDefault((x) => x.ExternalId == message.From.Id);
            var cart = ExObject.FindObjectOfType<ExCommerceObject>().Cart();
            var order = cart.GetOrder(message.From.Id);
            var modifier = db.OrderItemModifiers.FirstOrDefault((x) => x.Title.ToLower() == message.Text.ToLower());

            if(order != null && modifier != null)
            {
                order.Order.First().Modifier = modifier.Title;

                var description = Words.Word(
                    "order__descriptionOrder",
                    new WordPair("sortedContainer", BuildSortedOrder(order)),
                    new WordPair("orderPrice", order.SummaryPrice.ToString())
                );
            }
        }

        public bool Triggered(string text)
        {
            using var db = Database.Db();

            return db.OrderItemModifiers.Where((x) => x.Title.ToLower() == text.ToLower()).Count() > 0;
        }

        public ReplyKeyboardMarkup BuildApproveMenu()
        {
            return new ReplyKeyboardMarkup(
                new KeyboardButton[][]
                {
                    new KeyboardButton[]
                    {
                        Words.Word( "order__approveOrder" ), Words.Word("order__declineOrder")
                    },
                    new KeyboardButton[]
                    {
                        Words.Word("order__addNewItem")
                    }
                },

                resizeKeyboard: true
            );
        }

        public string BuildSortedOrder(CartItem userCart)
        {
            string htmlLine = "<ul>";

            for(var i = 0; i < userCart.Order.Count(); i++)
            {
                var currentItem = userCart.Order[i];

                htmlLine = $"<li>{currentItem.Item}({currentItem.Modifier}), {currentItem.Volume}</li>";
            }

            return htmlLine + "</ul>";
        }
    }
}
