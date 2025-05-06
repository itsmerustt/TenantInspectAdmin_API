using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenantInspectAdmin.DAL.Apple
{
    public interface IAppleService
    {
        public Task<string> GetAppListAsync();
        public string GenerateToken();
        public Task<string> GetSalesReportAsync(string startDate, string endDate, string filter = "daily");
        public Task<string> GetInstallSourceReportAsync(string startDate, string endDate);
        public Task<string> GetAppVersionAnalyticsAsync(string startDate, string endDate, string versionFilter);
    }
}
