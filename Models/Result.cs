using System;
using System.ComponentModel.DataAnnotations;

namespace Lingo.Models
{
    public class Result
    {
        [Key]
        [Required]
        public int ResultId {get; set;}
        [Required]
        public string Language { get; set; }
        [Required]
        public string Page { get; set; }
        public int Score { get; set; }       
        public int UserId { get; set; }    
        public DateTime Created { get; set; }
    
    }
}