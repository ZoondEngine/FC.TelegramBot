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

            [ "text__welcomeMessage" ] = "Добро пожаловать :username! Вы можете заказать кофе из контекстного меню!",

            [ "order__orderAlreadyExists" ] = "В данный момент заказ :order уже в обработке. Ожидайте, пожалуйста",
            [ "order__menuNotFound" ] = "Указанное меню не найдено. Пожалуйста, повторите попытку",
            [ "order__menuItemsEmpty" ] = "В данный момент позиции в этом меню отсутствуют. Пожалуйста, выберите другое меню",
            [ "order__orderStart" ] = "Я готов принять ваш заказ. <b>Идентификатор заказа :orderId</b>. Выберите меню из списка ниже, чтобы приступить к оформлению заказа",
            [ "order__prevOrderNotCompleted" ] = "Предыдущий заказ еще <b>не завершен</b>. <br>Вы не можете открыть новую заявку!",
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
