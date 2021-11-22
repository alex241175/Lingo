using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lingo.Models
{
    public class Grammar
    {
        [Key]
        [Required]
        public int GrammarId {get; set;}
        [Required]
        public string Title { get; set; }
        public string Text { get; set; }
        public string Source {get;set;}
        public string Filename {get;set;}
        public DateTime Created { get; set; }
        public string Category { get; set; }
        [Required]
        public string Language { get; set; }
    }
}