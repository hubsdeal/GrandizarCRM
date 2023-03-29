using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class SocialMediasExcelExporter : NpoiExcelExporterBase, ISocialMediasExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SocialMediasExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSocialMediaForViewDto> socialMedias)
        {
            return CreateExcelPackage(
                "SocialMedias.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("SocialMedias"));

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, socialMedias,
                        _ => _.SocialMedia.Name
                        );

                });
        }
    }
}