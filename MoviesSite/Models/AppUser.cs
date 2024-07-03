using Microsoft.AspNetCore.Identity;

namespace MoviesSite.Models
{
    public class AppUser : IdentityUser
    {
        public List<Review>? Reviews { get; set; }
        public List<Rating>? Ratings { get; set; }
        public List<Movie>? Movies { get; set; }
    }
}
