using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    [Table("Etiket")]
    public class Etiket
    {
        public int ID { get; set; }
        public int imageIndex { get; set; }
        public int choice { get; set; }
        public int cursorCol { get; set; }
        public int cursorRow { get; set; }
    }
}
