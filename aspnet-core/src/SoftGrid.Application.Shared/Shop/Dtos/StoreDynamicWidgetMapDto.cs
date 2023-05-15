
using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreDynamicWidgetMapDto : EntityDto<long>
    {
		public string CustomTitle { get; set; }

		public int? DisplaySequence { get; set; }

		public bool Active { get; set; }


		 public long? StoreId { get; set; }

		 		 public long? StoreWidgetId { get; set; }

		 
    }
}