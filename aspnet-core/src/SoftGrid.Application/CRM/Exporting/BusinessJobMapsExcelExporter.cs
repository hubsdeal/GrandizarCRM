using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class BusinessJobMapsExcelExporter : NpoiExcelExporterBase, IBusinessJobMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BusinessJobMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBusinessJobMapForViewDto> businessJobMaps)
        {
            return CreateExcelPackage(
                "BusinessJobMaps.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("BusinessJobMaps"));

                    AddHeader(
                        sheet,
                        (L("Business")) + L("Name"),
                        (L("Job")) + L("Title")
                        );

                    AddObjects(
                        sheet, businessJobMaps,
                        _ => _.BusinessName,
                        _ => _.JobTitle
                        );

                });
        }
    }
}