using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreTaxesExcelExporter : NpoiExcelExporterBase, IStoreTaxesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreTaxesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreTaxForViewDto> storeTaxes)
        {
            return CreateExcelPackage(
                "StoreTaxes.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("StoreTaxes"));

                    AddHeader(
                        sheet,
                        L("TaxName"),
                        L("PercentageOrAmount"),
                        L("TaxRatePercentage"),
                        L("TaxAmount"),
                        (L("Store")) + L("Name")
                        );

                    AddObjects(
                        sheet, storeTaxes,
                        _ => _.StoreTax.TaxName,
                        _ => _.StoreTax.PercentageOrAmount,
                        _ => _.StoreTax.TaxRatePercentage,
                        _ => _.StoreTax.TaxAmount,
                        _ => _.StoreName
                        );

                });
        }
    }
}