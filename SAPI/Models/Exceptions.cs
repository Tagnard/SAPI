using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPI.Models
{
    class NotAuthenticatedException : Exception
    {
        public NotAuthenticatedException()
        {

        }
    }

    class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException()
        {

        }
    }
}
