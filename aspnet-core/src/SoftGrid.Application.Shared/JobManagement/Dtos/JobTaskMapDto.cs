using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.JobManagement.Dtos
{
    public class JobTaskMapDto : EntityDto<long>
    {

        public long JobId { get; set; }

        public long TaskEventId { get; set; }

    }
}