using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.TaskManagement.Dtos
{
    public class CreateOrEditStoreTaskMapDto : EntityDto<long?>
    {

        public long StoreId { get; set; }

        public long TaskEventId { get; set; }

    }
}