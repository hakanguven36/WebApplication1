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

        public int beginX { get; set; }
        public int beginY { get; set; }
        public int endX { get; set; }
        public int endY { get; set; }

        [ForeignKey(nameof(Annotation))]
        public int AnnotationID { get; set; }
        public Annotation Annotation { get; set; }

        [ForeignKey(nameof(Photo))]
        public int PhotoID { get; set; }
        public Photo Photo { get; set; }

        [ForeignKey(nameof(User))]
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
