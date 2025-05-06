using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenantInspectAdmin.DAL.Notification
{
    public interface INotificationService
    {
        Task<string> SendPushNotificationAsync(string token, string title, string body);
        Task<string> SendDataOnlyNotificationAsync(string token, string title, string body);
    }
}
