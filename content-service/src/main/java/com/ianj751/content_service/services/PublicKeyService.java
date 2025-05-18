package com.ianj751.content_service.services;

import org.springframework.stereotype.Service;
import net.devh.boot.grpc.client.inject.GrpcClient;
import com.ianj751.content_service.protos.KeyServiceGrpc;
import com.ianj751.content_service.protos.PublicKeyRequest;

import com.ianj751.content_service.protos.PublicKeyResponse;

@Service
public class PublicKeyService {

    @GrpcClient("keyService")
    private KeyServiceGrpc.KeyServiceBlockingStub serviceBlockingStub;

    public PublicKeyResponse getPublicKey(String keyId){
        PublicKeyRequest req = PublicKeyRequest.newBuilder().setKid(keyId).build();
        return serviceBlockingStub.getPublicKey(req);
    }


}
