using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesSite.Models;
using MoviesSite.Paginator;
using MoviesSite.Services.Interfaces;
using MoviesSite.VMs.Movies;
using MoviesSite.VMs.Ratings;
using MoviesSite.VMs.Reviews;
using System.Linq;
using System.Security.Claims;

namespace MoviesSite.Controllers
{

    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly IReviewsService _reviewsService;
        private readonly IMoviesService _moviesService;
        public ReviewsController(IReviewsService reviewsService, IMoviesService moviesService)
        {
            _reviewsService = reviewsService;
            _moviesService = moviesService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(CreateReviewVM createReviewVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var review = new Review
                    {
                        ReviewTitle = createReviewVM.ReviewTitle,
                        ReviewContent = createReviewVM.ReviewContent,
                        ReviewedDateTime = createReviewVM.ReviewedDateTime,
                        MovieId = createReviewVM.MovieId,
                        UserId = createReviewVM.UserId
                    };

                    await _reviewsService.AddReview(review);
                    return RedirectToAction("Details", "Movies", new { id = createReviewVM.MovieId });
                }


                var movie = await _moviesService.GetMovieById(createReviewVM.MovieId);
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
                    YearReleased = movie.YearReleased,
                    DatePublished = movie.DatePublished,
                    AverageRating = movie.AverageRating,
                    UserId = movie.UserId,
                    User = movie.User,
                    Ratings = movie.Ratings,
                    Reviews = movie.Reviews,
                    CreateReviewVM = createReviewVM
                };

                return View("~/Views/Movies/Details.cshtml", detailsMovie);

            }
            catch (Exception ex) 
            {
                TempData["ErrorMessage"] = @"
            <div class='container mt-5'>
                <div class='text-center'>
                    <h1 class='display-4'>Oh no!</h1>
                    <p class='lead'>You have already submitted a review. You cannot submit a review twice.</p>
                    <hr class='my-4'>
                    <p>Please contact support if you think this is a mistake.</p>
                    <a class='btn btn-primary btn-lg' href='/' role='button'>Return to Home</a>
                </div>
            </div>"
                ;

                return RedirectToAction("Details", "Movies", new { id = createReviewVM.MovieId });

            }

                            
            
        }

        public async Task<IActionResult> UserReviews(int? pageNumber, string sortValue)
        {
            int pageSize = 6;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reviews = _reviewsService.GetAllReviews().Where(r => r.UserId == userId).AsQueryable();

            reviews = sortValue switch
            {
                "descendingByReviewedDateTime" => reviews.OrderByDescending(r => r.ReviewedDateTime),
                "ascendingByReviewedDateTime" => reviews.OrderBy(r => r.ReviewedDateTime),
                "descendingByReviewTitle" => reviews.OrderByDescending(r => r.ReviewTitle),
                "ascendingByReviewTitle" => reviews.OrderBy(r => r.ReviewTitle),
                _ => reviews.OrderByDescending(r => r.ReviewedDateTime)
            };

            ViewBag.CurrentSort = sortValue;
            var paginatedReviews = await PaginatedList<Review>.CreateAsync(reviews.AsNoTracking(), pageNumber ?? 1, pageSize);

            return View(paginatedReviews);
            
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _reviewsService.GetReviewById(id);
            if (review == null)
            {
                return NotFound();
            }

            await _reviewsService.DeleteReview(review);

            return RedirectToAction("UserReviews");
        }
    }
}
