using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FC.TelegramBot.Core.Database.Models
{
    [Table( "bot_order_item_volumes" )]
    public class OrderItemVolume
    {
        [Key]
        [Column( "id" )]
        public long Id { get; set; }

        [Column( "item_id" )]
        public long ItemId { get; set; }

        [Column( "title" )]
        public string Title { get; set; }

        [Column( "description" )]
        public string Description { get; set; }

        [Column( "created_at" )]
        public DateTime? CreatedAt { get; set; }

        [Column( "updated_at" )]
        public DateTime? UpdatedAt { get; set; }
    }
}
