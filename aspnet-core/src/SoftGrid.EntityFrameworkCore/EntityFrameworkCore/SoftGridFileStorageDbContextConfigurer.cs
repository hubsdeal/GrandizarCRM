using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace SoftGrid.EntityFrameworkCore
{
    public class SoftGridFileStorageDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<SoftGridFileStorageDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<SoftGridFileStorageDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
