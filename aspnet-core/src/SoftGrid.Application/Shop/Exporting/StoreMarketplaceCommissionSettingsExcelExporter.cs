using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreMarketplaceCommissionSettingsExcelExporter : NpoiExcelExporterBase, IStoreMarketplaceCommissionSettingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreMarketplaceCommissionSettingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreMarketplaceCommissionSettingForViewDto> storeMarketplaceCommissionSettings)
        {
            return CreateExcelPackage(
                "StoreMarketplaceCommissionSettings.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("StoreMarketplaceCommissionSettings"));

                    AddHeader(
                        sheet,
                        L("Percentage"),
                        L("FixedAmount"),
                        L("StartDate"),
                        L("EndDate"),
                        (L("Store")) + L("Name"),
                        (L("MarketplaceCommissionType")) + L("Name"),
                        (L("ProductCategory")) + L("Name"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, storeMarketplaceCommissionSettings,
                        _ => _.StoreMarketplaceCommissionSetting.Percentage,
                        _ => _.StoreMarketplaceCommissionSetting.FixedAmount,
                        _ => _timeZoneConverter.Convert(_.StoreMarketplaceCommissionSetting.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.StoreMarketplaceCommissionSetting.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.StoreName,
                        _ => _.MarketplaceCommissionTypeName,
                        _ => _.ProductCategoryName,
                        _ => _.ProductName
                        );

                    for (var i = 1; i <= storeMarketplaceCommissionSettings.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3); for (var i = 1; i <= storeMarketplaceCommissionSettings.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(4);
                });
        }
    }
}