using System;
using System.ComponentModel.DataAnnotations;

namespace Lingo.Models
{
    public class ReviewSetting
    {
        [Key]
        [Required]
        public int ReviewSettingId { get; set; } 
        public string Language { get; set;}
        public int PassedThreshold { get; set;}
        public int VocabsPerDay { get; set;}
        public int LastVocabId {get; set;}
        public string VocabsCategory { get; set;}
    }
}