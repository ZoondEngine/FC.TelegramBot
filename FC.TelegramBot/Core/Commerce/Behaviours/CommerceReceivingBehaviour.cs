using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Extensions.Object;
using FC.TelegramBot.Core.Eventing;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FC.TelegramBot.Core.Commerce.Behaviours
{
    public class CommerceReceivingBehaviour : ExBehaviour
    {
        private List<ICommerceHandler> CommerceHandlers;

        public override void Awake()
        {
            CommerceHandlers = Assembly.GetExecutingAssembly().GetTypes().Where( ( x ) => x.GetInterfaces().Contains( typeof( ICommerceHandler ) ) )
                                .Select( ( x ) => x.GetConstructor( Type.EmptyTypes )?.Invoke( null ) as ICommerceHandler )
                                .ToList();


            FindObjectOfType<ExEventObject>().Subscribe( nameof( OnReceiveCommerceMessage ), OnReceiveCommerceMessage );
        }

        public void OnReceiveCommerceMessage( CoreEvent args )
        {
            var msg = args.Args[ 1 ].Convert<Message>();
            var client = args.Args[ 0 ].Convert<ITelegramBotClient>();

            ThrowIf( 
                msg == null || client == null, 
                new Extensions.Object.Exceptions.ExException( "Commerce message can't be a null!" ) 
            );

            foreach(var handler in CommerceHandlers)
            {
                if ( handler.Triggered( msg.Text ) )
                {
                    handler.Execute( client, msg );
                }
            }
        }
    }
}
