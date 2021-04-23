using Extensions.Object;
using Extensions.Object.Attributes;
using FC.TelegramBot.Core.Messaging.Behaviours;
using FC.TelegramBot.Core.Settings;
using FC.TelegramBot.Core.Terminal;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace FC.TelegramBot.Core.Messaging
{
    [RequiredBehaviour(typeof(MessageHandleBehaviour))]
    [RequiredBehaviour(typeof(LoadFromSelfBehaviour))]
    [RequiredBehaviour(typeof(LoadFromAssembliesBehaviour))]
    [RequiredBehaviour(typeof(InformatorBehaviour))]
    public class ExMessagingObject : ExObject
    {
        private ITelegramBotClient BotClient;
        private ExSettingsObject Settings;
        private ExTerminalObject Terminal;

        public ExMessagingObject()
            : base()
        {
            Load();
        }
        public ExMessagingObject( string tag )
            : base( tag )
        {
            Load();
        }

        public void Load()
        {
            Settings = FindObjectOfType<BotContext>().Child<ExSettingsObject>();
            Terminal = FindObjectOfType<BotContext>().Child<ExTerminalObject>();

            ThrowIf( Settings == null, new Extensions.Object.Exceptions.ExException( "Settings can't be a null" ) );

            BotClient = new TelegramBotClient( Settings.Get( "api", "key" ).Convert<string>() );

            var me = BotClient.GetMeAsync();
            Terminal.Write().Success( $"Bot connected, ID: {me.Id}" );

            BotClient.StartReceiving();
            BotClient.OnMessage += OnMessageHandler;
            BotClient.OnReceiveError += OnErrorHandler;
            BotClient.OnReceiveGeneralError += OnGeneralErrorHandler;
        }

        public void Stop()
        {
            BotClient.OnMessage -= OnMessageHandler;
            BotClient.OnReceiveError -= OnErrorHandler;
            BotClient.OnReceiveGeneralError -= OnGeneralErrorHandler;
            BotClient.StopReceiving();
        }

        public void Reload()
        {
            Stop();
            Load();
        }

        private void OnMessageHandler( object sender, MessageEventArgs e )
            => GetComponent<MessageHandleBehaviour>().ApproveMessage( e );

        private void OnErrorHandler( object sender, ReceiveErrorEventArgs e )
        {
            Terminal.Write().Error( $"Error recevied: {e.ApiRequestException}" );
            Log.Error(e.ApiRequestException.ToString());
        }

        private void OnGeneralErrorHandler( object sender, ReceiveGeneralErrorEventArgs e )
        {
            Terminal.Write().Error( $"!! Fatal error occured: {e.Exception}" );
            Log.Error( "FATAL -- " + e.Exception.ToString() );
        }

        public ITelegramBotClient GetClient()
            => BotClient;

        public bool Valid()
            => BotClient != null;
    }
}
