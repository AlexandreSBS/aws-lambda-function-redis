using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSLambda3
{

    public interface ICacheService
    {
        Task<Student> DoSomethingAsync();
    }
}
