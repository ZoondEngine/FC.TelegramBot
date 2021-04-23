using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FC.TelegramBot.Core.Database.Models
{
    [Table( "bot_users" )]
    public class User
    {
        [Key]
        [Column( "id" )]
        public long Id { get; set; }

        [Column( "external_id" )]
        public long ExternalId { get; set; }

        [Column( "is_bot" )]
        public bool IsBot { get; set; }

        [Column( "first_name" )]
        public string FirstName { get; set; }

        [Column( "last_name" )]
        public string LastName { get; set; }

        [Column( "user_name" )]
        public string UserName { get; set; }

        [Column( "language_code" )]
        public string LanguageCode { get; set; }

        [Column( "role_id" )]
        public long RoleId { get; set; }

        [Column("balance")]
        public float Balance { get; set; }

        [Column( "created_at" )]
        public DateTime? CreatedAt { get; set; }

        [Column( "updated_at" )]
        public DateTime? UpdatedAt { get; set; }
    }
}
