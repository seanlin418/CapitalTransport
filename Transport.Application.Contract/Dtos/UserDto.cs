using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport.Application.Contract.Dtos
{
    public class UserDto
    {
        public string Login { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Company { get; set; }
        public int? NumberOfFollowers { get; set; }
        public int? NumberOfPublicRepositories { get; set; }
        public int? AverageNumberOfFollowersPerPublicRepository => NumberOfPublicRepositories > 0 
            ? NumberOfFollowers / NumberOfPublicRepositories 
            : null;
    }
}
