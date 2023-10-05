using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class LabelViewModel
    {
        public int id { get; set; }
        public int fotoID { get; set; }
        public int label { get; set; }
        public int cursorCol { get; set; }
        public int cursorRow { get; set; }
        public int cursorSize { get; set; }
    }
}
