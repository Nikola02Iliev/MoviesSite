using MoviesSite.Models;
using MoviesSite.Repositories.Interfaces;
using MoviesSite.Services.Interfaces;

namespace MoviesSite.Services.Implementations
{
    public class ReviewsService : IReviewsService
    {
        private readonly IReviewsRepository _reviewsRepository;

        public ReviewsService(IReviewsRepository reviewsRepository)
        {
            _reviewsRepository = reviewsRepository;
        }

        public async Task AddReview(Review review)
        {
           await _reviewsRepository.AddReview(review);
        }

        public async Task DeleteAllReviews(List<Review> reviews)
        {
            await _reviewsRepository.DeleteAllReviews(reviews);
        }

        public async Task DeleteReview(Review review)
        {
            await _reviewsRepository.DeleteReview(review);
        }

        public IQueryable<Review> GetAllReviews()
        {

            return _reviewsRepository.GetAllReviews();
        }

        public Task<Review> GetReviewById(int? id)
        {
            return _reviewsRepository.GetReviewById(id);
        }
    }
}
