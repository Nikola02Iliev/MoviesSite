using MoviesSite.Models;

namespace MoviesSite.Repositories.Interfaces
{
    public interface IRatingsRepository
    {
        Task AddRating(Rating rating);
        IQueryable<Rating> GetAllRatings();
        Task DeleteAllRatings(List<Rating> ratings);
        Task<Rating> GetRatingById(int? id);
        Task DeleteRating(Rating rating);
        Task Save();
    }
}
