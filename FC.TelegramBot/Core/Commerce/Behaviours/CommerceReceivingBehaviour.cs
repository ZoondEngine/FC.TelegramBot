using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Extensions.Object;
using FC.TelegramBot.Core.Eventing;
using Telegram.Bot.Types;

namespace FC.TelegramBot.Core.Commerce.Behaviours
{
    public class CommerceReceivingBehaviour : ExBehaviour
    {
        private Dictionary<string, Action<Message>> CommerceHandlers;

        public override void Awake()
        {
            CommerceHandlers = new Dictionary<string, Action<Message>>();

            var handlers = Assembly.GetExecutingAssembly().GetTypes().Where( ( x ) => x.GetInterfaces().Contains( typeof( ICommerceHandler ) ) )
                                .Select( ( x ) => x.GetConstructor( Type.EmptyTypes )?.Invoke( null ) as ICommerceHandler )
                                .ToList();

            for ( var i = 0; i < handlers.Count; i++ )
            {
                CommerceHandlers.Add( handlers[ i ].Trigger(), handlers[ i ].Execute );
            }

            FindObjectOfType<ExEventObject>().Subscribe( nameof( OnReceiveCommerceMessage ), OnReceiveCommerceMessage );
        }

        public void OnReceiveCommerceMessage( CoreEvent args )
        {
            var msg = args.Args[ 1 ].Convert<Message>();

            ThrowIf( 
                msg == null, 
                new Extensions.Object.Exceptions.ExException( "Commerce message can't be a null!" ) 
            );

            if ( CommerceHandlers.ContainsKey( msg.Text.ToLower() ) )
            {
                CommerceHandlers[ msg.Text.ToLower() ]?.Invoke( msg );
            }
        }
    }
}
