using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Custom_Exceptions.ExceptionClasses
{
    public class ValidationException:Exception
    {
        public Dictionary<string, string[]>? Errors { get; set; }
        public ValidationException(string message, Dictionary<string, string[]>? errors = null)
            : base(message)
        {
            Errors = errors;
        }
        
    }
}
