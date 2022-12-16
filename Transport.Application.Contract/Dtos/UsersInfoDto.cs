using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport.Application.Contract.Dtos
{
    public class RequestUsersInfoDto
    {
        public List<string> UserNames { get; set; } = new List<string>();
    }
}
