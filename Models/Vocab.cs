using System;
using System.ComponentModel.DataAnnotations;

namespace Lingo.Models
{
    public class Vocab
    {
        [Key]
        [Required]
        public int VocabId {get; set;}
        [Required]
        public string Language { get; set; }
        [Required]
        public string Text { get; set; }
        public string Chinese { get; set; }       
        public string Note {get;set;}
        public bool Review { get; set; }
        public string Category { get; set; }
        public int? EssayId { get; set; }   // nullable for those vocab not from essay
        public int UserId { get; set; }
        public DateTime Created { get; set; }
    
    }
}