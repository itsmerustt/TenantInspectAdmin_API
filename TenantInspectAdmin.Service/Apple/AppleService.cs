using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TenantInspectAdmin.DAL.Apple;

namespace TenantInspectAdmin.Service.Apple
{
    public class AppleService : IAppleService
    {

        private readonly string _keyId = "GMGD2Q3DF7";
        private readonly string _issuerId = "9af61435-3f87-4a78-98f1-9cba06769ae0";
        private readonly string _p8PrivateKey = @"-----BEGIN PRIVATE KEY-----
        MIGTAgEAMBMGByqGSM49AgEGCCqGSM49AwEHBHkwdwIBAQQgEn/Fuddwa5iAFpNp
        fNTanVlnSNirBJkuLlRJJMSKa/KgCgYIKoZIzj0DAQehRANCAARj0BlCNMqNX09T
        NeBoZOYZXWp+ga28ZknicbSi45JYb7kfZNqUXl0zMTOSAHcCS1IuPMd7rDV56Vwe
        DKH/ZjMk
        -----END PRIVATE KEY-----";

        public string GenerateToken()
        {
            string cleanedKey = _p8PrivateKey
                .Replace("-----BEGIN PRIVATE KEY-----", "")
                .Replace("-----END PRIVATE KEY-----", "")
                .Replace("\n", "")
                .Replace("\r", "")
                .Trim();

            byte[] privateKeyBytes = Convert.FromBase64String(cleanedKey);

            using var ecdsa = ECDsa.Create();
            ecdsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);

            var securityKey = new ECDsaSecurityKey(ecdsa)
            {
                KeyId = _keyId // ✅ This embeds the 'kid' automatically
            };

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.EcdsaSha256);

            var now = DateTimeOffset.UtcNow;
            var payload = new JwtPayload
    {
        { "iss", _issuerId },
        { "iat", now.ToUnixTimeSeconds() },
        { "exp", now.AddMinutes(20).ToUnixTimeSeconds() },
        { "aud", "appstoreconnect-v1" }
    };

            // ❌ DO NOT manually add "kid"
            var header = new JwtHeader(credentials);

            var token = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<string> GetAppListAsync()
        {
            // Generate token before making the request
            var token = GenerateToken(); // Ensure GenerateToken() is synchronous here

            // Create a new HttpClient instance to make the request
            using (var _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("https://api.appstoreconnect.apple.com/v1/apps");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API failed: {error}");
                }

                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }

        // Get sales report (Daily, Weekly, Monthly)
        public async Task<string> GetSalesReportAsync(string startDate, string endDate, string filter = "daily")
        {
            var token = GenerateToken(); // Token generated before making the request

            using (var _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // API endpoint for sales reports   
                var url = $"https://api.appstoreconnect.apple.com/v1/salesReports?filter[date]={startDate},{endDate}&filter[frequency]={filter}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API failed: {error}");
                }

                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }

        // Get install source report
        public async Task<string> GetInstallSourceReportAsync(string startDate, string endDate)
        {
            var token = GenerateToken(); // Token generated before making the request

            using (var _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Modify the URL to get install sources
                var url = $"https://api.appstoreconnect.apple.com/v1/analyticsReports?filter[date]={startDate},{endDate}&filter[metric]=installs";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API failed: {error}");
                }

                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }

        // Get app version analytics
        public async Task<string> GetAppVersionAnalyticsAsync(string startDate, string endDate, string versionFilter)
        {
            var token = GenerateToken(); // Token generated before making the request

            using (var _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // API endpoint to fetch analytics with a filter on app version
                var url = $"https://api.appstoreconnect.apple.com/v1/analyticsReports?filter[date]={startDate},{endDate}&filter[version]={versionFilter}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API failed: {error}");
                }

                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }


        private static string ExtractPrivateKey(string pem)
        {
            return pem.Replace("-----BEGIN PRIVATE KEY-----", "")
                      .Replace("-----END PRIVATE KEY-----", "")
                      .Replace("\n", "")
                      .Replace("\r", "")
                      .Trim();
        }
    }
}
