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

        [ForeignKey(nameof(HamResim))]
        public int HamResimID { get; set; }
        public HamResim HamResim { get; set; }

        public int choice { get; set; }
        public int cursorCol { get; set; }
        public int cursorRow { get; set; }
        public int cursorSize { get; set; } = 128;
    }
}
