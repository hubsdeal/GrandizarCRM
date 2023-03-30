using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductFaqsExcelExporter : NpoiExcelExporterBase, IProductFaqsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductFaqsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductFaqForViewDto> productFaqs)
        {
            return CreateExcelPackage(
                "ProductFaqs.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductFaqs"));

                    AddHeader(
                        sheet,
                        L("Question"),
                        L("Answer"),
                        L("Template"),
                        L("Publish"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, productFaqs,
                        _ => _.ProductFaq.Question,
                        _ => _.ProductFaq.Answer,
                        _ => _.ProductFaq.Template,
                        _ => _.ProductFaq.Publish,
                        _ => _.ProductName
                        );

                });
        }
    }
}