using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    [Table("Proje")]
    public class Proje
    {
        public int ID { get; set; }

        public string name { get; set; }

    }
}
