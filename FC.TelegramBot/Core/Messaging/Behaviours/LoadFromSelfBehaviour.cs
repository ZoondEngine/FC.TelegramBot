using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Extensions.Object;

namespace FC.TelegramBot.Core.Messaging.Behaviours
{
    public class LoadFromSelfBehaviour : ExBehaviour
    {
        private ExMessagingObject Parent;

        public override void Awake()
        {
            Parent = ParentObject.Unbox<ExMessagingObject>();

            LoadFromSelf();
        }

        private void LoadFromSelf()
        {
            var handlers = Assembly.GetExecutingAssembly().GetTypes().Where( ( x ) => x.GetInterfaces().Contains( typeof( IMessageHandler ) ) )
                                .Select( ( x ) => x.GetConstructor( Type.EmptyTypes )?.Invoke( null ) as IMessageHandler )
                                .ToList();

            var handler = Parent.GetComponent<MessageHandleBehaviour>();
            handlers.ForEach( handler.ApproveHandler );
        }
    }
}
