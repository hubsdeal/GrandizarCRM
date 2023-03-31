using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.DiscountManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.DiscountManagement.Exporting
{
    public class CustomerWalletsExcelExporter : NpoiExcelExporterBase, ICustomerWalletsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CustomerWalletsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCustomerWalletForViewDto> customerWallets)
        {
            return CreateExcelPackage(
                "CustomerWallets.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("CustomerWallets"));

                    AddHeader(
                        sheet,
                        L("WalletOpeningDate"),
                        L("BalanceDate"),
                        L("BalanceAmount"),
                        (L("Contact")) + L("FullName"),
                        (L("User")) + L("Name"),
                        (L("Currency")) + L("Name")
                        );

                    AddObjects(
                        sheet, customerWallets,
                        _ => _timeZoneConverter.Convert(_.CustomerWallet.WalletOpeningDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.CustomerWallet.BalanceDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.CustomerWallet.BalanceAmount,
                        _ => _.ContactFullName,
                        _ => _.UserName,
                        _ => _.CurrencyName
                        );

                    for (var i = 1; i <= customerWallets.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[1], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(1); for (var i = 1; i <= customerWallets.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(2);
                });
        }
    }
}