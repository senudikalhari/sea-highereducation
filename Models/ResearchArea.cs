using System.ComponentModel.DataAnnotations;

namespace BlindMatchPAS_Final.Models
{
    public class ResearchArea
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}