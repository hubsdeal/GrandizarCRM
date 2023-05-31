using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.WidgetManagement.Exporting
{
    public class StoreThemeMapsExcelExporter : NpoiExcelExporterBase, IStoreThemeMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreThemeMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreThemeMapForViewDto> storeThemeMaps)
        {
            return CreateExcelPackage(
                    "StoreThemeMaps.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("StoreThemeMaps"));

                        AddHeader(
                            sheet,
                        L("Active"),
                        (L("StoreMasterTheme")) + L("Name"),
                        (L("Store")) + L("Name")
                            );

                        AddObjects(
                            sheet, storeThemeMaps,
                        _ => _.StoreThemeMap.Active,
                        _ => _.StoreMasterThemeName,
                        _ => _.StoreName
                            );

                    });

        }
    }
}