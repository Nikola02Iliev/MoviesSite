using MoviesSite.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesSite.VMs.Movies
{
    public class ListMovieVM
    {
        [Key]
        public int MovieId { get; set; }

        public string? MovieName { get; set; }

        public string? MovieDescription { get; set; }

        public string? ImagePath { get; set; }

        public int YearReleased { get; set; }

        public DateTime DatePublished { get; set; }

        public double AverageRating { get; set; } = 0.0;
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser? User { get; set; }

        public string GetFormattedDatePublished()
        {
            return DatePublished.ToString("dd-MM-yyyy HH:mm");
        }
    }
}
