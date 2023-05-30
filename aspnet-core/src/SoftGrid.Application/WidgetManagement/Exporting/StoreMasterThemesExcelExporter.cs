using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.WidgetManagement.Exporting
{
    public class StoreMasterThemesExcelExporter : NpoiExcelExporterBase, IStoreMasterThemesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreMasterThemesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreMasterThemeForViewDto> storeMasterThemes)
        {
            return CreateExcelPackage(
                    "StoreMasterThemes.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("StoreMasterThemes"));

                        AddHeader(
                            sheet,
                        L("Name"),
                        L("Description"),
                        L("ThemeCode"),
                        L("ThumbnailImageId")
                            );

                        AddObjects(
                            sheet, storeMasterThemes,
                        _ => _.StoreMasterTheme.Name,
                        _ => _.StoreMasterTheme.Description,
                        _ => _.StoreMasterTheme.ThemeCode,
                        _ => _.StoreMasterTheme.ThumbnailImageId
                            );

                    });

        }
    }
}