using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.DiscountManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.DiscountManagement.Exporting
{
    public class DiscountCodeMapsExcelExporter : NpoiExcelExporterBase, IDiscountCodeMapsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DiscountCodeMapsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDiscountCodeMapForViewDto> discountCodeMaps)
        {
            return CreateExcelPackage(
                "DiscountCodeMaps.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("DiscountCodeMaps"));

                    AddHeader(
                        sheet,
                        (L("DiscountCodeGenerator")) + L("Name"),
                        (L("Store")) + L("Name"),
                        (L("Product")) + L("Name"),
                        (L("MembershipType")) + L("Name")
                        );

                    AddObjects(
                        sheet, discountCodeMaps,
                        _ => _.DiscountCodeGeneratorName,
                        _ => _.StoreName,
                        _ => _.ProductName,
                        _ => _.MembershipTypeName
                        );

                });
        }
    }
}