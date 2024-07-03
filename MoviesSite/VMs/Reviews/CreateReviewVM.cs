using MoviesSite.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesSite.VMs.Reviews
{
    public class CreateReviewVM
    {

        [Required(ErrorMessage = "Review title is required")]
        public string ReviewTitle { get; set; }

        [Required(ErrorMessage = "Review content is required")]
        public string ReviewContent { get; set; }

        public DateTime ReviewedDateTime { get; set; }

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
