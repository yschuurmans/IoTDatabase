using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Models
{
    public class ApiKey
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public bool CanWrite { get; set; }

        public List<Item> EditedItems { get; set; }
    }
}
