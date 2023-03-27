using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.SalesLeadManagement.Exporting
{
    public class LeadPipelineStatusesExcelExporter : NpoiExcelExporterBase, ILeadPipelineStatusesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LeadPipelineStatusesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLeadPipelineStatusForViewDto> leadPipelineStatuses)
        {
            return CreateExcelPackage(
                "LeadPipelineStatuses.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("LeadPipelineStatuses"));

                    AddHeader(
                        sheet,
                        L("EntryDate"),
                        L("ExitDate"),
                        L("EnteredAt"),
                        (L("Lead")) + L("Title"),
                        (L("LeadPipelineStage")) + L("Name"),
                        (L("Employee")) + L("Name")
                        );

                    AddObjects(
                        sheet, leadPipelineStatuses,
                        _ => _timeZoneConverter.Convert(_.LeadPipelineStatus.EntryDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.LeadPipelineStatus.ExitDate,
                        _ => _timeZoneConverter.Convert(_.LeadPipelineStatus.EnteredAt, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.LeadTitle,
                        _ => _.LeadPipelineStageName,
                        _ => _.EmployeeName
                        );

                    for (var i = 1; i <= leadPipelineStatuses.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[1], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(1); for (var i = 1; i <= leadPipelineStatuses.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3);
                });
        }
    }
}