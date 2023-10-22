using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class LabelViewModel
    {
        public int id { get; set; }
        public int projectID { get; set; }
        public int photoID { get; set; }
        public int annoID { get; set; }

        public List<Point> points{ get; set; }
        public float sizeFactor { get; set; }
        public int shape { get; set; }
    }

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
