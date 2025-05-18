namespace AuthService.Services;

public class KeyStore : IKeyStore
{
    private readonly Dictionary<string, string> _keys = new();

    public void AddKey(string kid, string publicKey)
    {
        _keys[kid] = publicKey;
    }

    public string GetPublicKey(string kid)
    {
        return _keys.TryGetValue(kid, out var key) ? key : string.Empty;
    }

    public Dictionary<string, string> GetAllKeys()
    {
        return new Dictionary<string, string>(_keys);
    }
}