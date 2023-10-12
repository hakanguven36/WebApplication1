using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    [Table("Annotation")]
    public class Annotation
    {
        public int ID { get; set; }

        public string name { get; set; }

        public string color { get; set; }

        public string textColor { get; set; }

        [ForeignKey(nameof(Project))]
        public int? ProjectID { get; set; }
        public Project Project { get; set; }
    }
}
