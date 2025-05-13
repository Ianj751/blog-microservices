using System.Text.Json;
using System.Text.Json.Serialization;
using AuthService.Data;
using AuthService.Models;
using AuthService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controller;
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepo userRepo;
    private readonly TokenProvider tokenProvider;

    public AuthController(IUserRepo userRepo, TokenProvider tokenProvider)
    {
        this.userRepo = userRepo;
        this.tokenProvider = tokenProvider;
    }

    [HttpPost("signin-google")]
    public async Task<IActionResult> SignInWithGoogle(GoogleSignInBody req)
    {


        try
        {

            GoogleResponse resp;
            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri("https://www.googleapis.com/oauth2/v3/userinfo");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {req.access_token}");


                using HttpResponseMessage response = await client.GetAsync("");
                response.EnsureSuccessStatusCode();

                string body = await response.Content.ReadAsStringAsync();
                resp = JsonSerializer.Deserialize<GoogleResponse>(body)!;
            }
            var user = new ApplicationUser()
            {
                ProviderDisplayName = resp.name,
                ProfilePictureUrl = resp.picture,
                ProviderUserId = resp.sub,
                Email = resp.email

            };
            if (await userRepo.GetUserByEmailAsync(user.Email) == null)
            {
                userRepo.CreateUser(user);
            }

            var token = tokenProvider.Create(user);
            return Ok(new { jwt = token });

        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
            return BadRequest(new { error = "Failed to authenticate with Google" });
        }
    }

    public record GoogleSignInBody
    {
        // [JsonPropertyName("access_token")]
        public required string access_token { get; init; }
        /* 
        Dont think i need the rest of them, will change if wrong
                [JsonPropertyName("refresh_token")]
                public string refreshToken { get; init; }
                public string TTL { get; init; } */
    }


    /* curl -H "Authorization: Bearer <access_code>" https://www.googleapis.com/oauth2/v3/userinfo
    {
      "sub": "10668...",
      "name": "ian j",
      "given_name": "ian",
      "family_name": "j",
      "picture": "https://lh3.googleusercontent.com/a/ACg8ocJiByLp3fw96lfsrtvzwQQZcqKeLlFvIDMvOSG20QyTfFraUWNH\u003ds96-c",
      "email": "ij961267@gmail.com",
      "email_verified": true
    } */
    public record GoogleResponse
    {
        //[JsonPropertyName("sub")]
        public required string sub { get; init; }
        // [JsonPropertyName("name")]
        public required string name { get; init; }
        //[JsonPropertyName("given_name")]
        public required string given_name { get; init; }
        // [JsonPropertyName("family_name")]
        public required string family_name { get; init; }
        // [JsonPropertyName("picture")]
        public required string picture { get; init; }
        //[JsonPropertyName("email")]
        public required string email { get; init; }
        //[JsonPropertyName("email_verified")]
        public required bool email_verified { get; init; }
    }
}