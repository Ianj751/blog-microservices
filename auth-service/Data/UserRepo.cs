using System.Threading.Tasks;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data;

public class UserRepo : IUserRepo
{
    private readonly ApplicationDbContext _context;
    public UserRepo(ApplicationDbContext ctx)
    {
        _context = ctx;
    }
    public void CreateUser(ApplicationUser user)
    {
        _context.Users.Add(user);
        _context.SaveChangesAsync();
    }

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
    {
        var user = await _context.ApplicationUsers
            .FirstOrDefaultAsync(u => u.Email == email);

        return user;
    }


    public bool SaveChanges()
    {
        return _context.SaveChanges() >= 0;
    }
}