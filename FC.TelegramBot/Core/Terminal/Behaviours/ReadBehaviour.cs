using System;
using Extensions.Object;

namespace FC.TelegramBot.Core.Terminal.Behaviours
{
    public class ReadBehaviour : ExBehaviour
    {
        //TODO: make command listener

        public T GetLineAs<T>()
        {
            var line = Console.ReadLine();
            if ( !string.IsNullOrWhiteSpace( line ) && !string.IsNullOrEmpty( line ) )
            {
                try
                {
                    return ( T ) Convert.ChangeType( line, typeof( T ) );
                }
                catch
                {
                    return default;
                }
            }

            return default;
        }
    }
}
