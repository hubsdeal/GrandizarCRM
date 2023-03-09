using Abp.AspNetCore.Mvc.Views;

namespace SoftGrid.Web.Views
{
    public abstract class SoftGridRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected SoftGridRazorPage()
        {
            LocalizationSourceName = SoftGridConsts.LocalizationSourceName;
        }
    }
}
