using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FC.TelegramBot.Core.Database.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("external_id")]
        public int ExternalId { get; set; }

        [Column("is_bot")]
        public bool IsBot { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("user_name")]
        public string UserName { get; set; }

        [Column("language_code")]
        public string LanguageCode { get; set; }
    }
}
