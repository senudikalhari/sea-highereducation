using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace BlindMatchPAS_Final.Models
{
    public class Project
    {
        public int Id { get; set; }

        
        //BASIC DETAILS
        
        [Required]
        public string Title { get; set; }

        [Required]
        public string Abstract { get; set; }

        public string? TechStack { get; set; }
        public string? ResearchArea { get; set; }

        
        public string Status { get; set; } = "Pending";

        
        public string? StudentId { get; set; }
        public string? StudentEmail { get; set; }

        
        public string? SupervisorId { get; set; }
        public string? SupervisorEmail { get; set; }

       
        public bool IsMatched { get; set; } = false;

        
        public string? FilePath { get; set; }

        //FILE UPLOAD
        [NotMapped]
        public IFormFile? File { get; set; }

        
        //FEEDBACK
        
        public string? Feedback { get; set; }

        
        //AI SCORE
        
        [Range(0, 100)]
        public int Score { get; set; } = 0;

        
        //OPTIONAL (GOOD FOR REPORT)
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}