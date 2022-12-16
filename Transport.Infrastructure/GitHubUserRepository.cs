using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Transport.Application.Contract.Dtos;
using Transport.Application.Contract.Repositories;
using Transport.Domain;

namespace Transport.Infrastructure
{
    public class GitHubUserRepository : IGitHubUserRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GitHubUserRepository> _logger;

        public GitHubUserRepository(HttpClient httpClient, ILogger<GitHubUserRepository> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(HttpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(ILogger<GitHubUserRepository>));
        }

        public async Task<User?> GetUserAsync(LoginName username)
        {
            var response = await _httpClient.GetAsync($"users/{username.Name}");

            var responseBody = await response.Content.ReadAsStringAsync();

            GitHubUser? gitHubUser = null;
            try
            {
                gitHubUser = JsonConvert.DeserializeObject<GitHubUser>(responseBody);
            }
            catch
            {
                return null;
            }

            if (gitHubUser == null)
            {
                return null;
            }

            var createUserNameResult = LoginName.CreateInstance(gitHubUser.Login);

            if (createUserNameResult.IsFailure)
            {
                _logger.LogError(createUserNameResult.Error);

                return null;
            }

            var createPersonNameResult = PersonName.CreateInstance(gitHubUser.Name);

            var createCompanyResult = CompanyName.CreateInstance(gitHubUser.Company);

            var user = new User(createUserNameResult.Value, createPersonNameResult.Value, createCompanyResult.Value, gitHubUser.NumberOfFollowers, gitHubUser.NumberOfPublicRepositories);

            return user;
        }
    }
}