using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    [Table("Label")]
    public class Label
    {
        public int ID { get; set; }

        public int label { get; set; }
        public int cursorCol { get; set; }
        public int cursorRow { get; set; }
        public int cursorSize { get; set; }

        [ForeignKey(nameof(Photo))]
        public int PhotoID { get; set; }
        public Photo Photo { get; set; }
    }
}
