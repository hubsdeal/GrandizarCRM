using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreBankAccountsExcelExporter : NpoiExcelExporterBase, IStoreBankAccountsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreBankAccountsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreBankAccountForViewDto> storeBankAccounts)
        {
            return CreateExcelPackage(
                "StoreBankAccounts.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("StoreBankAccounts"));

                    AddHeader(
                        sheet,
                        L("AccountName"),
                        L("AccountNo"),
                        L("BankName"),
                        L("RoutingNo"),
                        L("BankAddress"),
                        (L("Store")) + L("Name")
                        );

                    AddObjects(
                        sheet, storeBankAccounts,
                        _ => _.StoreBankAccount.AccountName,
                        _ => _.StoreBankAccount.AccountNo,
                        _ => _.StoreBankAccount.BankName,
                        _ => _.StoreBankAccount.RoutingNo,
                        _ => _.StoreBankAccount.BankAddress,
                        _ => _.StoreName
                        );

                });
        }
    }
}