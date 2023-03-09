using Abp.Modules;
using Abp.Reflection.Extensions;

namespace SoftGrid
{
    public class SoftGridCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SoftGridCoreSharedModule).GetAssembly());
        }
    }
}