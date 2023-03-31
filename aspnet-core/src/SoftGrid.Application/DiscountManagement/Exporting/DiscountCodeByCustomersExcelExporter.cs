using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.DiscountManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.DiscountManagement.Exporting
{
    public class DiscountCodeByCustomersExcelExporter : NpoiExcelExporterBase, IDiscountCodeByCustomersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DiscountCodeByCustomersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDiscountCodeByCustomerForViewDto> discountCodeByCustomers)
        {
            return CreateExcelPackage(
                "DiscountCodeByCustomers.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("DiscountCodeByCustomers"));

                    AddHeader(
                        sheet,
                        (L("DiscountCodeGenerator")) + L("Name"),
                        (L("Contact")) + L("FullName")
                        );

                    AddObjects(
                        sheet, discountCodeByCustomers,
                        _ => _.DiscountCodeGeneratorName,
                        _ => _.ContactFullName
                        );

                });
        }
    }
}