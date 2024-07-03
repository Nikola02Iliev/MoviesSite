using Microsoft.EntityFrameworkCore;
using MoviesSite.Context;
using MoviesSite.Models;
using MoviesSite.Repositories.Interfaces;

namespace MoviesSite.Repositories.Implementations
{
    public class RatingsRepository : IRatingsRepository
    {
        private readonly AppDBContext _context;
        private readonly DbSet<Rating> _dbSet;

        public RatingsRepository(AppDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<Rating>();
        }

        public async Task AddRating(Rating rating)
        {
            _dbSet.Add(rating);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllRatings(List<Rating> ratings)
        {
            _dbSet.RemoveRange(ratings);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRating(Rating rating)
        {
            _dbSet.Remove(rating);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Rating> GetAllRatings()
        {
            return _dbSet
                .Include(r => r.Movie);
        }

        public async Task<Rating> GetRatingById(int? id)
        {
            return await _dbSet
                .Include(r=>r.User)
                .FirstOrDefaultAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
