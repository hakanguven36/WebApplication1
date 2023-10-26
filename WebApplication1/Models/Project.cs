using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    [Table("Project")]
    public class Project
    {
        public int ID { get; set; }
        public string name { get; set; }

        public List<Annotation> annoList { get; set; }

        public List<Photo> photos { get; set; }
    }
}
