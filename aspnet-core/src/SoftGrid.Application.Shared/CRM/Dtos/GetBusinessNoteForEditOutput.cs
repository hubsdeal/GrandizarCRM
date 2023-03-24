using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetBusinessNoteForEditOutput
    {
        public CreateOrEditBusinessNoteDto BusinessNote { get; set; }

        public string BusinessName { get; set; }

    }
}