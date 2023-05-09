using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditWishListDto : EntityDto<long?>
    {

        public DateTime? Date { get; set; }

        public long ContactId { get; set; }

        public long? ProductId { get; set; }

        public long? StoreId { get; set; }

    }
}