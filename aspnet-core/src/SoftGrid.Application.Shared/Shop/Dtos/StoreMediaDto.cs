using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreMediaDto : EntityDto<long>
    {
        public int? DisplaySequence { get; set; }

        public long StoreId { get; set; }

        public long MediaLibraryId { get; set; }

    }
}