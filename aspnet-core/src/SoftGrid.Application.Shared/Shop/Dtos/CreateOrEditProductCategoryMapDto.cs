using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductCategoryMapDto : EntityDto<long?>
    {

        public long ProductId { get; set; }

        public long ProductCategoryId { get; set; }

    }
}