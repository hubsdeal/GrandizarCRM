using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreBusinessHourDto : EntityDto<long>
    {
        public bool NowOpenOrClosed { get; set; }

        public bool IsOpen24Hours { get; set; }

        public string MondayStartTime { get; set; }

        public string MondayEndTime { get; set; }

        public string TuesdayStartTime { get; set; }

        public string TuesdayEndTime { get; set; }

        public string WednesdayStartTime { get; set; }

        public string WednesdayEndTime { get; set; }

        public string ThursdayStartTime { get; set; }

        public string ThursdayEndTime { get; set; }

        public string FridayStartTime { get; set; }

        public string FridayEndTime { get; set; }

        public string SaturdayStartTime { get; set; }

        public string SaturdayEndTime { get; set; }

        public string SundayStartTime { get; set; }

        public string SundayEndTime { get; set; }

        public bool IsAcceptOnlyBusinessHour { get; set; }

        public long StoreId { get; set; }

        public long? MasterTagCategoryId { get; set; }

        public long? MasterTagId { get; set; }

    }
}