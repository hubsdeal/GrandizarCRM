using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreTagsExcelExporter : NpoiExcelExporterBase, IStoreTagsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreTagsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreTagForViewDto> storeTags)
        {
            return CreateExcelPackage(
                "StoreTags.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("StoreTags"));

                    AddHeader(
                        sheet,
                        L("CustomTag"),
                        L("TagValue"),
                        L("Verified"),
                        L("Sequence"),
                        (L("Store")) + L("Name"),
                        (L("MasterTagCategory")) + L("Name"),
                        (L("MasterTag")) + L("Name")
                        );

                    AddObjects(
                        sheet, storeTags,
                        _ => _.StoreTag.CustomTag,
                        _ => _.StoreTag.TagValue,
                        _ => _.StoreTag.Verified,
                        _ => _.StoreTag.Sequence,
                        _ => _.StoreName,
                        _ => _.MasterTagCategoryName,
                        _ => _.MasterTagName
                        );

                });
        }
    }
}