using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductReviewsExcelExporter : NpoiExcelExporterBase, IProductReviewsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductReviewsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductReviewForViewDto> productReviews)
        {
            return CreateExcelPackage(
                "ProductReviews.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductReviews"));

                    AddHeader(
                        sheet,
                        L("ReviewInfo"),
                        L("PostDate"),
                        L("Publish"),
                        L("PostTime"),
                        (L("Contact")) + L("FullName"),
                        (L("Product")) + L("Name"),
                        (L("Store")) + L("Name"),
                        (L("RatingLike")) + L("Name")
                        );

                    AddObjects(
                        sheet, productReviews,
                        _ => _.ProductReview.ReviewInfo,
                        _ => _timeZoneConverter.Convert(_.ProductReview.PostDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ProductReview.Publish,
                        _ => _.ProductReview.PostTime,
                        _ => _.ContactFullName,
                        _ => _.ProductName,
                        _ => _.StoreName,
                        _ => _.RatingLikeName
                        );

                    for (var i = 1; i <= productReviews.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(2);
                });
        }
    }
}