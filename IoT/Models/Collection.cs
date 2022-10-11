using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoT.Models
{
    [Table("Collection")]
    public class Collection
    {
        public Collection()
        {
            Items = new List<Item>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ApiKey Owner { get; set; }

        public List<Item> Items { get; set; }

    }
}
