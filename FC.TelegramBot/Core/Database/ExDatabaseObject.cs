using Extensions.Object;
using FC.TelegramBot.Core.Settings;

namespace FC.TelegramBot.Core.Database
{
    public class ExDatabaseObject : ExObject
    {
        public ExSettingsObject Settings;

        public ExDatabaseObject()
            : base()
        {
            Settings = FindObjectOfType<ExSettingsObject>();
        }

        public DatabaseContext Db()
        {
            return new DatabaseContext(
                Settings.Get( "database", "host" ).Convert<string>(),
                Settings.Get( "database", "user" ).Convert<string>(),
                Settings.Get( "database", "password" ).Convert<string>(),
                Settings.Get( "database", "db" ).Convert<string>()
            );
        }
    }
}
