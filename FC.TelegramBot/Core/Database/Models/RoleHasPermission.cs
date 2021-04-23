using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FC.TelegramBot.Core.Database.Models
{
    [Table( "bot_role_has_permissions" )]
    public class RoleHasPermission
    {
        [Key]
        [Column( "id" )]
        public long Id { get; set; }

        [Column( "role_id" )]
        public long RoleId { get; set; }

        [Column( "permission_id" )]
        public long PermissionId { get; set; }
    }
}
