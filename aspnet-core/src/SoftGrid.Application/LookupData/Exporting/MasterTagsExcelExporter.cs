using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class MasterTagsExcelExporter : NpoiExcelExporterBase, IMasterTagsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MasterTagsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMasterTagForViewDto> masterTags)
        {
            return CreateExcelPackage(
                "MasterTags.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("MasterTags"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("Synonyms"),
                        L("PictureId"),
                        L("DisplaySequence"),
                        (L("MasterTagCategory")) + L("Name")
                        );

                    AddObjects(
                        sheet, masterTags,
                        _ => _.MasterTag.Name,
                        _ => _.MasterTag.Description,
                        _ => _.MasterTag.Synonyms,
                        _ => _.MasterTag.PictureId,
                        _ => _.MasterTag.DisplaySequence,
                        _ => _.MasterTagCategoryName
                        );

                });
        }
    }
}