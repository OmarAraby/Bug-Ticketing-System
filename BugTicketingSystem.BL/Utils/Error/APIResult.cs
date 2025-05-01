using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.BL.Utils.Error
{
    public class APIResult
    {
        public bool Success { get; set; }
        public APIError[] Errors { get; set; } = [];
    }


    public class APIResult<T> : APIResult
    {
        public T? Data { get; set; }
    }

}
