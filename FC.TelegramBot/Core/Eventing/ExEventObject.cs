using System;
using System.Collections.Generic;
using Extensions.Object;

namespace FC.TelegramBot.Core.Eventing
{
    public class ExEventObject : ExObject
    {
        public Dictionary<string, List<Action<CoreEvent>>> Events 
            = new Dictionary<string, List<Action<CoreEvent>>>();

        public ExEventObject()
            : base()
        { }

        public ExEventObject(string tag)
            : base(tag)
        { }

        /// <summary>
        /// Calling the events stack
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        public void Call( string name, CoreEvent args )
        {
            if ( Exists( name ) )
            {
                var @event = Events[ name ];

                for ( var i = 0; i < @event.Count; i++ )
                {
                    @event[ i ].Invoke( args );
                }
            }
        }

        /// <summary>
        /// Check if event existing
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exists( string name )
        {
            return Events.ContainsKey( name );
        }

        /// <summary>
        /// Subscribe on event if not subscribed
        /// </summary>
        /// <param name="name"></param>
        /// <param name="handler"></param>
        public void Subscribe(string name, Action<CoreEvent> handler)
        {
            if ( !Exists( name ) ) 
            {
                Register( name );
            }

            if(!Events[name].Contains(handler))
            {
                Events[ name ].Add( handler );
            }
        }

        /// <summary>
        /// Unsibscribe if already subscribed
        /// </summary>
        /// <param name="name"></param>
        /// <param name="handler"></param>
        public void Unsubscribe(string name, Action<CoreEvent> handler)
        {
            if ( Exists( name ) )
            {
                if ( Events[ name ].Contains( handler ) )
                {
                    Events[ name ].Remove( handler );
                }
            }
        }

        /// <summary>
        /// Register event if not registered
        /// </summary>
        /// <param name="name"></param>
        public void Register( string name )
        {
            if ( !Exists( name ) )
            {
                Events.Add( name, new List<Action<CoreEvent>>() );
            }
        }

        /// <summary>
        /// Remove event if registered
        /// </summary>
        /// <param name="name"></param>
        public void Remove( string name )
        {
            if ( Exists( name ) )
            {
                Events.Remove( name );
            }
        }
    }
}
