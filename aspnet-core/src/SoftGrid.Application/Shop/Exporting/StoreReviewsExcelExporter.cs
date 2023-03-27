using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreReviewsExcelExporter : NpoiExcelExporterBase, IStoreReviewsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreReviewsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreReviewForViewDto> storeReviews)
        {
            return CreateExcelPackage(
                "StoreReviews.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("StoreReviews"));

                    AddHeader(
                        sheet,
                        L("ReviewInfo"),
                        L("PostDate"),
                        L("PostTime"),
                        L("IsPublish"),
                        (L("Store")) + L("Name"),
                        (L("Contact")) + L("FullName"),
                        (L("RatingLike")) + L("Name")
                        );

                    AddObjects(
                        sheet, storeReviews,
                        _ => _.StoreReview.ReviewInfo,
                        _ => _timeZoneConverter.Convert(_.StoreReview.PostDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.StoreReview.PostTime,
                        _ => _.StoreReview.IsPublish,
                        _ => _.StoreName,
                        _ => _.ContactFullName,
                        _ => _.RatingLikeName
                        );

                    for (var i = 1; i <= storeReviews.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(2);
                });
        }
    }
}