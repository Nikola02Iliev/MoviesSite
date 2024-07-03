using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesSite.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }

        public string? MovieName { get; set; }

        public string? MovieDescription { get; set; }

        public string? ImagePath { get; set; }

        public int YearReleased { get; set; }

        public DateTime DatePublished { get; set; }

        public double AverageRating { get; set; }

        public List<Review>? Reviews { get; set; }
        public List<Rating>? Ratings { get; set; }

        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser? User { get; set; }
    }
}
