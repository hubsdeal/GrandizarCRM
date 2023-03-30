using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductMediaDto : EntityDto<long>
    {
        public int? DisplaySequence { get; set; }

        public long ProductId { get; set; }

        public long? MediaLibraryId { get; set; }

    }
}