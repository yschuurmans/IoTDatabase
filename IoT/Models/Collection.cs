using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Models
{
    public class Collection
    {
        public Collection()
        {
            Items = new List<Item>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public List<Item> Items { get; set; }

    }
}
