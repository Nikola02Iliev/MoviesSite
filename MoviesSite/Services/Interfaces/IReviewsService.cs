using MoviesSite.Models;

namespace MoviesSite.Services.Interfaces
{
    public interface IReviewsService
    {
        Task AddReview(Review review);
        IQueryable<Review> GetAllReviews();
        Task DeleteAllReviews(List<Review> reviews);
        Task<Review> GetReviewById(int? id);
        Task DeleteReview(Review review);
    }
}
