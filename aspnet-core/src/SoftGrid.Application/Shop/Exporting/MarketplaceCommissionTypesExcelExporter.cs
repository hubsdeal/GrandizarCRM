using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class MarketplaceCommissionTypesExcelExporter : NpoiExcelExporterBase, IMarketplaceCommissionTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MarketplaceCommissionTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMarketplaceCommissionTypeForViewDto> marketplaceCommissionTypes)
        {
            return CreateExcelPackage(
                "MarketplaceCommissionTypes.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("MarketplaceCommissionTypes"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Percentage"),
                        L("FixedAmount")
                        );

                    AddObjects(
                        sheet, marketplaceCommissionTypes,
                        _ => _.MarketplaceCommissionType.Name,
                        _ => _.MarketplaceCommissionType.Percentage,
                        _ => _.MarketplaceCommissionType.FixedAmount
                        );

                });
        }
    }
}