using MoviesSite.Models;

namespace MoviesSite.Services.Interfaces
{
    public interface IAdminService
    {
        IQueryable<AppUser> GetAllUsers();
        Task<string> GetUserById(string? id);

        Task<AppUser> GetUser(string? id);
        Task DeleteUser(AppUser user);
        Task<bool> IsUserAdmin(AppUser user);
        
    }
}
