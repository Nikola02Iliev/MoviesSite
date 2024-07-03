using MoviesSite.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MoviesSite.VMs.Ratings
{
    public class CreateRatingVM
    {
        [Required(ErrorMessage = "Rating level is required")]
        [Range(1, 5, ErrorMessage = "Rating level must be from 1 to 5")]
        public int RatingLevel { get; set; }

        public DateTime RatedDateTime { get; set; }

        [Required]
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movie? Movie { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser? User { get; set; }
    }
}
