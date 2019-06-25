using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IoT.Models;

namespace IoT
{
    public class Item
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsPublic { get; set; }

        public Collection Collection { get; set; }
        public int CollectionFK { get; set; }
    }
}
