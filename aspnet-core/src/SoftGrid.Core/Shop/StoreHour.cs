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
	[Table("StoreHours")]
    public class StoreHour : CreationAuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual bool NowOpenOrClosed { get; set; }
		
		public virtual bool IsOpen24Hours { get; set; }
		
		[StringLength(StoreHourConsts.MaxMondayStartTimeLength, MinimumLength = StoreHourConsts.MinMondayStartTimeLength)]
		public virtual string MondayStartTime { get; set; }
		
		[StringLength(StoreHourConsts.MaxMondayEndTimeLength, MinimumLength = StoreHourConsts.MinMondayEndTimeLength)]
		public virtual string MondayEndTime { get; set; }
		
		[StringLength(StoreHourConsts.MaxTuesdayStartTimeLength, MinimumLength = StoreHourConsts.MinTuesdayStartTimeLength)]
		public virtual string TuesdayStartTime { get; set; }
		
		[StringLength(StoreHourConsts.MaxTuesdayEndTimeLength, MinimumLength = StoreHourConsts.MinTuesdayEndTimeLength)]
		public virtual string TuesdayEndTime { get; set; }
		
		[StringLength(StoreHourConsts.MaxWednesdayStartTimeLength, MinimumLength = StoreHourConsts.MinWednesdayStartTimeLength)]
		public virtual string WednesdayStartTime { get; set; }
		
		[StringLength(StoreHourConsts.MaxWednesdayEndTimeLength, MinimumLength = StoreHourConsts.MinWednesdayEndTimeLength)]
		public virtual string WednesdayEndTime { get; set; }
		
		[StringLength(StoreHourConsts.MaxThursdayStartTimeLength, MinimumLength = StoreHourConsts.MinThursdayStartTimeLength)]
		public virtual string ThursdayStartTime { get; set; }
		
		[StringLength(StoreHourConsts.MaxThursdayEndTimeLength, MinimumLength = StoreHourConsts.MinThursdayEndTimeLength)]
		public virtual string ThursdayEndTime { get; set; }
		
		[StringLength(StoreHourConsts.MaxFridayStartTimeLength, MinimumLength = StoreHourConsts.MinFridayStartTimeLength)]
		public virtual string FridayStartTime { get; set; }
		
		[StringLength(StoreHourConsts.MaxFridayEndTimeLength, MinimumLength = StoreHourConsts.MinFridayEndTimeLength)]
		public virtual string FridayEndTime { get; set; }
		
		[StringLength(StoreHourConsts.MaxSaturdayStartTimeLength, MinimumLength = StoreHourConsts.MinSaturdayStartTimeLength)]
		public virtual string SaturdayStartTime { get; set; }
		
		[StringLength(StoreHourConsts.MaxSaturdayEndTimeLength, MinimumLength = StoreHourConsts.MinSaturdayEndTimeLength)]
		public virtual string SaturdayEndTime { get; set; }
		
		[StringLength(StoreHourConsts.MaxSundayStartTimeLength, MinimumLength = StoreHourConsts.MinSundayStartTimeLength)]
		public virtual string SundayStartTime { get; set; }
		
		[StringLength(StoreHourConsts.MaxSundayEndTimeLength, MinimumLength = StoreHourConsts.MinSundayEndTimeLength)]
		public virtual string SundayEndTime { get; set; }
		

		public virtual long? StoreId { get; set; }
		
        [ForeignKey("StoreId")]
		public Store StoreFk { get; set; }
		
		public virtual long? MasterTagCategoryId { get; set; }
		
        [ForeignKey("MasterTagCategoryId")]
		public MasterTagCategory MasterTagCategoryFk { get; set; }
		
		public virtual long? MasterTagId { get; set; }
		
        [ForeignKey("MasterTagId")]
		public MasterTag MasterTagFk { get; set; }

		public virtual bool? IsAcceptOrdersOnlyBusinessHour { get; set; }
		
    }
}