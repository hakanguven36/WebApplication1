using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelSeperator.Models
{
    public class Project
    {
        public string projectName { get; set; }

        public List<string> annoList { get; set; }

        public List<Photo> photos { get; set; }
    }

    public class Photo
    {
        public string photoName { get; set; }
        
        public List<Label> photoLabels { get; set; }
    }

    public class Label
    {
        public int annoID { get; set; }

        public List<Point> points { get; set; }
    }
    public class Point
    {
        public int x { get; set; }

        public int y { get; set; }
    }
}
