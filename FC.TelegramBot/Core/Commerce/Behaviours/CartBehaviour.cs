using System;
using System.Collections.Generic;
using Extensions.Object;
using FC.TelegramBot.Core.Commerce.Models;

namespace FC.TelegramBot.Core.Commerce.Behaviours
{
    public class CartBehaviour : ExBehaviour
    {
        private Dictionary<long, CartItem> Cart;

        public override void Awake()
        {
            Cart = new Dictionary<long, CartItem>();
        }

        public override void Update()
        {
            if ( ( CurrentTickTime - PreviousTickTime ).Minutes > 3 )
            {
                Dictionary<long, CartItem> newCart
                    = new Dictionary<long, CartItem>();

                foreach ( var item in Cart )
                {
                    var delta = ( DateTime.Now - item.Value.CreatedAt );
                    if ( delta.Minutes < 30 )
                    {
                        newCart.Add( item.Key, item.Value );
                    }
                }

                Cart = newCart;
            }
        }

        public bool UserOrderingNow( long userId )
            => Cart.ContainsKey( userId );

        public CartItem GetOrder( long userId )
        {
            if(UserOrderingNow(userId))
            {
                return Cart[ userId ];
            }

            return default;
        }

        public void AddCartItem(CartItem item)
        {
            ThrowIf( 
                item == null, 
                new Extensions.Object.Exceptions.ExException( "Cart item can't be a null!" ) 
            );

            if ( !Cart.ContainsKey( item.TelegramUserId ) )
            {
                Cart.Add( item.TelegramUserId, item );
            }
        }

        public void AddOrder(OrderedItem item, long userId)
        {
            ThrowIf(
                item == null,
                new Extensions.Object.Exceptions.ExException( "Order can't be a null!" )
            );

            if(Cart.ContainsKey(userId))
            {
                Cart[ userId ].AddItem( item );
            }
        }

        public void Clean(long userId)
        {
            if ( Cart.ContainsKey( userId ) )
            {
                Cart.Remove( userId );
            }
        }
    }
}
