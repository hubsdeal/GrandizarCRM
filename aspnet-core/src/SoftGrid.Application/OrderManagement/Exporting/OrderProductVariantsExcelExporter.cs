using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.OrderManagement.Exporting
{
    public class OrderProductVariantsExcelExporter : NpoiExcelExporterBase, IOrderProductVariantsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrderProductVariantsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrderProductVariantForViewDto> orderProductVariants)
        {
            return CreateExcelPackage(
                "OrderProductVariants.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("OrderProductVariants"));

                    AddHeader(
                        sheet,
                        L("Price"),
                        L("OrderProductInfoId"),
                        (L("ProductVariantCategory")) + L("Name"),
                        (L("ProductVariant")) + L("Name")
                        );

                    AddObjects(
                        sheet, orderProductVariants,
                        _ => _.OrderProductVariant.Price,
                        _ => _.OrderProductVariant.OrderProductInfoId,
                        _ => _.ProductVariantCategoryName,
                        _ => _.ProductVariantName
                        );

                });
        }
    }
}