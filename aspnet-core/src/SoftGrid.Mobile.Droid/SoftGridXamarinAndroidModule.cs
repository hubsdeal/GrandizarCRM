using Abp.Modules;
using Abp.Reflection.Extensions;

namespace SoftGrid
{
    [DependsOn(typeof(SoftGridXamarinSharedModule))]
    public class SoftGridXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SoftGridXamarinAndroidModule).GetAssembly());
        }
    }
}