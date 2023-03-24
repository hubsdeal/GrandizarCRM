using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreBusinessHoursExcelExporter : NpoiExcelExporterBase, IStoreBusinessHoursExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreBusinessHoursExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreBusinessHourForViewDto> storeBusinessHours)
        {
            return CreateExcelPackage(
                "StoreBusinessHours.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("StoreBusinessHours"));

                    AddHeader(
                        sheet,
                        L("NowOpenOrClosed"),
                        L("IsOpen24Hours"),
                        L("MondayStartTime"),
                        L("MondayEndTime"),
                        L("TuesdayStartTime"),
                        L("TuesdayEndTime"),
                        L("WednesdayStartTime"),
                        L("WednesdayEndTime"),
                        L("ThursdayStartTime"),
                        L("ThursdayEndTime"),
                        L("FridayStartTime"),
                        L("FridayEndTime"),
                        L("SaturdayStartTime"),
                        L("SaturdayEndTime"),
                        L("SundayStartTime"),
                        L("SundayEndTime"),
                        L("IsAcceptOnlyBusinessHour"),
                        (L("Store")) + L("Name"),
                        (L("MasterTagCategory")) + L("Name"),
                        (L("MasterTag")) + L("Name")
                        );

                    AddObjects(
                        sheet, storeBusinessHours,
                        _ => _.StoreBusinessHour.NowOpenOrClosed,
                        _ => _.StoreBusinessHour.IsOpen24Hours,
                        _ => _.StoreBusinessHour.MondayStartTime,
                        _ => _.StoreBusinessHour.MondayEndTime,
                        _ => _.StoreBusinessHour.TuesdayStartTime,
                        _ => _.StoreBusinessHour.TuesdayEndTime,
                        _ => _.StoreBusinessHour.WednesdayStartTime,
                        _ => _.StoreBusinessHour.WednesdayEndTime,
                        _ => _.StoreBusinessHour.ThursdayStartTime,
                        _ => _.StoreBusinessHour.ThursdayEndTime,
                        _ => _.StoreBusinessHour.FridayStartTime,
                        _ => _.StoreBusinessHour.FridayEndTime,
                        _ => _.StoreBusinessHour.SaturdayStartTime,
                        _ => _.StoreBusinessHour.SaturdayEndTime,
                        _ => _.StoreBusinessHour.SundayStartTime,
                        _ => _.StoreBusinessHour.SundayEndTime,
                        _ => _.StoreBusinessHour.IsAcceptOnlyBusinessHour,
                        _ => _.StoreName,
                        _ => _.MasterTagCategoryName,
                        _ => _.MasterTagName
                        );

                });
        }
    }
}