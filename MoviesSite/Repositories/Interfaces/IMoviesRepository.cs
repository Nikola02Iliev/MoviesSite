using MoviesSite.Models;

namespace MoviesSite.Repositories.Interfaces
{
    public interface IMoviesRepository
    {
        IQueryable<Movie> GetAllMovies();
        Task<Movie> GetMovieById(int? id);
        Task AddMovie(Movie movie);
        Task DeleteMovie(Movie movie);
        Task DeleteAllMovies(List<Movie> movies);
        Task UpdateMovie(Movie movie);
        


    }
}
