using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.SalesLeadManagement.Exporting
{
    public class LeadSourcesExcelExporter : NpoiExcelExporterBase, ILeadSourcesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LeadSourcesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLeadSourceForViewDto> leadSources)
        {
            return CreateExcelPackage(
                "LeadSources.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("LeadSources"));

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, leadSources,
                        _ => _.LeadSource.Name
                        );

                });
        }
    }
}