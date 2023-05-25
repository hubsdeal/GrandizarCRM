using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.LookupData
{
	[Table("DeliveryTypes")]
    public class DeliveryType : Entity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		[StringLength(DeliveryTypeConsts.MaxNameLength, MinimumLength = DeliveryTypeConsts.MinNameLength)]
		public virtual string Name { get; set; }
		
		public virtual string Description { get; set; }
		
		public virtual Guid? PictureId { get; set; }
    }
}