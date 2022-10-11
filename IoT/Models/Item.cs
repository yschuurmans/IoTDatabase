using IoT.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoT
{
    [Table("Item")]
    public class Item
    {
        public int Id { get; set; }
        [MaxLength(1000)]
        public string Key { get; set; }
        [MaxLength(1000)]
        public string Value { get; set; }

        public Collection Collection { get; set; }
        public int CollectionFK { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public ApiKey LastUpdater { get; set; }
        public int LastUpdaterFK { get; set; }
    }
}
