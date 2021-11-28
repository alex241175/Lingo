using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string Category { get; set; }  // User, 1000, 3000, 6000, Phrasal Verb
        public int? UserId { get; set; }    // nullable for standard category 
        public string EssayIds {get; set;}   
        public DateTime Created { get; set; }
    
    }
}