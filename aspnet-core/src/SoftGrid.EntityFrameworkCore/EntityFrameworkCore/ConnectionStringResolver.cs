using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Text;
using SoftGrid.Configuration;

namespace SoftGrid.EntityFrameworkCore
{
    public class ConnectionStringResolver : DefaultConnectionStringResolver
    {
        private readonly IConfigurationRoot _appConfiguration;

        public ConnectionStringResolver(IAbpStartupConfiguration configuration, IHostingEnvironment hostingEnvironment)
            : base(configuration)
        {
            _appConfiguration =
                AppConfigurations.Get(hostingEnvironment.ContentRootPath, hostingEnvironment.EnvironmentName);
        }

        public override string GetNameOrConnectionString(ConnectionStringResolveArgs args)
        {
            if (args["DbContextConcreteType"] as Type == typeof(SoftGridFileStorageDbContext))
            {
                return _appConfiguration.GetConnectionString(SoftGridConsts.FileStorageDbConnectionStringName);
            }

            return base.GetNameOrConnectionString(args);
        }
    }
}
