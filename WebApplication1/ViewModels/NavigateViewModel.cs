using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class NavigateViewModel
    {
        public int projectID { get; set; }
        public int photoID { get; set; }
        public int photoNo { get; set; }
        public int seen { get; set; } // or completed
        public int filesCount { get; set; }
        public string path { get; set; }
        public string labels { get; set; }
        public string error { get; set; }
    }
}
