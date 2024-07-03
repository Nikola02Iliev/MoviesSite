using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesSite.Models;
using MoviesSite.Paginator;
using MoviesSite.Services.Interfaces;
using MoviesSite.VMs.Movies;
using MoviesSite.VMs.Ratings;
using System.Security.Claims;

namespace MoviesSite.Controllers
{
    [Authorize]
    public class RatingsController : Controller
    {
        private readonly IRatingsService _ratingsService;
        private readonly IMoviesService _moviesService;


        public RatingsController(IRatingsService ratingsService, IMoviesService moviesService)
        {
            _ratingsService = ratingsService;
            _moviesService = moviesService;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRating(CreateRatingVM createRatingVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var rating = new Rating
                    {
                        RatingLevel = createRatingVM.RatingLevel,
                        RatedDateTime = createRatingVM.RatedDateTime,
                        MovieId = createRatingVM.MovieId,
                        UserId = createRatingVM.UserId
                    };

                    await _ratingsService.AddRating(rating);

                    return RedirectToAction("Details", "Movies", new { id = createRatingVM.MovieId });

                }

                var movie = await _moviesService.GetMovieById(createRatingVM.MovieId);
                if (movie == null)
                {
                    return NotFound();
                }

                var detailsMovie = new DetailsMovieVM
                {
                    MovieId = movie.MovieId,
                    MovieName = movie.MovieName,
                    MovieDescription = movie.MovieDescription,
                    ImagePath = movie.ImagePath,
                    AverageRating = movie.AverageRating,
                    YearReleased = movie.YearReleased,
                    DatePublished = movie.DatePublished,
                    User = movie.User,
                    UserId = movie.UserId,
                    Reviews = movie.Reviews,
                    Ratings = movie.Ratings,
                    CreateRatingVM = createRatingVM
                };


                return View("~/Views/Movies/Details.cshtml", detailsMovie);

            }
            catch (Exception ex) 
            {
                TempData["ErrorMessage"] = @"
            <div class='container mt-5'>
                <div class='text-center'>
                    <h1 class='display-4'>Oh no!</h1>
                    <p class='lead'>You have already submitted a rating. You cannot submit a rating twice.</p>
                    <hr class='my-4'>
                    <p>Please contact support if you think this is a mistake.</p>
                    <a class='btn btn-primary btn-lg' href='/' role='button'>Return to Home</a>
                </div>
            </div>";

                return RedirectToAction("Details", "Movies", new { id = createRatingVM.MovieId });
            }

        }

        [HttpGet]
        public async Task<IActionResult> UserRatings(int? pageNumber, string sortValue)
        {
            int pageSize = 8;


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var ratings = _ratingsService.GetAllRatings().Where(r => r.UserId == userId).AsQueryable();

            ratings = sortValue switch
            {
                "descendingByRatedDateTime" => ratings.OrderByDescending(r => r.RatedDateTime),
                "ascendingByRatedDateTime" => ratings.OrderBy(r => r.RatedDateTime),
                "descendingByRatingLevel" => ratings.OrderByDescending(r => r.RatingLevel),
                "ascendingByRatingLevel" => ratings.OrderBy(r => r.RatingLevel),
                _ => ratings.OrderByDescending(r => r.RatedDateTime)

            };

            ViewBag.CurrentSort = sortValue;
            var paginatedRatings = await PaginatedList<Rating>.CreateAsync(ratings.AsNoTracking(), pageNumber ?? 1, pageSize);
            return View(paginatedRatings);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteRating(int id)
        {
            var rating = await _ratingsService.GetRatingById(id);
            if(rating == null)
            {
                return NotFound();
            }

            await _ratingsService.DeleteRating(rating);

            return RedirectToAction("UserRatings");
        }

        
    }
}
