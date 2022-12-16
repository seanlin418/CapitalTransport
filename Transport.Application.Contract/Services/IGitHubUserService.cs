
using FunctionalExtensions;
using Transport.Domain;

namespace Transport.Application.Contract.Services
{
    public interface IGitHubUserService
    {
        Task<IEnumerable<User>> RetrieveUsersAsync(IEnumerable<string?>? names);
    }
}