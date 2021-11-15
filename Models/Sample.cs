using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lingo.Models
{
    public class Sample
    {
        [Key]
        [Required]
        public int SampleId {get; set;}
        [Required]
        public string Title { get; set; }
        public string Text { get; set; }
        public string Source {get;set;}
        public DateTime Created { get; set; }
        public string Category { get; set; }
        [Required]
        public string Language { get; set; }
        public IList<Essay> Essays { get; set; }
    }
}