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

        [ForeignKey(nameof(Photo))]
        public int photoID { get; set; }
        public Photo Photo { get; set; }

        public List<Coordinate> points { get; set; }
        public double sizeFactor { get; set; }
        public SHAPE shape { get; set; }

        public int annoID { get; set; }
        public int userID { get; set; }
        public int projectID { get; set; }
    }

    [Table("Coordinate")]
    public class Coordinate
    {
        public int ID { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        [ForeignKey(nameof(Label))]
        public int LabelID { get; set; }
        public Label Label { get; set; }
    }

    public enum SHAPE{
        rectangle,
        circle,
        poligon
    }
}
