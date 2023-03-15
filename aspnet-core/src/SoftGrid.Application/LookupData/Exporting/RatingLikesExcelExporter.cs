using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class RatingLikesExcelExporter : NpoiExcelExporterBase, IRatingLikesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RatingLikesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRatingLikeForViewDto> ratingLikes)
        {
            return CreateExcelPackage(
                "RatingLikes.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("RatingLikes"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Score"),
                        L("IconLink")
                        );

                    AddObjects(
                        sheet, ratingLikes,
                        _ => _.RatingLike.Name,
                        _ => _.RatingLike.Score,
                        _ => _.RatingLike.IconLink
                        );

                });
        }
    }
}