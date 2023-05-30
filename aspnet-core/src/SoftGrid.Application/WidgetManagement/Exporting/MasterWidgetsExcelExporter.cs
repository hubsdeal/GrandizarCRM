using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.WidgetManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.WidgetManagement.Exporting
{
    public class MasterWidgetsExcelExporter : NpoiExcelExporterBase, IMasterWidgetsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MasterWidgetsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMasterWidgetForViewDto> masterWidgets)
        {
            return CreateExcelPackage(
                    "MasterWidgets.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("MasterWidgets"));

                        AddHeader(
                            sheet,
                        L("Name"),
                        L("Description"),
                        L("DesignCode"),
                        L("Publish"),
                        L("InternalDisplayNumber"),
                        L("ThumbnailImageId")
                            );

                        AddObjects(
                            sheet, masterWidgets,
                        _ => _.MasterWidget.Name,
                        _ => _.MasterWidget.Description,
                        _ => _.MasterWidget.DesignCode,
                        _ => _.MasterWidget.Publish,
                        _ => _.MasterWidget.InternalDisplayNumber,
                        _ => _.MasterWidget.ThumbnailImageId
                            );

                    });

        }
    }
}