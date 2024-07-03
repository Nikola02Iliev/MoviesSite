using MoviesSite.Models;

namespace MoviesSite.Services.Interfaces
{
    public interface IMoviesService
    {
        IQueryable<Movie> GetAllMovies();
        Task<Movie> GetMovieById(int? id);
        Task AddMovie(Movie movie);
        Task UpdateMovie(Movie movie);
        Task DeleteMovie(Movie movie);
        Task DeleteAllMovies(List<Movie> movies);

    }
}
