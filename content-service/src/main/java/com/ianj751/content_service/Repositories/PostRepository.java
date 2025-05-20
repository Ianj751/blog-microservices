package com.ianj751.content_service.Repositories;

import java.util.List;
import java.util.UUID;

import org.springframework.data.domain.Pageable;
//import org.springframework.boot.autoconfigure.data.web.SpringDataWebProperties.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;

import org.springframework.data.jpa.repository.Query;

import com.ianj751.content_service.Models.Post;

public interface PostRepository extends JpaRepository<Post, UUID>{
    
    @Query(value = "SELECT * FROM POSTS WHERE AUTHOR_ID = ?1 ORDER BY LAST_MODIFIED_AT DESC", nativeQuery = true)
    List<Post> findByAuthorId(UUID authorId, Pageable pageable);
}
