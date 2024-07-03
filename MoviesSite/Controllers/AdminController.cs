using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesSite.Services.Implementations;
using MoviesSite.Services.Interfaces;

namespace MoviesSite.Controllers
{
    [Authorize("Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IMoviesService _moviesService;
        private readonly IReviewsService _reviewsService;
        private readonly IRatingsService _ratingsService;

        public AdminController(IAdminService adminService, IMoviesService moviesService, IReviewsService reviewsService, IRatingsService ratingsService)
        {
            _adminService = adminService;
            _moviesService = moviesService;
            _reviewsService = reviewsService;
            _ratingsService = ratingsService;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _adminService.GetAllUsers();

            return View(users);
            
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _adminService.GetUser(id);
            var userIsAdmin = await _adminService.IsUserAdmin(user);

            if (userIsAdmin)
            {
                return RedirectToAction("GetUsers");
            }

            var userId = await _adminService.GetUserById(id);
            if (userId == null)
            {
                return NotFound();
            }

            
            var userRatings = _ratingsService.GetAllRatings().Where(r => r.UserId == userId).ToList();
            await _ratingsService.DeleteAllRatings(userRatings);

            var userReviews = _reviewsService.GetAllReviews().Where(r => r.UserId == userId).ToList();
            await _reviewsService.DeleteAllReviews(userReviews);

            
            var userMovies = _moviesService.GetAllMovies().Where(m => m.UserId == userId).ToList();

            foreach (var movie in userMovies)
            {
                
                var ratingsToDelete = _ratingsService.GetAllRatings().Where(r => r.MovieId == movie.MovieId).ToList();
                await _ratingsService.DeleteAllRatings(ratingsToDelete);

                
                var reviewsToDelete = _reviewsService.GetAllReviews().Where(r => r.MovieId == movie.MovieId).ToList();
                await _reviewsService.DeleteAllReviews(reviewsToDelete);
            }

            
            await _moviesService.DeleteAllMovies(userMovies);

            
            await _adminService.DeleteUser(user);

            return RedirectToAction("GetUsers");
        }


    }
}
