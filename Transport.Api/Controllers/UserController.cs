using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Transport.Application.Contract.Dtos;
using Transport.Application.Contract.Queries;

namespace Transport.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UserController(IMediator meditor, IMapper mapper)
        {
            _mediator = meditor ?? throw new ArgumentNullException(nameof(meditor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        public async Task<IActionResult> RetrieveUsers(RequestUsersInfoDto requestUserInfo)
        {
            var query = new GetGitHubUserQuery { Names = requestUserInfo.UserNames };

            var users = await _mediator.Send(query);

            return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
        }
    }
}
