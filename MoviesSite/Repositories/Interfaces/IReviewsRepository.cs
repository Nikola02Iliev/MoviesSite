using MoviesSite.Models;

namespace MoviesSite.Repositories.Interfaces
{
    public interface IReviewsRepository
    {
        Task AddReview(Review review);
        IQueryable<Review> GetAllReviews();
        Task DeleteAllReviews(List<Review> reviews);
        Task<Review> GetReviewById(int? id);
        Task DeleteReview(Review review);
    }
}
