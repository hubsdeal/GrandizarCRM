using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.OrderManagement.Exporting
{
    public class OrderSalesChannelsExcelExporter : NpoiExcelExporterBase, IOrderSalesChannelsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrderSalesChannelsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrderSalesChannelForViewDto> orderSalesChannels)
        {
            return CreateExcelPackage(
                "OrderSalesChannels.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("OrderSalesChannels"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("LinkName"),
                        L("ApiLink"),
                        L("UserId"),
                        L("Password"),
                        L("Notes")
                        );

                    AddObjects(
                        sheet, orderSalesChannels,
                        _ => _.OrderSalesChannel.Name,
                        _ => _.OrderSalesChannel.LinkName,
                        _ => _.OrderSalesChannel.ApiLink,
                        _ => _.OrderSalesChannel.UserId,
                        _ => _.OrderSalesChannel.Password,
                        _ => _.OrderSalesChannel.Notes
                        );

                });
        }
    }
}