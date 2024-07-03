using MoviesSite.Models;
using MoviesSite.Repositories.Interfaces;
using MoviesSite.Services.Interfaces;
using System.Drawing.Drawing2D;

namespace MoviesSite.Services.Implementations
{
    public class MoviesService : IMoviesService
    {
        private readonly IMoviesRepository _moviesRepository;

        public MoviesService(IMoviesRepository moviesRepository)
        {
            _moviesRepository = moviesRepository;
        }

        public async Task AddMovie(Movie movie)
        {
            await _moviesRepository.AddMovie(movie);
        }

        public async Task DeleteAllMovies(List<Movie> movies)
        {
             await _moviesRepository.DeleteAllMovies(movies);
        }

        public async Task DeleteMovie(Movie movie)
        {
            await _moviesRepository.DeleteMovie(movie);
        }

        public IQueryable<Movie> GetAllMovies()
        {
            return _moviesRepository.GetAllMovies();
        }

        public Task<Movie> GetMovieById(int? id)
        {
            return _moviesRepository.GetMovieById(id);
        }

        public async Task UpdateMovie(Movie movie)
        {
            await _moviesRepository.UpdateMovie(movie);
        }
    }
}
