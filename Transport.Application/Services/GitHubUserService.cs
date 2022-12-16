using FunctionalExtensions;
using Microsoft.Extensions.Logging;
using Transport.Application.Contract.Repositories;
using Transport.Application.Contract.Services;
using Transport.Domain;

namespace Transport.Application.Services
{
    public class GitHubUserService : IGitHubUserService
    {
        private readonly IGitHubUserRepository _userRepository;
        private readonly ILogger<GitHubUserService> _logger;

        public GitHubUserService(IGitHubUserRepository userRepository, ILogger<GitHubUserService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(HttpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(ILogger<GitHubUserService>));
        }

        public async Task<IEnumerable<User>> RetrieveUsersAsync(IEnumerable<string?>? names)
        {
            var userNames = ExtractUserNames(names);

            if (userNames?.Any() != true)
            {
                _logger.LogInformation("No valid user name provided for retrieving users");
                return new List<User>();
            }

            var users = new List<User>();

            foreach (var currentUserName in userNames)
            {
                var currentUser = await _userRepository.GetUserAsync(currentUserName);

                if (currentUser == null || users.Any(w => w == currentUser))
                {
                    continue;
                }

                users.Add(currentUser);
            }

            return users
                .OrderByDescending(w => !string.IsNullOrEmpty(w.Name.Name))
                .ThenBy(w => w.Name.Name);
        }

        private IEnumerable<LoginName> ExtractUserNames(IEnumerable<string?>? names)
        {
            var userNames = new List<LoginName>();

            if (names?.Any() != true)
                return userNames;

            foreach (var name in names)
            {
                var result = LoginName.CreateInstance(name);

                if (result.IsFailure)
                {
                    _logger.LogError(result.Error);
                    continue;
                }

                if (userNames.Any(userName => userName == result.Value))
                {
                    _logger.LogInformation($"Duplicate user found: {result.Value.Name}");
                    continue;
                }

                //get distinct user names
                userNames.Add(result.Value);
            }

            return userNames;
        }
    }
}
