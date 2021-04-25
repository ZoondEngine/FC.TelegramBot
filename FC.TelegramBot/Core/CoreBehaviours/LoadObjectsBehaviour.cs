using System.Collections.Generic;
using System.Linq;
using Extensions.Object;
using FC.TelegramBot.Core.Commerce;
using FC.TelegramBot.Core.Database;
using FC.TelegramBot.Core.Eventing;
using FC.TelegramBot.Core.Messaging;
using FC.TelegramBot.Core.Settings;
using FC.TelegramBot.Core.Terminal;
using FC.TelegramBot.Core.Words;

namespace FC.TelegramBot.Core.CoreBehaviours
{
    public class LoadObjectsBehaviour : ExBehaviour
    {
        private readonly List<ExObject> LoadedObjects = new List<ExObject>();

        public void Load()
        {
            LoadedObjects.Add( Instantiate<ExEventObject>() );
            LoadedObjects.Add( Instantiate<ExTerminalObject>() );
            LoadedObjects.Add( Instantiate<ExSettingsObject>() );
            LoadedObjects.Add( Instantiate<ExWordsObject>() );
            LoadedObjects.Add( Instantiate<ExDatabaseObject>() );
            LoadedObjects.Add( Instantiate<ExMessagingObject>() );
            LoadedObjects.Add( Instantiate<ExCommerceObject>() );
        }

        public T Find<T>() where T : ExObject
            => ( T ) LoadedObjects.First( ( x ) => x.GetType() == typeof( T ) );
        public T[] All<T>() where T : ExObject
            => ( T[] ) LoadedObjects.Where( ( x ) => x.GetType() == typeof( T ) ).ToArray();

        public override void BeforeDestroy()
            => LoadedObjects.ForEach( ( x ) => x.Destroy() );
    }
}
