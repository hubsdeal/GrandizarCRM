using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
    [Table("ContractTypes")]
    public class ContractType : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(ContractTypeConsts.MaxNameLength, MinimumLength = ContractTypeConsts.MinNameLength)]
        public virtual string Name { get; set; }

    }
}