using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesSite.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        public string? ReviewTitle { get; set; }

        public string? ReviewContent { get; set; }

        public DateTime ReviewedDateTime { get; set; }
       
        public int? MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movie? Movie { get; set; }

        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser? User { get; set; }

        public string GetFormattedReviewedDateTime()
        {
            return ReviewedDateTime.ToString();
        }
    }
}
