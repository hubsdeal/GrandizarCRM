using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace SoftGrid.EntityFrameworkCore
{
    public static class SoftGridDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<SoftGridDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<SoftGridDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}