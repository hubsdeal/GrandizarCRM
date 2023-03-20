using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SoftGrid.Configuration;
using SoftGrid.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftGrid.EntityFrameworkCore
{
    public class SoftGridFileStorageDbContextFactory : IDesignTimeDbContextFactory<SoftGridFileStorageDbContext>
    {
        public SoftGridFileStorageDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SoftGridFileStorageDbContext>();
            var configuration = AppConfigurations.Get(
                WebContentDirectoryFinder.CalculateContentRootFolder(),
                addUserSecrets: true
            );

            SoftGridFileStorageDbContextConfigurer.Configure(builder, configuration.GetConnectionString(SoftGridConsts.FileStorageDbConnectionStringName));

            return new SoftGridFileStorageDbContext(builder.Options);
        }
    }
}
