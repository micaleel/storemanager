using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManager.Models
{
    public class Movement
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int ItemId { get; set; }
        public int LocationId { get; set; }
        public string Notes { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }

        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }
    }
}