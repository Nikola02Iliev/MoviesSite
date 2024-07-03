using Microsoft.AspNetCore.Identity;
using MoviesSite.Models;
using MoviesSite.Services.Interfaces;

namespace MoviesSite.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task DeleteUser(AppUser user)
        {

          await _userManager.DeleteAsync(user);
        }

        public IQueryable<AppUser> GetAllUsers()
        {

            return _userManager.Users.AsQueryable();
        }

        public async Task<AppUser> GetUser(string? id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<string> GetUserById(string? id)
        {
            var user = await  _userManager.FindByIdAsync(id);
            var userId = await _userManager.GetUserIdAsync(user);

            return userId;
        }

        public async Task<bool> IsUserAdmin(AppUser user)
        {
            return await _userManager.IsInRoleAsync(user, "Admin");
        }
    }
}
