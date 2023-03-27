using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreReviewFeedbacksExcelExporter : NpoiExcelExporterBase, IStoreReviewFeedbacksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreReviewFeedbacksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreReviewFeedbackForViewDto> storeReviewFeedbacks)
        {
            return CreateExcelPackage(
                "StoreReviewFeedbacks.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("StoreReviewFeedbacks"));

                    AddHeader(
                        sheet,
                        L("ReplyText"),
                        L("IsPublished"),
                        (L("StoreReview")) + L("ReviewInfo"),
                        (L("Contact")) + L("FullName"),
                        (L("RatingLike")) + L("Name")
                        );

                    AddObjects(
                        sheet, storeReviewFeedbacks,
                        _ => _.StoreReviewFeedback.ReplyText,
                        _ => _.StoreReviewFeedback.IsPublished,
                        _ => _.StoreReviewReviewInfo,
                        _ => _.ContactFullName,
                        _ => _.RatingLikeName
                        );

                });
        }
    }
}