using MoviesSite.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesSite.VMs.Movies
{
    public class CreateMovieVM
    {
        
        [Required (ErrorMessage = "Movie name field is required!")]
        public string? MovieName { get; set; }

        [Required(ErrorMessage = "Movie description field is required!")]
        public string? MovieDescription { get; set; }

        [Required(ErrorMessage = "Image is required!")]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Year released field is required!")]
        public int YearReleased { get; set; }

        public DateTime DatePublished { get; set; }

        public double AverageRating { get; set; } = 0.0;
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser? User { get; set; }
    }
}
