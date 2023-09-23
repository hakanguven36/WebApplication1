using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    [Table("UserSetting")]
    public class UserSetting
    {
        public int ID { get; set; }

        [ForeignKey(nameof(User))]
        public int UserID { get; set; }
        public User User { get; set; }

        public int canvasSize { get; set; }

        public int ucanCanvasSize { get; set; }

        public SEENORWHAT seenOrWhat { get; set; }
    }


}
