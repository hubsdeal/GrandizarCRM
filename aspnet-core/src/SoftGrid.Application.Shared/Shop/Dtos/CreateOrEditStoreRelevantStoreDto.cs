using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditStoreRelevantStoreDto : EntityDto<long?>
    {

        public long RelevantStoreId { get; set; }

        public long PrimaryStoreId { get; set; }

    }
}