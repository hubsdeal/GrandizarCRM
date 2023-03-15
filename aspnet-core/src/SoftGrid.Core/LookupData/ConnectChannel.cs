using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("ConnectChannels")]
    public class ConnectChannel : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Name { get; set; }

    }
}