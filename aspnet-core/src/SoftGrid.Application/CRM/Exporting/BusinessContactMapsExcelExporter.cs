using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class BusinessContactMapsExcelExporter : NpoiExcelExporterBase, IBusinessContactMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BusinessContactMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBusinessContactMapForViewDto> businessContactMaps)
        {
            return CreateExcelPackage(
                "BusinessContactMaps.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("BusinessContactMaps"));

                    AddHeader(
                        sheet,
                        (L("Business")) + L("Name"),
                        (L("Contact")) + L("FullName")
                        );

                    AddObjects(
                        sheet, businessContactMaps,
                        _ => _.BusinessName,
                        _ => _.ContactFullName
                        );

                });
        }
    }
}