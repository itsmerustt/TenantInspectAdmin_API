using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenantInspectAdmin.Common
{
    public class BlobResult
    {
        public bool Successfull { get; set; }
        public string ErrorMessage { get; set; }
        public string AbsuloteUri { get; set; }
        public string FileName { get; set; }
    }
}
