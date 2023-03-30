using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Territory.Dtos
{
    public class CreateOrEditHubStoreDto : EntityDto<long?>
    {

        public bool Published { get; set; }

        public int? DisplaySequence { get; set; }

        public long HubId { get; set; }

        public long StoreId { get; set; }

    }
}