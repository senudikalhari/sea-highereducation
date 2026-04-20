using System;
using System.ComponentModel.DataAnnotations;

namespace BlindMatchPAS_Final.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string? UserEmail { get; set; }

        [Required]
        public string Message { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}