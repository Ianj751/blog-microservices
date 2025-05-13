using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data;

public interface IUserRepo
{
    bool SaveChanges();

    //IEnumerable<ApplicationUser> GetApplicationUsers();
    public Task<ApplicationUser?> GetUserByEmailAsync(string email);
    void CreateUser(ApplicationUser user);
}