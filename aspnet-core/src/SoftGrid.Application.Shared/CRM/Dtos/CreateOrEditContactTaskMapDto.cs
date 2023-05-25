using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class CreateOrEditContactTaskMapDto : EntityDto<long?>
    {

        public long ContactId { get; set; }

        public long TaskEventId { get; set; }

    }
}