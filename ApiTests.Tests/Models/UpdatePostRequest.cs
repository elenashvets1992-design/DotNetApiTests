namespace ApiTests.Models;

public class UpdatePostRequest
{
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}