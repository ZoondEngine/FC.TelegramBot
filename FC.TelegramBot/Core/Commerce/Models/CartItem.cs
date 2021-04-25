using System;
using System.Collections.Generic;

namespace FC.TelegramBot.Core.Commerce.Models
{
    public class CartItem
    {
        public long UserId;
        public long TelegramUserId;
        public long OrderId;
        public List<OrderedItem> Order;
        public float SummaryPrice;
        public DateTime CreatedAt;

        public CartItem(long userId, long telegramUserId)
        {
            UserId = userId;
            TelegramUserId = telegramUserId;
            OrderId = new Random().Next( 10000, 214767546 );

            Order = new List<OrderedItem>();
            SummaryPrice = 0.0f;
            CreatedAt = DateTime.Now;
        }

        public void AddItem( OrderedItem item )
        {
            Order.Add( item );

            Recalculate();
        }

        public void Recalculate()
        {
            SummaryPrice = 0.0f;

            for ( var i = 0; i < Order.Count; i++ )
            {
                SummaryPrice += Order[ i ].Price;
            }
        }
    }
}
