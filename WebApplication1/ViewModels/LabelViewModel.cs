using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class LabelViewModel
    {
        public int id { get; set; }
        public int photoID { get; set; }
        public int label { get; set; }
        public Rectangle rectangle { get; set; }
        public int sizeFactor { get; set; }
    }

    public class Rectangle
    {
        public int beginX { get; set; }
        public int endX { get; set; }
        public int beginY { get; set; }
        public int endY { get; set; }

    }
}
