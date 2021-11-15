using System;
using System.ComponentModel.DataAnnotations;

namespace Lingo.Models
{
    public class User
    {
        public int UserId { get; set; } 
        [Display(Name="User Name")]
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }    
        public string Role { get; set; }   

    }
}