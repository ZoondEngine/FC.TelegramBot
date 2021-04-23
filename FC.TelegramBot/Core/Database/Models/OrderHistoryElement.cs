using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FC.TelegramBot.Core.Database.Models
{
    [Table("bot_order_history")]
    public class OrderHistoryElement
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("item_id")]
        public long ItemId { get; set; }

        [Column("modifier_id")]
        public long? ModifierId { get; set; }

        [Column("volume_id")]
        public long? VolumeId { get; set; }

        [Column("summary_price")]
        public float SummaryPrice { get; set; }

        [Column("status_id")]
        public long StatusId { get; set; }

        [Column( "created_at" )]
        public DateTime? CreatedAt { get; set; }

        [Column( "updated_at" )]
        public DateTime? UpdatedAt { get; set; }
    }
}
