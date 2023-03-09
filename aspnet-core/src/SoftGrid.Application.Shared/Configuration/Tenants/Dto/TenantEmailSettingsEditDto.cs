using Abp.Auditing;
using SoftGrid.Configuration.Dto;

namespace SoftGrid.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}