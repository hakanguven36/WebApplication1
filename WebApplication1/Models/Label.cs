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
        public int posX { get; set; }
        public int posY { get; set; }
        public int wid { get; set; }
        public int hei { get; set; }

        [ForeignKey(nameof(Photo))]
        public int PhotoID { get; set; }
        public Photo Photo { get; set; }

        [ForeignKey(nameof(User))]
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
