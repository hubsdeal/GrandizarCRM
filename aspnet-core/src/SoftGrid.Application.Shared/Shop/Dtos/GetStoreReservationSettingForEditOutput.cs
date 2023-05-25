using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreReservationSettingForEditOutput
    {
        public CreateOrEditStoreReservationSettingDto StoreReservationSetting { get; set; }

        public string StoreName { get; set; }

    }
}