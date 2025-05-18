using AuthService.Protos;
using Grpc.Core;

namespace AuthService.Services;

public class KeyGrpcService : KeyService.KeyServiceBase
{
    private readonly IKeyStore _keyStore;
    private readonly ILogger<KeyGrpcService> _logger;
    public KeyGrpcService(IKeyStore keyStore, ILogger<KeyGrpcService> logger)
    {
        _keyStore = keyStore;
        _logger = logger;
    }
    public override Task<PublicKeyResponse> GetPublicKey(PublicKeyRequest request, ServerCallContext context)
    {
        return Task.FromResult(new PublicKeyResponse
        {
            Key = _keyStore.GetPublicKey(request.Kid)
        });
    }
}