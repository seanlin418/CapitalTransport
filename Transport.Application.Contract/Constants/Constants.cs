using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transport.Application.Contract.Constants
{
    public static class ApplicationConstants
    {
        public const string BadRequestExceptionMessage = "The request could not be proceessed due to invalid data sent to the server";
        public const string GeneralExceptionMessage = "Something went wrong, the problem is logged and will be looking into it";
    }
}
