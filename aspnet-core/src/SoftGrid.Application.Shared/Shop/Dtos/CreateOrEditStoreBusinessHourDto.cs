using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditStoreBusinessHourDto : EntityDto<long?>
    {

        public bool NowOpenOrClosed { get; set; }

        public bool IsOpen24Hours { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxMondayStartTimeLength, MinimumLength = StoreBusinessHourConsts.MinMondayStartTimeLength)]
        public string MondayStartTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxMondayEndTimeLength, MinimumLength = StoreBusinessHourConsts.MinMondayEndTimeLength)]
        public string MondayEndTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxTuesdayStartTimeLength, MinimumLength = StoreBusinessHourConsts.MinTuesdayStartTimeLength)]
        public string TuesdayStartTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxTuesdayEndTimeLength, MinimumLength = StoreBusinessHourConsts.MinTuesdayEndTimeLength)]
        public string TuesdayEndTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxWednesdayStartTimeLength, MinimumLength = StoreBusinessHourConsts.MinWednesdayStartTimeLength)]
        public string WednesdayStartTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxWednesdayEndTimeLength, MinimumLength = StoreBusinessHourConsts.MinWednesdayEndTimeLength)]
        public string WednesdayEndTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxThursdayStartTimeLength, MinimumLength = StoreBusinessHourConsts.MinThursdayStartTimeLength)]
        public string ThursdayStartTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxThursdayEndTimeLength, MinimumLength = StoreBusinessHourConsts.MinThursdayEndTimeLength)]
        public string ThursdayEndTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxFridayStartTimeLength, MinimumLength = StoreBusinessHourConsts.MinFridayStartTimeLength)]
        public string FridayStartTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxFridayEndTimeLength, MinimumLength = StoreBusinessHourConsts.MinFridayEndTimeLength)]
        public string FridayEndTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxSaturdayStartTimeLength, MinimumLength = StoreBusinessHourConsts.MinSaturdayStartTimeLength)]
        public string SaturdayStartTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxSaturdayEndTimeLength, MinimumLength = StoreBusinessHourConsts.MinSaturdayEndTimeLength)]
        public string SaturdayEndTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxSundayStartTimeLength, MinimumLength = StoreBusinessHourConsts.MinSundayStartTimeLength)]
        public string SundayStartTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxSundayEndTimeLength, MinimumLength = StoreBusinessHourConsts.MinSundayEndTimeLength)]
        public string SundayEndTime { get; set; }

        public bool IsAcceptOnlyBusinessHour { get; set; }

        public long StoreId { get; set; }

        public long? MasterTagCategoryId { get; set; }

        public long? MasterTagId { get; set; }

    }
}