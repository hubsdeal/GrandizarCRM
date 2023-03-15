using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class MediaLibrariesExcelExporter : NpoiExcelExporterBase, IMediaLibrariesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MediaLibrariesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMediaLibraryForViewDto> mediaLibraries)
        {
            return CreateExcelPackage(
                "MediaLibraries.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("MediaLibraries"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Size"),
                        L("FileExtension"),
                        L("Dimension"),
                        L("VideoLink"),
                        L("SeoTag"),
                        L("AltTag"),
                        L("VirtualPath"),
                        L("BinaryObjectId"),
                        (L("MasterTagCategory")) + L("Name"),
                        (L("MasterTag")) + L("Name")
                        );

                    AddObjects(
                        sheet, mediaLibraries,
                        _ => _.MediaLibrary.Name,
                        _ => _.MediaLibrary.Size,
                        _ => _.MediaLibrary.FileExtension,
                        _ => _.MediaLibrary.Dimension,
                        _ => _.MediaLibrary.VideoLink,
                        _ => _.MediaLibrary.SeoTag,
                        _ => _.MediaLibrary.AltTag,
                        _ => _.MediaLibrary.VirtualPath,
                        _ => _.MediaLibrary.BinaryObjectId,
                        _ => _.MasterTagCategoryName,
                        _ => _.MasterTagName
                        );

                });
        }
    }
}