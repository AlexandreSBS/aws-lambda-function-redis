using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSLambda3
{
    public class Student
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ContactDetails ContactDetails { get; set; }
    }
}
