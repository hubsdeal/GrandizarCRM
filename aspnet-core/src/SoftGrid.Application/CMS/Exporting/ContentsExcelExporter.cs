using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CMS.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CMS.Exporting
{
    public class ContentsExcelExporter : NpoiExcelExporterBase, IContentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ContentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetContentForViewDto> contents)
        {
            return CreateExcelPackage(
                    "Contents.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("Contents"));

                        AddHeader(
                            sheet,
                        L("Title"),
                        L("Body"),
                        L("Published"),
                        L("PublishedDate"),
                        L("PublishTime"),
                        L("SeoUrl"),
                        L("SeoKeywords"),
                        L("SeoDescription"),
                        L("ContentTypeId"),
                        (L("MediaLibrary")) + L("Name")
                            );

                        AddObjects(
                            sheet, contents,
                        _ => _.Content.Title,
                        _ => _.Content.Body,
                        _ => _.Content.Published,
                        _ => _timeZoneConverter.Convert(_.Content.PublishedDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Content.PublishTime,
                        _ => _.Content.SeoUrl,
                        _ => _.Content.SeoKeywords,
                        _ => _.Content.SeoDescription,
                        _ => _.Content.ContentTypeId,
                        _ => _.MediaLibraryName
                            );

                        for (var i = 1; i <= contents.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[4 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(4 - 1);
                    });

        }
    }
}