using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Label
    {
        public int photoID { get; set; }

        public int annoID { get; set; }

        public List<Point> points { get; set; }
    }

    public class Point
    {
        public double x { get; set; }

        public double y { get; set; }
    }

}
