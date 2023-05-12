using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.TaskManagement.Dtos
{
    public class StoreTaskMapDto : EntityDto<long>
    {

        public long StoreId { get; set; }

        public long TaskEventId { get; set; }

    }
}