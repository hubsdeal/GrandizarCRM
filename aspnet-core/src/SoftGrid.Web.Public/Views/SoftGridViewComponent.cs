using Abp.AspNetCore.Mvc.ViewComponents;

namespace SoftGrid.Web.Public.Views
{
    public abstract class SoftGridViewComponent : AbpViewComponent
    {
        protected SoftGridViewComponent()
        {
            LocalizationSourceName = SoftGridConsts.LocalizationSourceName;
        }
    }
}