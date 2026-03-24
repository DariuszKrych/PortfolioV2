using Dariusz_Krych___Portfolio.Models;

namespace Dariusz_Krych___Portfolio.Services;

public interface IGithubService
{
    Task<List<Github_Project>> GetProjectsAsync(List<string> repoUrls);
}
