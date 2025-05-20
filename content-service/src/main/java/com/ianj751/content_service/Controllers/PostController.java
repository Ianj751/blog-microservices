package com.ianj751.content_service.Controllers;
import org.springframework.web.bind.annotation.RestController;

import com.ianj751.content_service.Models.Post;
import com.ianj751.content_service.Repositories.PostRepository;

import lombok.RequiredArgsConstructor;

import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;

import java.util.List;
import java.util.Optional;
import java.util.UUID;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;




@RestController
@RequestMapping("/posts")
@RequiredArgsConstructor
public class PostController {
    private final PostRepository postRepo;

    @GetMapping("")
    public ResponseEntity<Post> getPostById(@RequestParam UUID id) {
        Optional<Post> p = postRepo.findById(id);
        Post post =  p.isPresent() ? p.get() : new Post();
        return ResponseEntity.ok(post);
    }
    
    @GetMapping("/user")
    public ResponseEntity<List<Post>> getPostsbyUserId(@RequestParam UUID userId, @RequestParam(name = "page",defaultValue = "0") Integer page) {
        Integer size = 32;
        Pageable pageable = PageRequest.of(page, size);
        return ResponseEntity.ok(postRepo.findByAuthorId(userId, pageable));
    }
    
    @PostMapping("")
    public ResponseEntity<Post> createPost(@RequestBody Post entity) {
        return ResponseEntity.ok(postRepo.save(entity));
    }
    

}
