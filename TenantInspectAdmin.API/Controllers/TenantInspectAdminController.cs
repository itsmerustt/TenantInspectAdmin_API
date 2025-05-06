using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenantInspectAdmin.DAL.Apple;
using TenantInspectAdmin.DAL.Notification;
using TenantInspectAdmin.DAL.Repository;

namespace TenantInspectAdmin.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = false)]
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class TenantInspectAdminController : ControllerBase
    {
        private readonly ILogger<TenantInspectAdminController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ITenantInspectAdminRepository _tenantInspectRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string _storageAccountString;
        private readonly string _sendgridkey;
        private readonly string _sendgridkeyTenantInspect;
        private readonly INotificationService _notif;
        private readonly IAppleService _apple;

        public TenantInspectAdminController(ITenantInspectAdminRepository tenantInspectRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment, ILogger<TenantInspectAdminController> logger, INotificationService notif, IAppleService apple)
        {
            _logger = logger;
            _hostEnvironment = environment;
            _configuration = configuration;
            _tenantInspectRepository = tenantInspectRepository;
            this.httpContextAccessor = httpContextAccessor;
            _sendgridkey = _configuration.GetConnectionString("SendGridKey");
            _sendgridkeyTenantInspect = _configuration.GetConnectionString("SendGridKeyTenantInspect");
            _storageAccountString = _configuration.GetConnectionString("StorageAccountString");
            _notif = notif;
            _apple = apple;
        }

        #region API Controllers Here

        #endregion
    }
}
