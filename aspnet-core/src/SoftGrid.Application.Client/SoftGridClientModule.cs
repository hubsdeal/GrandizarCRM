using Abp.Modules;
using Abp.Reflection.Extensions;

namespace SoftGrid
{
    public class SoftGridClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SoftGridClientModule).GetAssembly());
        }
    }
}
