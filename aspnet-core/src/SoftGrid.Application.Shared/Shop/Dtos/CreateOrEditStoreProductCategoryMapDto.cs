using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditStoreProductCategoryMapDto : EntityDto<long?>
    {

        public bool Published { get; set; }

        public int? DisplaySequence { get; set; }

        public long StoreId { get; set; }

        public long ProductCategoryId { get; set; }

    }
}