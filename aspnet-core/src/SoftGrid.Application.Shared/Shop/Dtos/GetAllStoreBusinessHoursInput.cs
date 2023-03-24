using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreBusinessHoursInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? NowOpenOrClosedFilter { get; set; }

        public int? IsOpen24HoursFilter { get; set; }

        public string MondayStartTimeFilter { get; set; }

        public string MondayEndTimeFilter { get; set; }

        public string TuesdayStartTimeFilter { get; set; }

        public string TuesdayEndTimeFilter { get; set; }

        public string WednesdayStartTimeFilter { get; set; }

        public string WednesdayEndTimeFilter { get; set; }

        public string ThursdayStartTimeFilter { get; set; }

        public string ThursdayEndTimeFilter { get; set; }

        public string FridayStartTimeFilter { get; set; }

        public string FridayEndTimeFilter { get; set; }

        public string SaturdayStartTimeFilter { get; set; }

        public string SaturdayEndTimeFilter { get; set; }

        public string SundayStartTimeFilter { get; set; }

        public string SundayEndTimeFilter { get; set; }

        public int? IsAcceptOnlyBusinessHourFilter { get; set; }

        public string StoreNameFilter { get; set; }

        public string MasterTagCategoryNameFilter { get; set; }

        public string MasterTagNameFilter { get; set; }

    }
}