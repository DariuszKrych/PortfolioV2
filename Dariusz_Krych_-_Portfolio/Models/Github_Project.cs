namespace Dariusz_Krych___Portfolio.Models;

public class Github_Project
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string GitHubLink { get; set; } = string.Empty;
    public string? VideoUrl { get; set; }
}