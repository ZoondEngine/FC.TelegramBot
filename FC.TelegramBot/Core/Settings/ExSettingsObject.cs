using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Extensions.Object;

namespace FC.TelegramBot.Core.Settings
{
    public class ExSettingsObject : ExObject
    {
        private SettingsStore Settings;

        public ExSettingsObject()
            : base()
        {
            ThrowUnless(
                Load(),
                new Extensions.Object.Exceptions.ExException( "Can't load settings module" )
            );
        }

        public ExSettingsObject(string tag)
            : base(tag)
        {
            ThrowUnless(
                Load(),
                new Extensions.Object.Exceptions.ExException( "Can't load settings module" )
            );
        }

        public override void Destroy()
            => Save();

        private bool Load()
        {
            var path = Path.Combine( "settings", "bot_config.json" );

            if ( !File.Exists( path ) ) 
            {
                File.WriteAllText( path, SerializeDefault() );
            }

            ThrowIf( !File.Exists( path ), 
                new Extensions.Object.Exceptions.ExException("Invalid argument assert") 
            );

            var data = File.ReadAllText( path );
            Settings = JsonSerializer.Deserialize<SettingsStore>( data );

            return Settings.Size() > 0;
        }

        private void Save()
        {
            var path = Path.Combine( "settings", "bot_config.json" );

            if ( File.Exists( path ) )
            {
                File.Delete( path );
            }

            File.WriteAllText( path, JsonSerializer.Serialize( Settings ) );
        }

        private string SerializeDefault()
        {
            var data = new SettingsStore();

            data.Push( "api", new Dictionary<string, string>()
            {
                ["key"] = "1653818201:AAF7nB72uiljkyR2Vb6xulQwYq4ggWbzlHY",
            });

            data.Push( "database", new Dictionary<string, string>()
            {
                [ "host" ] = "127.0.0.1",
                [ "user" ] = "root",
                [ "password" ] = "password_here",
                [ "db" ] = "fc-database",
            });

            return JsonSerializer.Serialize( data );
        }

        public object Get( string key, string subkey = "" )
            => Settings[ key, subkey ];
        public void Set( string key, string subkey, string value )
            => Settings.Push( key, subkey, value );
        public void Set( string key, Dictionary<string, string> value )
            => Settings.Push( key, value );
    }
}
