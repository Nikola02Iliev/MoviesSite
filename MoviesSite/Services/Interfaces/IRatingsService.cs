using MoviesSite.Models;

namespace MoviesSite.Services.Interfaces
{
    public interface IRatingsService
    {
        Task AddRating(Rating rating);
        Task RecalculateRatings();
        Task RecalculateRating(int? id);
        IQueryable<Rating> GetAllRatings();
        Task DeleteAllRatings(List<Rating> ratings);
        Task<Rating> GetRatingById(int? id);
        Task DeleteRating(Rating rating);
        
    }
}
