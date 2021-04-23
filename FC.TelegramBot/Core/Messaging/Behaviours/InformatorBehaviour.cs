using Extensions.Object;
using Extensions.Object.Exceptions;
using FC.TelegramBot.Core.Eventing;
using FC.TelegramBot.Core.Messaging.EventHandlers;
using FC.TelegramBot.Core.Terminal;
using Telegram.Bot.Args;

namespace FC.TelegramBot.Core.Messaging.Behaviours
{
    public class InformatorBehaviour : ExBehaviour
    {
        private ExTerminalObject Terminal;
        private ExEventObject EventSystem;

        public override void Awake()
        {
            Terminal = FindObjectOfType<ExTerminalObject>();
            EventSystem = FindObjectOfType<ExEventObject>();

            ThrowIf(
                Terminal == null || EventSystem == null,
                new ExException("Terminal or EventSystem can't be a null for message informator")
            );

            EventSystem.Subscribe( nameof( OnMessageHandled ), OnMessageHandled );
            EventSystem.Subscribe( nameof( OnStartMessageReceived ), OnStartMessageReceived );
            EventSystem.Subscribe( nameof( OnSpamDetected ), OnSpamDetected );
            EventSystem.Subscribe( nameof( OnMessageHandleError ), OnMessageHandleError );
            EventSystem.Subscribe( nameof( OnUnrecognizableMessage ), OnUnrecognizableMessage );
        }

        public override void BeforeDestroy()
        {
            EventSystem.Unsubscribe( nameof( OnMessageHandled ), OnMessageHandled );
            EventSystem.Unsubscribe( nameof( OnStartMessageReceived ), OnStartMessageReceived );
            EventSystem.Unsubscribe( nameof( OnSpamDetected ), OnSpamDetected );
            EventSystem.Unsubscribe( nameof( OnMessageHandleError ), OnMessageHandleError );
            EventSystem.Unsubscribe( nameof( OnUnrecognizableMessage ), OnUnrecognizableMessage );
        }

        private void OnMessageHandled(CoreEvent args)
        {
            var message = args.Args[ 0 ].Convert<Telegram.Bot.Types.Message>();

            Terminal.Write().Success( 
                $"Message: '{message.Text}' from '@{message.From.Username}' handled!" 
            );
            Log.Trace( $"Message: '{message.Text}' from '@{message.From.Username}' handled!" );
        }

        private void OnStartMessageReceived( CoreEvent args )
        {
            var message = args.Args[ 0 ].Convert<Telegram.Bot.Types.Message>();

            Terminal.Write().Warning(
                $"Searching user '@{message.From.Username}' in database ..."
            );

            OnStartUserFindEvent.Handle( message );
        }

        private void OnSpamDetected( CoreEvent args )
        {

        }

        private void OnMessageHandleError( CoreEvent args )
        {
            var message = args.Args[ 0 ].Convert<Telegram.Bot.Types.Message>();

            Terminal.Write().Error(
                $"Error handling message '{message.Text}' from '@{message.From.Username}'"
            );

            Log.Error( $"Error handling message '{message.Text}' from '@{message.From.Username}'" );
        }

        private void OnUnrecognizableMessage( CoreEvent args )
        {
            var message = args.Args[ 0 ].Convert<Telegram.Bot.Types.Message>();

            Terminal.Write().Error(
                $"Message '{message.Text}' from '@{message.From.Username}' can't be recognizable"
            );

            Log.Error( $"Message '{message.Text}' from '@{message.From.Username}' can't be recognizable" );
        }
    }
}
