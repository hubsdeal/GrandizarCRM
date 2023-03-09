using Abp.Domain.Services;

namespace SoftGrid
{
    public abstract class SoftGridDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected SoftGridDomainServiceBase()
        {
            LocalizationSourceName = SoftGridConsts.LocalizationSourceName;
        }
    }
}
