using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Araclar;

namespace WebApplication1.Models
{
    [Table("User")]
    public class User
    {
        public int ID { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "4-30 Karakter olmalı!")]
        public string username { get; set; }

        public string passwordEnc { get; set; }

        [NotMapped]
        [StringLength(12, MinimumLength = 4, ErrorMessage = "4-12 Karakter olmalı!")]
        public string password
        {
            get => passwordEnc.encout();
            set => passwordEnc = value.encin();
        }

        public int hatali { get; set; }

        public bool kilitli { get; set; }

        public bool admin { get; set; }

    }
}
