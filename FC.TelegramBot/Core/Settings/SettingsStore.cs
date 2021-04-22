using System.Collections.Generic;
using System.Text.Json.Serialization;
using Extensions.Object;
using Extensions.Object.Exceptions;

namespace FC.TelegramBot.Core.Settings
{
    public class SettingsStore
    {
        [JsonPropertyName( "Settings" )]
        public Dictionary<string, Dictionary<string, string>> Store { get; set; } 
            = new Dictionary<string, Dictionary<string, string>>();

        public int Size()
            => Store.Count;

        public bool Exists(string key, string subkey = "")
        {
            bool result = false;
            result |= Store.ContainsKey( key );

            if(subkey != "")
            {
                result |= Store[ key ].ContainsKey( subkey );
            }

            return result;
        }

        public void Delete(string key, string subkey = "")
        {
            if ( Exists( key, subkey ) )
            {
                if ( key != "" )
                {
                    Store[ key ].Remove( subkey );
                }
                else
                {
                    Store.Remove( key );
                }
            }
        }

        public void Push(string key, string subkey, string value)
        {
            this[ key, subkey ] = value;
        }
        public void Push(string key, Dictionary<string, string> value)
        {
            Delete( key );
            Store.Add( key, value );
        }

        public object this[string key, string subkey = ""]
        {
            get
            {
                ExObject.ThrowUnless( Exists( key, subkey ), 
                    new ExException( "Invalid key for settings store" ) 
                );

                if(subkey == "")
                {
                    return Store[ key ];
                }

                return Store[ key ][ subkey ];
            }

            set
            {
                Delete( key, subkey );
                Push( key, subkey, value.Convert<string>() );
            }
        }
    }
}
