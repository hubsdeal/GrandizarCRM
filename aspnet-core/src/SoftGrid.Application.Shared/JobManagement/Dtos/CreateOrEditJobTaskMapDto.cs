using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.JobManagement.Dtos
{
    public class CreateOrEditJobTaskMapDto : EntityDto<long?>
    {

        public long JobId { get; set; }

        public long TaskEventId { get; set; }

    }
}