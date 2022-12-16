using MediatR;
using Transport.Application.Contract.Services;
using Transport.Domain;

namespace Transport.Application.Contract.Queries
{
    public sealed class GetGitHubQueryHandler : IRequestHandler<GetGitHubUserQuery, IEnumerable<User>>
    {
        private readonly IGitHubUserService _userService;

        public GetGitHubQueryHandler(IGitHubUserService userService)
        {
            _userService = userService;
        }

        public async Task<IEnumerable<User>> Handle(GetGitHubUserQuery request, CancellationToken cancellationToken)
        {
            var users = await _userService.RetrieveUsersAsync(request.Names);

            return users;
        }
    }
}
