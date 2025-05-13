using Microsoft.AspNetCore.Identity;

namespace AuthService.Models;
public class ApplicationUser : IdentityUser<Guid>
{
    public required string ProviderDisplayName { get; set; }
    public required string ProviderUserId { get; set; }
    public required string ProfilePictureUrl { get; set; }
    public DateTime DateJoined { get; set; } = DateTime.Now;


}