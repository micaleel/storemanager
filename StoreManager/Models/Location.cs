using System.Collections.Generic;

namespace StoreManager.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Movement> Movements { get; set; } 
    }
}