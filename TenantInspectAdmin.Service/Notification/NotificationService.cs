using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenantInspectAdmin.DAL.Notification;
using TenantInspectAdmin.DAL.Repository;

namespace TenantInspectAdmin.Service.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly FirebaseApp _firebaseApp;
        private readonly ITenantInspectAdminRepository _tenantinspectRepository;
        // Constructor now takes the initialized FirebaseApp from Program.cs
        public NotificationService(FirebaseApp firebaseApp)
        {
            _firebaseApp = firebaseApp;

        }

        // Implement the method from INotificationService interface
        public async Task<string> SendPushNotificationAsync(string token, string title, string body)
        {
            var message = new Message()
            {
                Token = token, // Device token
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = title,
                    Body = body
                }
            };

            try
            {
                // Send the message to Firebase Cloud Messaging
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                //ToggerInsertViewModel loggerBody = new ToggerInsertViewModel
                //{
                //    TransactionId = 0,
                //    Name = "API: Send Push Notif Async method",
                //    Details = "Send push",
                //    APIResponse = response,
                //    APIRequest = JsonConvert.SerializeObject(message),
                //    DateCreated = DateTime.Now,
                //    IsActiveError = true
                //};
                //bool isSuccess = await _tenantinspectRepository.TransactionAsync("TransactionLogsInsert", loggerBody);
                return response;
                Console.WriteLine($"Successfully sent message: {response}");
            }
            catch (Exception ex)
            {
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                //ToggerInsertViewModel loggerBody = new ToggerInsertViewModel
                //{
                //    TransactionId = 0,
                //    Name = "API: Send Push Notif Async method catch",
                //    Details = "Send push catch",
                //    APIResponse = response,
                //    APIRequest = JsonConvert.SerializeObject(message),
                //    DateCreated = DateTime.Now,
                //    IsActiveError = true
                //};
                //bool isSuccess = await _tenantinspectRepository.TransactionAsync("TransactionLogsInsert", loggerBody);
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
            return string.Empty;
        }
        public async Task<string> SendDataOnlyNotificationAsync(string token, string title, string body)
        {
            var message = new Message()
            {
                Token = token, // Device token
                Data = new Dictionary<string, string>
        {
            { "title", title },
            { "body", body }
        }
            };

            try
            {
                // Send the data-only message to Firebase Cloud Messaging
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine($"Successfully sent data-only message: {response}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending data-only message: {ex.Message}");
                return string.Empty;
            }
        }

    }
}
