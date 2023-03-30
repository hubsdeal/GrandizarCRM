using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductReviewFeedbacksExcelExporter : NpoiExcelExporterBase, IProductReviewFeedbacksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductReviewFeedbacksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductReviewFeedbackForViewDto> productReviewFeedbacks)
        {
            return CreateExcelPackage(
                "ProductReviewFeedbacks.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductReviewFeedbacks"));

                    AddHeader(
                        sheet,
                        L("ReplyText"),
                        L("Published"),
                        (L("Contact")) + L("FullName"),
                        (L("ProductReview")) + L("ReviewInfo"),
                        (L("RatingLike")) + L("Name")
                        );

                    AddObjects(
                        sheet, productReviewFeedbacks,
                        _ => _.ProductReviewFeedback.ReplyText,
                        _ => _.ProductReviewFeedback.Published,
                        _ => _.ContactFullName,
                        _ => _.ProductReviewReviewInfo,
                        _ => _.RatingLikeName
                        );

                });
        }
    }
}