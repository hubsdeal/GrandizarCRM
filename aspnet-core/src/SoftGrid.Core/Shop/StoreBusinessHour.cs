using SoftGrid.Shop;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("StoreBusinessHours")]
    public class StoreBusinessHour : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual bool NowOpenOrClosed { get; set; }

        public virtual bool IsOpen24Hours { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxMondayStartTimeLength, MinimumLength = StoreBusinessHourConsts.MinMondayStartTimeLength)]
        public virtual string MondayStartTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxMondayEndTimeLength, MinimumLength = StoreBusinessHourConsts.MinMondayEndTimeLength)]
        public virtual string MondayEndTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxTuesdayStartTimeLength, MinimumLength = StoreBusinessHourConsts.MinTuesdayStartTimeLength)]
        public virtual string TuesdayStartTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxTuesdayEndTimeLength, MinimumLength = StoreBusinessHourConsts.MinTuesdayEndTimeLength)]
        public virtual string TuesdayEndTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxWednesdayStartTimeLength, MinimumLength = StoreBusinessHourConsts.MinWednesdayStartTimeLength)]
        public virtual string WednesdayStartTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxWednesdayEndTimeLength, MinimumLength = StoreBusinessHourConsts.MinWednesdayEndTimeLength)]
        public virtual string WednesdayEndTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxThursdayStartTimeLength, MinimumLength = StoreBusinessHourConsts.MinThursdayStartTimeLength)]
        public virtual string ThursdayStartTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxThursdayEndTimeLength, MinimumLength = StoreBusinessHourConsts.MinThursdayEndTimeLength)]
        public virtual string ThursdayEndTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxFridayStartTimeLength, MinimumLength = StoreBusinessHourConsts.MinFridayStartTimeLength)]
        public virtual string FridayStartTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxFridayEndTimeLength, MinimumLength = StoreBusinessHourConsts.MinFridayEndTimeLength)]
        public virtual string FridayEndTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxSaturdayStartTimeLength, MinimumLength = StoreBusinessHourConsts.MinSaturdayStartTimeLength)]
        public virtual string SaturdayStartTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxSaturdayEndTimeLength, MinimumLength = StoreBusinessHourConsts.MinSaturdayEndTimeLength)]
        public virtual string SaturdayEndTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxSundayStartTimeLength, MinimumLength = StoreBusinessHourConsts.MinSundayStartTimeLength)]
        public virtual string SundayStartTime { get; set; }

        [StringLength(StoreBusinessHourConsts.MaxSundayEndTimeLength, MinimumLength = StoreBusinessHourConsts.MinSundayEndTimeLength)]
        public virtual string SundayEndTime { get; set; }

        public virtual bool IsAcceptOnlyBusinessHour { get; set; }

        public virtual long StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store StoreFk { get; set; }

        public virtual long? MasterTagCategoryId { get; set; }

        [ForeignKey("MasterTagCategoryId")]
        public MasterTagCategory MasterTagCategoryFk { get; set; }

        public virtual long? MasterTagId { get; set; }

        [ForeignKey("MasterTagId")]
        public MasterTag MasterTagFk { get; set; }

    }
}