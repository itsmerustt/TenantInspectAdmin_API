using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenantInspectAdmin.Common.Reusable
{
    public static class DateTimeHelper
    {
        public static DateTime GetUTCNow()
        {
            return DateTime.UtcNow;
        }

        public static string GetUTCNowAsString()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
        }
    }
}
