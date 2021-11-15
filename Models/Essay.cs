using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lingo.Models
{
    public class Essay
    {
        [Key]
        [Required]
        public int EssayId {get; set;}
        [Required]
        public string Title { get; set; }
        public string Text { get; set; }
        public string Source {get;set;}
        public DateTime Created { get; set; }
        public string Category { get; set; }
        [Required]
        public string Language { get; set; }
        public int? SampleId { get; set; }   // nullable for those essay not from sample
        public int UserId { get; set; } 
        public IList<Vocab> Vocabs { get; set; }
    }
}