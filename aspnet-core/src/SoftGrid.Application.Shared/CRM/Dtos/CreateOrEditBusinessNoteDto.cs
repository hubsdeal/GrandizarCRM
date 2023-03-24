using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class CreateOrEditBusinessNoteDto : EntityDto<long?>
    {

        public string Notes { get; set; }

        public long BusinessId { get; set; }

    }
}