using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FC.TelegramBot.Core.Database.Models
{
    [Table("bot_order_items")]
    public class OrderItem
    {
        [Key]
        [Column( "id" )]
        public long Id { get; set; }

        [Column( "menu_id" )]
        public long MenuId { get; set; }

        [Column( "title" )]
        public string Title { get; set; }

        [Column( "description" )]
        public string Description { get; set; }

        [Column( "allow_modifiers" )]
        public bool AllowModifiers { get; set; }

        [Column( "price" )]
        public float Price { get; set; }

        [Column( "created_at" )]
        public DateTime? CreatedAt { get; set; }

        [Column( "updated_at" )]
        public DateTime? UpdatedAt { get; set; }
    }
}
