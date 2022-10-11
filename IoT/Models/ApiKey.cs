using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Models
{
    [Table("ApiKey")]
    public class ApiKey
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public bool CanWrite { get; set; }

        public List<Item> EditedItems { get; set; }
    }
}
