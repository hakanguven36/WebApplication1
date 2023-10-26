using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    [Table("Photo")]
    public class Photo
    {
        public int ID { get; set; }

        public string sysname { get; set; }

        public string orjname { get; set; }

        public string extention { get; set; }

        public string contentType { get; set; }

        public double sizeMB { get; set; }

        public string imageFormat { get; set; }

        public bool completed { get; set; }

        public string labels { get; set; }

        [ForeignKey(nameof(Project))]
        public int ProjectID { get; set; }
        public Project Project { get; set; }

        public DateTime date { get; set; }
    }
}
