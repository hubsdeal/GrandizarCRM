using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class BusinessProductMapsExcelExporter : NpoiExcelExporterBase, IBusinessProductMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BusinessProductMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBusinessProductMapForViewDto> businessProductMaps)
        {
            return CreateExcelPackage(
                "BusinessProductMaps.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("BusinessProductMaps"));

                    AddHeader(
                        sheet,
                        (L("Business")) + L("Name"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, businessProductMaps,
                        _ => _.BusinessName,
                        _ => _.ProductName
                        );

                });
        }
    }
}