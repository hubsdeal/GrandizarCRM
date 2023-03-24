using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreOwnerTeamsForExcelInput
    {
        public string Filter { get; set; }

        public int? ActiveFilter { get; set; }

        public int? PrimaryFilter { get; set; }

        public int? OrderEmailNotificationFilter { get; set; }

        public int? OrderSmsNotificationFilter { get; set; }

        public string StoreNameFilter { get; set; }

        public string UserNameFilter { get; set; }

    }
}