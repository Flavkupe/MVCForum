using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCForum.Domain.DomainModel
{    
    public class RequestStatus
    {    
        public static RequestStatus Success { get { return new RequestStatus() { Status = "Success" }; } }
        public static RequestStatus Failure { get { return new RequestStatus() { Status = "Failure" }; } }

        public string Status { get; set; }

    }
}
