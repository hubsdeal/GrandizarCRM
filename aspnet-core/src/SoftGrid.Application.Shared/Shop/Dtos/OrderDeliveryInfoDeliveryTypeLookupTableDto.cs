using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class OrderDeliveryInfoDeliveryTypeLookupTableDto
    {
		public int Id { get; set; }

		public string DisplayName { get; set; }
		public Guid? PictureId { get; set; }
		public string Picture { get; set; }
    }
}