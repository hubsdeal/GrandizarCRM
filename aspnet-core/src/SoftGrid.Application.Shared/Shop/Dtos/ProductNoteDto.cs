using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductNoteDto : EntityDto<long>
    {
        public string Notes { get; set; }

        public long ProductId { get; set; }

    }
}