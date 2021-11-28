using System;
using System.ComponentModel.DataAnnotations;

namespace Lingo.Models
{
    public class Setting
    {
        [Key]
        [Required]
        public int SettingId { get; set; } 
        public string Name { get; set;}
        public string Value { get; set;}
    }
}