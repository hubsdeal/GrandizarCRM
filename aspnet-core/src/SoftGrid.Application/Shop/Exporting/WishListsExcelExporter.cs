using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class WishListsExcelExporter : NpoiExcelExporterBase, IWishListsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public WishListsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetWishListForViewDto> wishLists)
        {
            return CreateExcelPackage(
                    "WishLists.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("WishLists"));

                        AddHeader(
                            sheet,
                        L("Date"),
                        (L("Contact")) + L("FullName"),
                        (L("Product")) + L("Name"),
                        (L("Store")) + L("Name")
                            );

                        AddObjects(
                            sheet, wishLists,
                        _ => _timeZoneConverter.Convert(_.WishList.Date, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ContactFullName,
                        _ => _.ProductName,
                        _ => _.StoreName
                            );

                        for (var i = 1; i <= wishLists.Count; i++)
                        {
                            SetCellDataFormat(sheet.GetRow(i).Cells[1 - 1], "yyyy-mm-dd");
                        }
                        sheet.AutoSizeColumn(1 - 1);
                    });

        }
    }
}