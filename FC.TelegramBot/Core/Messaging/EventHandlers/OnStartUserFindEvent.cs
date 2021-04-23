using System.Linq;
using Extensions.Object;
using FC.TelegramBot.Core.Database;
using FC.TelegramBot.Core.Settings;
using FC.TelegramBot.Core.Terminal;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace FC.TelegramBot.Core.Messaging.EventHandlers
{
    public static class OnStartUserFindEvent
    {
        public static void Handle(Message message)
        {
            var db = ExObject.FindObjectOfType<ExDatabaseObject>().Db();
            var settings = ExObject.FindObjectOfType<ExSettingsObject>();
            var user = db.Users.Where( 
                ( x ) => x.UserName == message.From.Username 
                && x.ExternalId == message.From.Id 
            );

            var terminal = ExObject.FindObjectOfType<ExTerminalObject>();

            if(user.Count() > 0)
            {
                terminal.Write().Success( $"User '@{message.From.Username}' already exists in database, don't need to write him here" );
            }
            else
            {
                terminal.Write().Warning( $"User '@{message.From.Username}' not found, creating ..." );
                var state = db.Users.Add( new Database.Models.User()
                {
                    ExternalId = message.From.Id,
                    IsBot = message.From.IsBot,
                    FirstName = message.From.FirstName,
                    LastName = message.From.LastName,
                    UserName = message.From.Username,
                    LanguageCode = message.From.LanguageCode,
                    RoleId = int.Parse( settings.Get( "user", "role-id-creation" ).Convert<string>() )
                });

                if(state.State == EntityState.Added)
                {
                    db.SaveChanges();

                    terminal.Write().Success( $"User '@{message.From.Username}' applied to database" );
                }
                else
                {
                    terminal.Write().Error( $"User '@{message.From.Username}' can't be applied, database error!" );
                }
            }
        }
    }
}
