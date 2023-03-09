using Abp.Modules;
using Abp.Reflection.Extensions;

namespace SoftGrid
{
    [DependsOn(typeof(SoftGridXamarinSharedModule))]
    public class SoftGridXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SoftGridXamarinIosModule).GetAssembly());
        }
    }
}