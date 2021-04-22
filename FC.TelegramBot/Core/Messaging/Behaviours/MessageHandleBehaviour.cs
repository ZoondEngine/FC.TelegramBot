using System.Collections.Generic;
using Extensions.Object;
using FC.TelegramBot.Core.Terminal;
using Telegram.Bot.Args;

namespace FC.TelegramBot.Core.Messaging.Behaviours
{
    public class MessageHandleBehaviour : ExBehaviour
    {
        private ExMessagingObject Parent;
        private ExTerminalObject Terminal;
        private List<IMessageHandler> Handlers = new List<IMessageHandler>();
        private Queue<MessageEventArgs> Messages = new Queue<MessageEventArgs>();

        public override void Awake()
        {
            Parent = ParentObject.Unbox<ExMessagingObject>();
            Terminal = FindObjectOfType<ExTerminalObject>();

            ThrowIf( 
                Parent == null || Terminal == null, 
                new Extensions.Object.Exceptions.ExException( "Parent object can't be a null!" ) 
            );
        }

        public override void Update()
        {
            if(Messages.Count > 0)
            {
                Terminal.Write().Success( $"Objects in queue: {Messages.Count}" );
                var message = Messages.Dequeue();

                foreach(var handler in Handlers)
                {
                    if(handler.Executable(message))
                    {
                        if (handler.Execute(Parent.GetClient(), message))
                        {
                            Terminal.Write().Debug( $"Message '{message.Message.Text}'(@{message.Message.From.Username}) proceed: true" );
                        }
                        else
                        {
                            Terminal.Write().Error( $"Can't execute message '{message.Message.Text}'(@{message.Message.From.Username}) proceed: false" );
                        }
                    }
                    else
                    {
                        Terminal.Write().Warning( $"Not found handler for message: '{message.Message.Text}'(@{message.Message.From.Username})" );
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
