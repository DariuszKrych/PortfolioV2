using System.Text.RegularExpressions;
using System.Net.Http.Headers;
using System.Net;
using Dariusz_Krych___Portfolio.Models;
using System.Text.Json;

namespace Dariusz_Krych___Portfolio.Services;

public class GithubService : IGithubService
{
    private readonly HttpClient _httpClient;

    public GithubService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("PortfolioApp", "1.0"));
    }

    public async Task<List<Github_Project>> GetProjectsAsync(List<string> repoUrls)
    {
        var projects = new List<Github_Project>();

        foreach (var url in repoUrls)
        {
            if (string.IsNullOrWhiteSpace(url)) continue;

            try
            {
                var (owner, repo) = ParseGitHubUrl(url);
                if (owner == null || repo == null) continue;

                var project = await GetProjectDetailsAsync(owner, repo, url);
                if (project != null)
                {
                    projects.Add(project);
                }
            }
            catch (Exception ex)
            {
                // Log error or ignore
                Console.WriteLine($"Error fetching {url}: {ex.Message}");
            }
        }

        return projects;
    }

    private (string? owner, string? repo) ParseGitHubUrl(string url)
    {
        var match = Regex.Match(url, @"github\.com/([^/]+)/([^/]+)");
        if (match.Success)
        {
            return (match.Groups[1].Value, match.Groups[2].Value);
        }
        return (null, null);
    }

    private async Task<Github_Project?> GetProjectDetailsAsync(string owner, string repo, string originalUrl)
    {
        // Get Repo Details (Description, Default Branch)
        var apiUrl = $"https://api.github.com/repos/{owner}/{repo}";
        var response = await _httpClient.GetAsync(apiUrl);
        
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        var name = root.GetProperty("name").GetString() ?? repo;
        var description = root.TryGetProperty("description", out var descProp) ? descProp.GetString() : "";
        var defaultBranch = root.GetProperty("default_branch").GetString() ?? "main";

        // Get README Content
        var readmeUrl = $"https://raw.githubusercontent.com/{owner}/{repo}/{defaultBranch}/README.md";
        var readmeResponse = await _httpClient.GetAsync(readmeUrl);
        var readmeContent = readmeResponse.IsSuccessStatusCode ? await readmeResponse.Content.ReadAsStringAsync() : "";

        // 3. Extract Video URL
        var videoUrl = ExtractVideoUrl(readmeContent);

        // 4. Resolve GitHub User Attachment Redirects
        if (videoUrl != null && videoUrl.Contains("user-attachments/assets"))
        {
            videoUrl = await ResolveRedirectAsync(videoUrl);
        }

        return new Github_Project
        {
            Title = name,
            Description = description ?? "No description available.",
            GitHubLink = originalUrl,
            VideoUrl = videoUrl
        };
    }

    private async Task<string> ResolveRedirectAsync(string url)
    {
        try
        {
            // Create a handler that does NOT follow redirects to manually capture the Location header
            using var handler = new HttpClientHandler { AllowAutoRedirect = false };
            using var client = new HttpClient(handler);
            
            // Mimic a standard browser request to ensure GitHub replies with the redirect
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            
            if (response.StatusCode == HttpStatusCode.Redirect || 
                response.StatusCode == HttpStatusCode.MovedPermanently ||
                response.StatusCode == HttpStatusCode.Found ||
                response.StatusCode == HttpStatusCode.SeeOther)
            {
                var newUrl = response.Headers.Location?.ToString();
                if (!string.IsNullOrEmpty(newUrl))
                {
                    return newUrl;
                }
            }
        }
        catch
        {
            // Fallback to original if resolution fails
        }
        return url;
    }

    private string? ExtractVideoUrl(string content)
    {
        if (string.IsNullOrEmpty(content)) return null;

        // GitHub User Attachments
        // Pattern: https://github.com/user-attachments/assets/GUID
        var assetMatch = Regex.Match(content, @"https://github\.com/user-attachments/assets/[\w-]+", RegexOptions.IgnoreCase);
        if (assetMatch.Success) return assetMatch.Value;

        // Standard video files (.mp4, .webm)
        var videoMatch = Regex.Match(content, @"https?://[^\s()""'<>]+\.(?:mp4|webm)", RegexOptions.IgnoreCase);
        if (videoMatch.Success) return videoMatch.Value;

        // YouTube links
        var youtubeMatch = Regex.Match(content, @"(https?://(?:www\.)?youtube\.com/watch\?v=[\w-]+)|(https?://youtu\.be/[\w-]+)", RegexOptions.IgnoreCase);
        if (youtubeMatch.Success) return youtubeMatch.Value;

        return null;
    }
}
