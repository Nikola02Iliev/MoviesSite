using Microsoft.EntityFrameworkCore;
using MoviesSite.Models;
using MoviesSite.Repositories.Interfaces;
using MoviesSite.Services.Interfaces;

namespace MoviesSite.Services.Implementations
{
    public class RatingsService : IRatingsService
    {
        private readonly IRatingsRepository _ratingsRepository;
        private readonly IMoviesRepository _moviesRepository;

        public RatingsService(IRatingsRepository ratingsRepository, IMoviesRepository moviesRepository)
        {
            _ratingsRepository = ratingsRepository;
            _moviesRepository = moviesRepository;
        }

        public async Task AddRating(Rating rating)
        {
            await _ratingsRepository.AddRating(rating);
        }

        public async Task DeleteAllRatings(List<Rating> ratings)
        {
            await _ratingsRepository.DeleteAllRatings(ratings);
        }

        public async Task DeleteRating(Rating rating)
        {
            await _ratingsRepository.DeleteRating(rating);
        }

        public IQueryable<Rating> GetAllRatings()
        {
            return _ratingsRepository.GetAllRatings();
        }

        public Task<Rating> GetRatingById(int? id)
        {
            return _ratingsRepository.GetRatingById(id);
        }

        public async Task RecalculateRating(int? id)
        {
            var movie = await _moviesRepository.GetMovieById(id);

            movie.AverageRating = movie.Ratings.Any() ? movie.Ratings.Average(r => r.RatingLevel) : 0;

            await _ratingsRepository.Save();
        }

        public async Task RecalculateRatings()
        {
            var movies = _moviesRepository.GetAllMovies()
                .Include(m=>m.Ratings);

            foreach (var movie in movies)
            {
                movie.AverageRating = movie.Ratings.Any() ? movie.Ratings.Average(r => r.RatingLevel) : 0;
            }

            await _ratingsRepository.Save();
        }
    }
}
