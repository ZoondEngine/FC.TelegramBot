using System.Collections.Generic;
using Extensions.Object;

namespace FC.TelegramBot.Core.Words
{
    public class ExWordsObject : ExObject
    {
        private Dictionary<string, string> Words = new Dictionary<string, string>()
        {
            [ "menu__coffeeOrder" ] = "Заказать кофе",
            [ "menu__ordersHistory" ] = "История заказов",
            [ "menu__freshPromotions" ] = "Свежие акции",
            [ "menu__getGift" ] = "Получить подарок",
            [ "menu__contacts" ] = "Контакты",
            [ "menu__report" ] = "Отправить жалобу",

            ["text__welcomeMessage"] = "Добро пожаловать :username! Вы можете заказать кофе из контекстного меню!",
        };

        public string Word( string key )
            => Words.ContainsKey( key ) ? Words[ key ] : "word-unknown";

        public string Word(string key, params WordPair[] replaces)
        {
            string replaced = Word( key );
            
            foreach(var item in replaces)
            {
                replaced = replaced.Replace( $":{item.Word}", item.Value );
            }

            return replaced;
        }
    }
}
