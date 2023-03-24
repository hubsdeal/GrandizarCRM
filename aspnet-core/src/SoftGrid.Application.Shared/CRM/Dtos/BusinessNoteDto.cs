using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.CRM.Dtos
{
    public class BusinessNoteDto : EntityDto<long>
    {
        public string Notes { get; set; }

        public long BusinessId { get; set; }

    }
}