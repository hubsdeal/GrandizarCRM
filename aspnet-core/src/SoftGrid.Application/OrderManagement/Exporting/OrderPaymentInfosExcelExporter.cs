using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.OrderManagement.Exporting
{
    public class OrderPaymentInfosExcelExporter : NpoiExcelExporterBase, IOrderPaymentInfosExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrderPaymentInfosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrderPaymentInfoForViewDto> orderPaymentInfos)
        {
            return CreateExcelPackage(
                "OrderPaymentInfos.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("OrderPaymentInfos"));

                    AddHeader(
                        sheet,
                        L("PaymentSplit"),
                        L("DueAmount"),
                        L("PaySplitAmount"),
                        L("BillingAddress"),
                        L("BillingCity"),
                        L("BillingState"),
                        L("BillingZipCode"),
                        L("SaveCreditCardNumber"),
                        L("MaskedCreditCardNumber"),
                        L("CardName"),
                        L("CardNumber"),
                        L("CardCvv"),
                        L("CardExpirationMonth"),
                        L("CardExpirationYear"),
                        L("AuthorizationTransactionNumber"),
                        L("AuthorizationTransactionCode"),
                        L("AuthrorizationTransactionResult"),
                        L("CustomerIpAddress"),
                        L("CustomerDeviceInfo"),
                        L("PaidDate"),
                        L("PaidTime"),
                        (L("Order")) + L("InvoiceNumber"),
                        (L("Currency")) + L("Name"),
                        (L("PaymentType")) + L("Name")
                        );

                    AddObjects(
                        sheet, orderPaymentInfos,
                        _ => _.OrderPaymentInfo.PaymentSplit,
                        _ => _.OrderPaymentInfo.DueAmount,
                        _ => _.OrderPaymentInfo.PaySplitAmount,
                        _ => _.OrderPaymentInfo.BillingAddress,
                        _ => _.OrderPaymentInfo.BillingCity,
                        _ => _.OrderPaymentInfo.BillingState,
                        _ => _.OrderPaymentInfo.BillingZipCode,
                        _ => _.OrderPaymentInfo.SaveCreditCardNumber,
                        _ => _.OrderPaymentInfo.MaskedCreditCardNumber,
                        _ => _.OrderPaymentInfo.CardName,
                        _ => _.OrderPaymentInfo.CardNumber,
                        _ => _.OrderPaymentInfo.CardCvv,
                        _ => _.OrderPaymentInfo.CardExpirationMonth,
                        _ => _.OrderPaymentInfo.CardExpirationYear,
                        _ => _.OrderPaymentInfo.AuthorizationTransactionNumber,
                        _ => _.OrderPaymentInfo.AuthorizationTransactionCode,
                        _ => _.OrderPaymentInfo.AuthrorizationTransactionResult,
                        _ => _.OrderPaymentInfo.CustomerIpAddress,
                        _ => _.OrderPaymentInfo.CustomerDeviceInfo,
                        _ => _timeZoneConverter.Convert(_.OrderPaymentInfo.PaidDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OrderPaymentInfo.PaidTime,
                        _ => _.OrderInvoiceNumber,
                        _ => _.CurrencyName,
                        _ => _.PaymentTypeName
                        );

                    for (var i = 1; i <= orderPaymentInfos.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[20], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(20);
                });
        }
    }
}