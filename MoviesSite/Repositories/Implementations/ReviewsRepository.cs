using Microsoft.EntityFrameworkCore;
using MoviesSite.Context;
using MoviesSite.Models;
using MoviesSite.Repositories.Interfaces;

namespace MoviesSite.Repositories.Implementations
{
    public class ReviewsRepository : IReviewsRepository
    {
        private readonly AppDBContext _context;
        private readonly DbSet<Review> _dbSet;

        public ReviewsRepository(AppDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<Review>();
        }

        public async Task AddReview(Review review)
        {
            _dbSet.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllReviews(List<Review> reviews)
        {
            _dbSet.RemoveRange(reviews);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReview(Review review)
        {
             _dbSet.Remove(review);
             await _context.SaveChangesAsync();
        }

        public IQueryable<Review> GetAllReviews()
        {
            return _dbSet
                .Include(r => r.Movie);
        }

        public async Task<Review> GetReviewById(int? id)
        {
            return await _dbSet
                .Include(r => r.Movie)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.ReviewId == id);
        }
    }
}
