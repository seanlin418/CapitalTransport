using FunctionalExtensions;
using Transport.Domain;

namespace Transport.Application.Contract.Repositories
{
    public interface IGitHubUserRepository
    {
        Task<User?> GetUserAsync(LoginName username);
    }
}