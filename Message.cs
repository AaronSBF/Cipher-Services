using System;
using System.ComponentModel.DataAnnotations;

namespace CipherServices.Models
{
    public class Message
    {
        public int ID { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z]+$",
            ErrorMessage="The message may contain only letters.")]
        public string Text { get; set; }
    }
}
