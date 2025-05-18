
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthService.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Services;

public class TokenProvider
{

    private readonly IConfiguration _configuration;
    private readonly IKeyStore _keyStore;

    public TokenProvider(IConfiguration configuration, IKeyStore keyStore)
    {
        this._configuration = configuration;
        this._keyStore = keyStore;
    }

    public string Create(ApplicationUser user)
    {

        var rsaProvider = new RSACryptoServiceProvider(512);
        var securityKey = new RsaSecurityKey(rsaProvider);


        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256Signature);
        var guid = new Guid().ToString();
        _keyStore.AddKey(guid, Encoding.UTF8.GetString(securityKey.Rsa.ExportRSAPublicKey()));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                    //Gotta add some more claims here
                ]
            ),
            Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
            SigningCredentials = creds,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            AdditionalHeaderClaims = new Dictionary<string, object>{
                { "kid", guid}

            }

        };

        var handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }

}