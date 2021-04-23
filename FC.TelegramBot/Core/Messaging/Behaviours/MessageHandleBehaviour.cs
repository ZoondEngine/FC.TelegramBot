using System.Collections.Generic;
using Extensions.Object;
using Extensions.Object.Exceptions;
using FC.TelegramBot.Core.Eventing;
using FC.TelegramBot.Core.Terminal;
using Telegram.Bot.Args;

namespace FC.TelegramBot.Core.Messaging.Behaviours
{
    public class MessageHandleBehaviour : ExBehaviour
    {
        private ExMessagingObject Parent;
        private ExEventObject EventSystem;

        private readonly List<IMessageHandler> Handlers 
            = new List<IMessageHandler>();
        private readonly Queue<MessageEventArgs> Messages 
            = new Queue<MessageEventArgs>();

        public override void Awake()
        {
            Parent = ParentObject.Unbox<ExMessagingObject>();
            EventSystem = FindObjectOfType<ExEventObject>();

            ThrowIf( 
                Parent == null || EventSystem == null, 
                new ExException( "Parent object can't be a null!" ) 
            );

            EventSystem.Register( "OnMessageHandled" );
            EventSystem.Register( "OnStartMessageReceived" );
            EventSystem.Register( "OnSpamDetected" );
            EventSystem.Register( "OnMessageHandleError" );
            EventSystem.Register( "OnUnrecognizableMessage" );
        }

        public override void Update()
        {
            if(Messages.Count > 0)
            {
                var message = Messages.Dequeue();

                foreach(var handler in Handlers)
                {
                    if(handler.Executable(message))
                    {
                        if (handler.Execute(Parent.GetClient(), message))
                        {
                            EventSystem.Call(
                                "OnMessageHandled",
                                new CoreEvent( this, new List<object>() { message.Message, handler } )
                            );
                        }
                        else
                        {
                            EventSystem.Call( 
                                "OnMessageHandleError", 
                                new CoreEvent( this, new List<object>() { message.Message, handler } ) 
                            );
                        }
                    }
                    else
                    {
                        EventSystem.Call( 
                            "OnUnrecognizableMessage", 
                            new CoreEvent( this, new List<object>() { message.Message } ) 
                        );
                    }
                }
            }
        }

        public void ApproveMessage( MessageEventArgs message )
            => Messages.Enqueue( message );

        public void ApproveHandler( IMessageHandler handler )
            => Handlers.Add( handler );
    }
}
