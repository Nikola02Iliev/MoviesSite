using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesSite.Models;
using MoviesSite.Paginator;
using MoviesSite.Services.Interfaces;
using MoviesSite.VMs;
using MoviesSite.VMs.Movies;
using System.Security.Claims;

namespace MoviesSite.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviesService _moviesService;
        private readonly IRatingsService _ratingsService;
        private readonly IReviewsService _reviewsService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MoviesController(IMoviesService moviesService, IWebHostEnvironment webHostEnvironment, IRatingsService ratingsService, IReviewsService reviewsService)
        {
            _moviesService = moviesService;
            _webHostEnvironment = webHostEnvironment;
            _ratingsService = ratingsService;
            _reviewsService = reviewsService;
        }



        [HttpGet]
        public async Task<IActionResult> Index(int? pageNumber, string searchValue, string sortValue)
        {
            await _ratingsService.RecalculateRatings();
            int pageSize = 6;

            var movies = _moviesService.GetAllMovies().AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                movies = movies.Where(m => m.MovieName.Contains(searchValue));
            }

            movies = sortValue switch
            {
                "descendingByDatePublished" => movies.OrderByDescending(m => m.DatePublished),
                "ascendingByDatePublished" => movies.OrderBy(m => m.DatePublished),
                "descendingByYearReleased" => movies.OrderByDescending(m => m.YearReleased),
                "ascendingByYearReleased" => movies.OrderBy(m => m.YearReleased),
                "descendingByAverageRating" => movies.OrderByDescending(m => m.AverageRating),
                "ascendingByAverageRating" => movies.OrderBy(m => m.AverageRating),
                _ => movies.OrderByDescending(m => m.DatePublished),
            };

            var listMovieVMs = movies.Select(m => new ListMovieVM
            {
                MovieId = m.MovieId,
                MovieName = m.MovieName,
                MovieDescription = m.MovieDescription,
                ImagePath = m.ImagePath,
                AverageRating = m.AverageRating,
                DatePublished = m.DatePublished,
                YearReleased = m.YearReleased,
                User = m.User,
                UserId = m.UserId
            });

            var paginatedMovies = await PaginatedList<ListMovieVM>.CreateAsync(listMovieVMs.AsNoTracking(), pageNumber ?? 1, pageSize);
            ViewBag.CurrentSort = sortValue;
            ViewBag.CurrentSearch = searchValue;

            return View(paginatedMovies);
        }



        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            var createMovieVM = new CreateMovieVM();
            return View(createMovieVM);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMovieVM createMovieVM)
        {
            if (createMovieVM.Image != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(createMovieVM.Image.FileName);
                string filePath = Path.Combine(uploadDir, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await createMovieVM.Image.CopyToAsync(fileStream);
                }

                string relativePath = Path.Combine("Images", fileName).Replace("\\", "/");

                var movie = new Movie
                {
                    MovieName = createMovieVM.MovieName,
                    MovieDescription = createMovieVM.MovieDescription,
                    ImagePath = relativePath,
                    YearReleased = createMovieVM.YearReleased,
                    DatePublished = createMovieVM.DatePublished,
                    AverageRating = createMovieVM.AverageRating,
                    UserId = createMovieVM.UserId
                };

                await _moviesService.AddMovie(movie);

                return RedirectToAction("Index");

            }

            return View(createMovieVM);


        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            await _ratingsService.RecalculateRating(id);

            var movie = await _moviesService.GetMovieById(id);

            if (movie == null)
            {
                return NotFound();
            }


            var detailsMovie = new DetailsMovieVM
            {
                MovieId = movie.MovieId,
                MovieName = movie.MovieName,
                MovieDescription = movie.MovieDescription,
                AverageRating = movie.AverageRating,
                ImagePath = movie.ImagePath,
                YearReleased = movie.YearReleased,
                DatePublished = movie.DatePublished,
                UserId = movie.UserId,
                User = movie.User,
                Reviews = movie.Reviews,
                Ratings = movie.Ratings
            };


            return View(detailsMovie);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UserMovies(int? pageNumber, string sortValue)
        {

            await _ratingsService.RecalculateRatings();

            int pageSize = 6;


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var movies = _moviesService.GetAllMovies().AsQueryable();

            movies = sortValue switch
            {
                "descendingByDatePublished" => movies.OrderByDescending(m => m.DatePublished),
                "ascendingByDatePublished" => movies.OrderBy(m => m.DatePublished),
                "descendingByYearReleased" => movies.OrderByDescending(m => m.YearReleased),
                "ascendingByYearReleased" => movies.OrderBy(m => m.YearReleased),
                "descendingByAverageRating" => movies.OrderByDescending(m => m.AverageRating),
                "ascendingByAverageRating" => movies.OrderBy(m => m.AverageRating),
                _ => movies.OrderByDescending(m => m.DatePublished),
            };

            var listMovieVMs = movies.Where(m => m.UserId == userId).Select(m => new ListMovieVM
            {
                MovieId = m.MovieId,
                MovieName = m.MovieName,
                MovieDescription = m.MovieDescription,
                ImagePath = m.ImagePath,
                AverageRating = m.AverageRating,
                DatePublished = m.DatePublished,
                YearReleased = m.YearReleased,
                User = m.User,
                UserId = m.UserId
            });

            var paginatedMovies = await PaginatedList<ListMovieVM>.CreateAsync(listMovieVMs.AsNoTracking(), pageNumber ?? 1, pageSize);
            ViewBag.CurrentSort = sortValue;

            return View(paginatedMovies);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _moviesService.GetMovieById(id);
            var movieId = movie.MovieId;

            var movieReviews = _reviewsService.GetAllReviews().Where(r => r.MovieId == movieId).ToList();
            var movieRatings = _ratingsService.GetAllRatings().Where(r => r.MovieId == movieId).ToList();

            await _reviewsService.DeleteAllReviews(movieReviews);
            await _ratingsService.DeleteAllRatings(movieRatings);

            await _moviesService.DeleteMovie(movie);

            return RedirectToAction("Index");

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateMovie(int id)
        {
            var movie = await _moviesService.GetMovieById(id);

            var updateMovieVM = new UpdateMovieVM
            {
                MovieId = movie.MovieId,
                MovieName = movie.MovieName,
                MovieDescription = movie.MovieDescription,
                ImagePath = movie.ImagePath,
                YearReleased = movie.YearReleased,
                

            };


            return View(updateMovieVM);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateMovie(UpdateMovieVM updateMovieVM)
        {

            if (ModelState.IsValid)
            {
                var movie = await _moviesService.GetMovieById(updateMovieVM.MovieId);
                if (movie == null) 
                {
                    return NotFound();
                }

                if (updateMovieVM.Image != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(updateMovieVM.Image.FileName);
                    string filePath = Path.Combine(uploadDir, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await updateMovieVM.Image.CopyToAsync(fileStream);
                    }

                    string relativePath = Path.Combine("Images", fileName).Replace("\\", "/");
                    movie.ImagePath = relativePath;
                }

                

                movie.MovieName= updateMovieVM.MovieName;
                movie.MovieDescription= updateMovieVM.MovieDescription;
                movie.YearReleased = updateMovieVM.YearReleased;

                var movieId = movie.MovieId;

                var movieRatings = _ratingsService.GetAllRatings().Where(r => r.MovieId == movieId).ToList();
                var movieReviews = _reviewsService.GetAllReviews().Where(r => r.MovieId == movieId).ToList();

                await _ratingsService.DeleteAllRatings(movieRatings);
                await _reviewsService.DeleteAllReviews(movieReviews);

                await _moviesService.UpdateMovie(movie);

                return RedirectToAction("UserMovies");

            }
            
       
            return View(updateMovieVM);

        }
    }
}