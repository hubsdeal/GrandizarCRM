using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SoftGrid.Configure;
using SoftGrid.Startup;
using SoftGrid.Test.Base;

namespace SoftGrid.GraphQL.Tests
{
    [DependsOn(
        typeof(SoftGridGraphQLModule),
        typeof(SoftGridTestBaseModule))]
    public class SoftGridGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SoftGridGraphQLTestModule).GetAssembly());
        }
    }
}