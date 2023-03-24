using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditStoreAccountTeamDto : EntityDto<long?>
    {

        public bool Primary { get; set; }

        public bool Active { get; set; }

        public bool OrderEmailNotification { get; set; }

        public bool OrderSmsNotification { get; set; }

        public long StoreId { get; set; }

        public long EmployeeId { get; set; }

    }
}