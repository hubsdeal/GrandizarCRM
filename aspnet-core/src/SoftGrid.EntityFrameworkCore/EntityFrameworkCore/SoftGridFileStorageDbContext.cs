using Microsoft.EntityFrameworkCore;
using SoftGrid.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.Zero.EntityFrameworkCore;
using SoftGrid.MultiTenancy;
using SoftGrid.Authorization.Roles;
using SoftGrid.Authorization.Users;
using Abp.EntityFrameworkCore;

namespace SoftGrid.EntityFrameworkCore
{
    public class SoftGridFileStorageDbContext : AbpDbContext
    {
        public virtual DbSet<FileBinaryData> FileBinaryDatas { get; set; }

        public SoftGridFileStorageDbContext(DbContextOptions<SoftGridFileStorageDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<FileBinaryData>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

        }
    }
}
