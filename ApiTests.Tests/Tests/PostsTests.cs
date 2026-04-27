using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using ApiTests.Models;
using ApiTests.Utils;
using NUnit.Framework;

namespace ApiTests.Tests;

[TestFixture] 
public class PostsTests : BaseTest
{

//GET /posts
[Test]
public async Task GetPosts_ReturnsAllPosts()
{
    // ACT get to /posts
    var posts = await client.GetAsync<List<Post>>("/posts");

    // ASSERTS for response
    Assert.That(posts, Is.Not.Null, "Posts response should not be null");
    Assert.That(posts.Count, Is.EqualTo(100),"API should return exactly 100 posts");

    var firstPost = posts[0];
    
    // ASSERTS for data
    Assert.That(firstPost.Id, Is.GreaterThan(0), "First post ID should be a valid positive number");
    Assert.That(firstPost.UserId, Is.GreaterThan(0), "UserId should be a valid positive number");
    Assert.That(firstPost.Title, Is.Not.Null.And.Not.Empty, "Post title should not be null or empty");
    Assert.That(firstPost.Body, Is.Not.Null.And.Not.Empty, "Post body should not be null or empty");
}


// GET /posts/{id} positive+negatuve tests
[Test]
public async Task GetSinglePost_ReturnsPost()
{
    // ACT get to /posts/1 (positive)
    var post = await client.GetAsync<Post>("/posts/1");

    // ASSERTS for response
    Assert.That(post, Is.Not.Null, "Posts response should not be null");
    Assert.That(post.Id, Is.EqualTo(1), "Post ID should be 1 for endpoint /posts/1");

    // ACT get to /posts/999999999 (negative (JSONPlaceholder limitation))
    var response = await client.GetRawAsync("/posts/999999999");

    var json = await response.Content.ReadAsStringAsync();

    // ASSERT for handling API limitation for missing resources
    Assert.That(
        response.StatusCode == HttpStatusCode.NotFound ||
        (response.StatusCode == HttpStatusCode.OK && json == "{}"),
        Is.True, 
        "API should return 404 or empty object {} for non-existing post"
    );
}
  

//POST /posts
[Test]
public async Task CreatePost_ReturnsCreatedPost1()
{
    // ARRANGE
    var newPost = new CreatePostRequest
    {
        UserId = 1,
        Title = "Test title",
        Body = "Test body"
    };

    // ACT post to /posts/
    var response = await client.PostAsync<Post>("/posts", newPost);

    // ASSERTS for response
    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    Assert.That(response.Data, Is.Not.Null, "Posts response should not be null");

    // ASSERTS for data
    Assert.That(response.Data.Title, Is.EqualTo(newPost.Title), "Returned title should match request payload");
    Assert.That(response.Data.Body, Is.EqualTo(newPost.Body), "Returned body should match request payload");
    Assert.That(response.Data.UserId, Is.EqualTo(newPost.UserId), "Returned UserId should match request payload");

    // ASSERT that id generated
    Assert.That(response.Data.Id, Is.GreaterThan(0), "Created post should have a generated ID > 0");
}


//PUT /posts/{id}
[Test]
public async Task UpdatePost_ReturnsUpdatedPost()
{
    // ARRANGE
    var updatePost = new UpdatePostRequest
    {
        UserId = 1,
        Title = "Updated title",
        Body = "Updated body"
    };

    // ACT put to /posts/1
    var response = await client.PutAsync<Post>("/posts/1", updatePost);


    // ASSERTS for response
    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "PUT /posts/1 should return HTTP 200 OK");
    Assert.That(response.Data, Is.Not.Null, "Response data should not be null after update operation");

    // ASSERTS for data
    Assert.That(response.Data.Id, Is.EqualTo(1), "Post ID should remain 1 for endpoint /posts/1");
    Assert.That(response.Data.UserId, Is.EqualTo(updatePost.UserId), "UserId should match updated request payload");
    Assert.That(response.Data.Title, Is.EqualTo(updatePost.Title), "Title should be updated to new value");
    Assert.That(response.Data.Body, Is.EqualTo(updatePost.Body), "Body should be updated to new value");
}
   
   
//DELETE /posts/{id}
[Test]
public async Task DeletePost_ReturnsSuccess()
{
    // ARRANGE
    var postId = 1;

    // ACT delete to /posts/1
    var response = await client.DeleteAsync($"/posts/{postId}");

    // ASSERT for response
    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "DELETE /posts/{id} should return HTTP 200 OK");

    var body = response.Data?.ToString();
    Assert.That(body, Is.EqualTo("{}").Or.Empty, "Response body should be empty or {} after successful delete operation");
}


}

