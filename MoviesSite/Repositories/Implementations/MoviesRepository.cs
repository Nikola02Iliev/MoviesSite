using Microsoft.EntityFrameworkCore;
using MoviesSite.Context;
using MoviesSite.Models;
using MoviesSite.Repositories.Interfaces;

namespace MoviesSite.Repositories.Implementations
{
    public class MoviesRepository : IMoviesRepository
    {
        private readonly AppDBContext _context;
        private readonly DbSet<Movie> _dbSet;

        public MoviesRepository(AppDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<Movie>();
        }

        public async Task AddMovie(Movie movie)
        {
            _dbSet.Add(movie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllMovies(List<Movie> movies)
        {
            _dbSet.RemoveRange(movies);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMovie(Movie movie)
        {
            _dbSet.Remove(movie);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Movie> GetAllMovies()
        {
            return _dbSet
                .Include(m => m.User)
                .AsQueryable();
        }

        public async Task<Movie> GetMovieById(int? id)
        {
            return await _dbSet
                .Include(m => m.User)
                .Include(m => m.Reviews)
                    .ThenInclude(r => r.User)
                .Include(m => m.Ratings)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.MovieId == id);
        }

        public async Task UpdateMovie(Movie movie)
        {
            _dbSet.Update(movie);
            await _context.SaveChangesAsync();
        }
    }
}
