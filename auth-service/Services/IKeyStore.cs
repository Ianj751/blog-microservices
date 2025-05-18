namespace AuthService.Services;

public interface IKeyStore
{
    void AddKey(string kid, string publicKey);
    string? GetPublicKey(string kid);
    Dictionary<string, string> GetAllKeys();
}