using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TenantInspectAdmin.Common.Reusable
{
    public static class MailerHelper
    {
        private static string gettTemplateHTML(bool WithLink, Dictionary<string, string>? emailTemplates)
        {
            string sResult = "";
            try
            {
                string FileName = "EmailTemplateNotificationWithLink";
                if (!WithLink)
                {
                    FileName = "EmailTemplateNotification";
                }
                if (emailTemplates.Any(x => x.Key == FileName))
                {
                    sResult = emailTemplates[FileName];
                }
            }
            catch
            {
            }
            return sResult;
        }
        private static string gettTemplateHTMLTenantInspect(bool WithLink, Dictionary<string, string>? emailTemplates)
        {
            string sResult = "";
            try
            {
                string FileName = "EmailTemplateTenantInspectV2";
                if (!WithLink)
                {
                    FileName = "EmailTemplateNotification";
                }
                if (emailTemplates.Any(x => x.Key == FileName))
                {
                    sResult = emailTemplates[FileName];
                }
            }
            catch
            {
            }
            return sResult;
        }
        private static string gettTemplateHTMLLandlordInspect(bool WithLink, Dictionary<string, string>? emailTemplates)
        {
            string sResult = "";
            try
            {
                string FileName = "EmailTemplateLandlordInspect";
                if (!WithLink)
                {
                    FileName = "EmailTemplateNotification";
                }
                if (emailTemplates.Any(x => x.Key == FileName))
                {
                    sResult = emailTemplates[FileName];
                }
            }
            catch
            {
            }
            return sResult;
        }
        private static string gettTemplateHTMLNew(bool WithLink, Dictionary<string, string>? emailTemplates)
        {
            string sResult = "";
            try
            {
                string FileName = "EmailTemplateNotificationNew";
                if (!WithLink)
                {
                    FileName = "EmailTemplateNotification";
                }
                if (emailTemplates.Any(x => x.Key == FileName))
                {
                    sResult = emailTemplates[FileName];
                }
            }
            catch
            {
            }
            return sResult;
        }

        public static Response SendEmail(string Subject, string ToEmail, string ToName, string Title, string SubTitle, string HtmlBody, string PlainText, string Link, string LinkText, string sendgridkey, Dictionary<string, string>? emailTemplates)
        {
            var apiKey = sendgridkey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("support@leaseprotector.com", "Lease Protector");
            var subject = Subject;
            var to = new EmailAddress(ToEmail, ToName);
            var plainTextContent = PlainText;

            var htmlContent = gettTemplateHTML(Link == null ? false : true, emailTemplates);

            htmlContent = htmlContent.Replace("{{{Title}}}", Title);
            htmlContent = htmlContent.Replace("{{{SubTitle}}}", SubTitle);
            htmlContent = htmlContent.Replace("{{{Message}}}", HtmlBody);

            htmlContent = htmlContent.Replace("{{{Link}}}", Link);
            htmlContent = htmlContent.Replace("{{{LinkText}}}", LinkText);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = client.SendEmailAsync(msg);

            string headers = String.Empty;

            foreach (var key in response.Result.Headers)
            {
                headers += key.Key + "=" + key.Value.ToArray<string>()[0] + "<br>";
                if (key.Key.Trim().ToLower() == "x-message-id")
                {

                }
            }
            return response.Result;
        }
        public static Response SendEmailMoveInspector(string Subject, string ToEmail, string ToName, string Title, string SubTitle, string HtmlBody, string PlainText, string Link, string LinkText, string sendgridkey, Dictionary<string, string>? emailTemplates)
        {
            var apiKey = sendgridkey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("support@leaseprotector.com", "Move Inspector");
            var subject = Subject;
            var to = new EmailAddress(ToEmail, ToName);
            var plainTextContent = PlainText;

            var htmlContent = gettTemplateHTML(Link == null ? false : true, emailTemplates);

            htmlContent = htmlContent.Replace("{{{Title}}}", Title);
            htmlContent = htmlContent.Replace("{{{SubTitle}}}", SubTitle);
            htmlContent = htmlContent.Replace("{{{Message}}}", HtmlBody);

            htmlContent = htmlContent.Replace("{{{Link}}}", Link);
            htmlContent = htmlContent.Replace("{{{LinkText}}}", LinkText);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = client.SendEmailAsync(msg);

            string headers = String.Empty;

            foreach (var key in response.Result.Headers)
            {
                headers += key.Key + "=" + key.Value.ToArray<string>()[0] + "<br>";
                if (key.Key.Trim().ToLower() == "x-message-id")
                {

                }
            }
            return response.Result;
        }
        public static Response SendEmailMoveInspectorNew(string Subject, string ToEmail, string ToName, string Title, string SubTitle, string HtmlBody, string PlainText, string Link, string LinkText, string sendgridkey, Dictionary<string, string>? emailTemplates)
        {
            var apiKey = sendgridkey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("support@leaseprotector.com", "Move Inspector");
            var subject = Subject;
            var to = new EmailAddress(ToEmail, ToName);
            var plainTextContent = PlainText;

            var htmlContent = gettTemplateHTMLNew(Link == null ? false : true, emailTemplates);

            htmlContent = htmlContent.Replace("{{{Title}}}", Title);
            htmlContent = htmlContent.Replace("{{{SubTitle}}}", SubTitle);
            htmlContent = htmlContent.Replace("{{{Message}}}", HtmlBody);

            htmlContent = htmlContent.Replace("{{{Link}}}", Link);
            htmlContent = htmlContent.Replace("{{{LinkText}}}", LinkText);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = client.SendEmailAsync(msg);

            string headers = String.Empty;

            foreach (var key in response.Result.Headers)
            {
                headers += key.Key + "=" + key.Value.ToArray<string>()[0] + "<br>";
                if (key.Key.Trim().ToLower() == "x-message-id")
                {

                }
            }
            return response.Result;
        }
        public static Response SendEmailTenantInspect(string Subject, string ToEmail, string ToName, string Title, string SubTitle, string HtmlBody, string PlainText, string Link, string LinkText, string sendgridkey, Dictionary<string, string>? emailTemplates)
        {
            var apiKey = sendgridkey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("support@tenantinspect.org", "Tenant Inspect");
            var subject = Subject;
            var to = new EmailAddress(ToEmail, ToName);
            var plainTextContent = PlainText;

            var htmlContent = gettTemplateHTMLTenantInspect(Link == null ? false : true, emailTemplates);

            htmlContent = htmlContent.Replace("{{{Title}}}", Title);
            htmlContent = htmlContent.Replace("{{{SubTitle}}}", SubTitle);
            htmlContent = htmlContent.Replace("{{{Message}}}", HtmlBody);

            htmlContent = htmlContent.Replace("{{{Link}}}", Link);
            htmlContent = htmlContent.Replace("{{{LinkText}}}", LinkText);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = client.SendEmailAsync(msg);

            string headers = String.Empty;

            foreach (var key in response.Result.Headers)
            {
                headers += key.Key + "=" + key.Value.ToArray<string>()[0] + "<br>";
                if (key.Key.Trim().ToLower() == "x-message-id")
                {

                }
            }
            return response.Result;
        }
        public static Response SendEmailLandlordInspect(string Subject, string ToEmail, string ToName, string Title, string SubTitle, string HtmlBody, string PlainText, string Link, string LinkText, string sendgridkey, Dictionary<string, string>? emailTemplates)
        {
            var apiKey = sendgridkey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("support@tenantinspect.org", "Landlord Inspect");
            var subject = Subject;
            var to = new EmailAddress(ToEmail, ToName);
            var plainTextContent = PlainText;

            var htmlContent = gettTemplateHTMLLandlordInspect(Link == null ? false : true, emailTemplates);

            htmlContent = htmlContent.Replace("{{{Title}}}", Title);
            htmlContent = htmlContent.Replace("{{{SubTitle}}}", SubTitle);
            htmlContent = htmlContent.Replace("{{{Message}}}", HtmlBody);

            htmlContent = htmlContent.Replace("{{{Link}}}", Link);
            htmlContent = htmlContent.Replace("{{{LinkText}}}", LinkText);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = client.SendEmailAsync(msg);

            string headers = String.Empty;

            foreach (var key in response.Result.Headers)
            {
                headers += key.Key + "=" + key.Value.ToArray<string>()[0] + "<br>";
                if (key.Key.Trim().ToLower() == "x-message-id")
                {

                }
            }
            return response.Result;
        }
    }
}
