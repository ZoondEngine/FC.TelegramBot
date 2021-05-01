using System;
using Extensions.Object;
using System.IO;

namespace FC.TelegramBot.Core.Logging
{
    public class ExLoggingObject : ExObject
    {
        public ExLoggingObject()
            : base()
        { }

        public ExLoggingObject( string tag )
            : base( tag )
        { }

        public void Error( string message )
            => Write( "ERR", message );
        public void Warning( string message )
            => Write( "WARN", message );
        public void Debug( string message )
            => Write( "TRACE", message );

        private void Write(string prefix, string message)
        {
            if ( !Directory.Exists( "logs" ) )
            {
                Directory.CreateDirectory( "logs" );
            }

            File.AppendAllText(
                "logs/" + DateTime.Now.ToString( "dd__MM__yyyy__HH_mm" ) + ".txt",
                FormatPrefix( prefix ) + message + Environment.NewLine
            );
        }

        private string FormatPrefix( string prefix )
            => $"[{DateTime.Now:dd/MM/yyyy HH:mm:ss.F}][{prefix}]: ";
    }
}
