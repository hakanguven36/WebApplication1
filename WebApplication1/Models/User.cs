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

        private string _password;

        [StringLength(12, MinimumLength = 4, ErrorMessage = "4-12 Karakter olmalı!")]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        public string password
        {
            get => _password.encout();
            set => _password = value.encin();
        }

        public int hatali { get; set; }

        public bool kilitli { get; set; }

    }
}
