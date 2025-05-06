using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenantInspectAdmin.Common
{
    public class GenericAPIResponse
    {
        public bool isSuccess { get; set; }
        public string? Message { get; set; }
        public object Data { get; set; }
    }
}
