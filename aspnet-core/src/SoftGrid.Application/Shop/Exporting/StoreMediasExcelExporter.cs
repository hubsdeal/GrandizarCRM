using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreMediasExcelExporter : NpoiExcelExporterBase, IStoreMediasExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreMediasExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreMediaForViewDto> storeMedias)
        {
            return CreateExcelPackage(
                "StoreMedias.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("StoreMedias"));

                    AddHeader(
                        sheet,
                        L("DisplaySequence"),
                        (L("Store")) + L("Name"),
                        (L("MediaLibrary")) + L("Name")
                        );

                    AddObjects(
                        sheet, storeMedias,
                        _ => _.StoreMedia.DisplaySequence,
                        _ => _.StoreName,
                        _ => _.MediaLibraryName
                        );

                });
        }
    }
}