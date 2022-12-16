using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transport.Domain;

namespace Transport.Application.Contract.Queries
{
    public class GetGitHubUserQuery : IRequest<IEnumerable<User>>
    {
        public IEnumerable<string> Names { get; set; } = new List<string>();
    }
}
