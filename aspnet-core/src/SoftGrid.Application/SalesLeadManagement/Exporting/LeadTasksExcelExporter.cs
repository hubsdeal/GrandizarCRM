using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.SalesLeadManagement.Exporting
{
    public class LeadTasksExcelExporter : NpoiExcelExporterBase, ILeadTasksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LeadTasksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLeadTaskForViewDto> leadTasks)
        {
            return CreateExcelPackage(
                "LeadTasks.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("LeadTasks"));

                    AddHeader(
                        sheet,
                        (L("Lead")) + L("Title"),
                        (L("TaskEvent")) + L("Name")
                        );

                    AddObjects(
                        sheet, leadTasks,
                        _ => _.LeadTitle,
                        _ => _.TaskEventName
                        );

                });
        }
    }
}