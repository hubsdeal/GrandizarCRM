using Abp.Modules;
using Abp.Reflection.Extensions;

namespace SoftGrid
{
    [DependsOn(typeof(SoftGridCoreSharedModule))]
    public class SoftGridApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SoftGridApplicationSharedModule).GetAssembly());
        }
    }
}