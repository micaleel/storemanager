using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManager.Models
{
    public class Item
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ItemStatusId { get; set; }

        [ForeignKey("ItemStatusId")]
        public virtual ItemStatus Status { get; set; }

        public virtual List<Movement> Movements { get; set; } 
    }
}