using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KembimValutor.Models
{
    public class users
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string Birthday { get; set; }
        public string Email { get; set; }
        public char Type { get; set; }
    }
}