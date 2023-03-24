using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class BusinessStoreMapsExcelExporter : NpoiExcelExporterBase, IBusinessStoreMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BusinessStoreMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBusinessStoreMapForViewDto> businessStoreMaps)
        {
            return CreateExcelPackage(
                "BusinessStoreMaps.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("BusinessStoreMaps"));

                    AddHeader(
                        sheet,
                        (L("Business")) + L("Name"),
                        (L("Store")) + L("Name")
                        );

                    AddObjects(
                        sheet, businessStoreMaps,
                        _ => _.BusinessName,
                        _ => _.StoreName
                        );

                });
        }
    }
}