package com.ianj751.content_service.services;

import java.util.Base64;
import java.util.Map;

import javax.crypto.spec.SecretKeySpec;


import com.google.gson.Gson;
import com.ianj751.content_service.protos.PublicKeyResponse;

import io.jsonwebtoken.JwtParser;
import io.jsonwebtoken.Jwts;

import lombok.RequiredArgsConstructor;

@RequiredArgsConstructor
public class JWTValidator {

    private final PublicKeyService pKeyService;
   
    public boolean Validate(String token){
        String[] chunks = token.split("\\.");
        Base64.Decoder decoder = Base64.getUrlDecoder();

        String header = new String(decoder.decode(chunks[0]));
        //String payload = new String(decoder.decode(chunks[1]));
        //SignatureAlgorithm sa = Jwts.SIG.RS256;
        

        Gson gson = new Gson();
        Map<String, Object> map = gson.fromJson(header, new com.google.gson.reflect.TypeToken<Map<String, Object>>(){}.getType());
    
        PublicKeyResponse resp =  pKeyService.getPublicKey( (String) map.get("kid"));
        byte[] key = Base64.getUrlDecoder().decode(resp.getKey());

       try {
          
            SecretKeySpec secretKeySpec = new SecretKeySpec(key, "RS256");
            JwtParser jwtParser = Jwts.parser()
                .verifyWith(secretKeySpec)
                .build();
            jwtParser.parse(token);
       } catch (Exception e) {
            System.out.println("Error verifying JWT integrity: "+ e.getMessage());
            return false;
       }
        
        return true;
    }
}
