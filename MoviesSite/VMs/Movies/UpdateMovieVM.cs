using MoviesSite.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MoviesSite.VMs.Movies
{
    public class UpdateMovieVM
    {
        [Key]
        public int? MovieId { get; set; }

        [Required(ErrorMessage = "Movie name field is required!")]
        public string? MovieName { get; set; }

        [Required(ErrorMessage = "Movie description field is required!")]
        public string? MovieDescription { get; set; }

        public IFormFile? Image { get; set; }
        public string? ImagePath { get; set; }

        [Required(ErrorMessage = "Year released field is required!")]
        public int YearReleased { get; set; }

        

        

        

    }
}
