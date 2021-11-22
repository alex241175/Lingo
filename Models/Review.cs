using System;
using System.ComponentModel.DataAnnotations;

namespace Lingo.Models
{
    public class Review
    {
        [Key]
        [Required]
        public int ReviewId { get; set; } 
        public int? VocabId { get; set;}
        public int? UserId { get; set;}         

    }
}