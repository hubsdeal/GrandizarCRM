using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreNoteDto : EntityDto<long>
    {
        public string Notes { get; set; }

        public long StoreId { get; set; }

    }
}