using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.SalesLeadManagement.Exporting
{
    public class LeadPipelineStagesExcelExporter : NpoiExcelExporterBase, ILeadPipelineStagesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LeadPipelineStagesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLeadPipelineStageForViewDto> leadPipelineStages)
        {
            return CreateExcelPackage(
                "LeadPipelineStages.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("LeadPipelineStages"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("StageOrder")
                        );

                    AddObjects(
                        sheet, leadPipelineStages,
                        _ => _.LeadPipelineStage.Name,
                        _ => _.LeadPipelineStage.StageOrder
                        );

                });
        }
    }
}