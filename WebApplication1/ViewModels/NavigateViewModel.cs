using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class NavigateViewModel
    {
        public int projectID { get; set; }
        public int imageID { get; set; }
        public int imageNo { get; set; }
        public bool? seen { get; set; } // or completed
        public int filesCount { get; set; }
        public string path { get; set; }
        public string error { get; set; }
    }
}
