using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductReturnInfosExcelExporter : NpoiExcelExporterBase, IProductReturnInfosExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductReturnInfosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductReturnInfoForViewDto> productReturnInfos)
        {
            return CreateExcelPackage(
                "ProductReturnInfos.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductReturnInfos"));

                    AddHeader(
                        sheet,
                        L("CustomerNote"),
                        L("AdminNote"),
                        (L("Product")) + L("Name"),
                        (L("ReturnType")) + L("Name"),
                        (L("ReturnStatus")) + L("Name")
                        );

                    AddObjects(
                        sheet, productReturnInfos,
                        _ => _.ProductReturnInfo.CustomerNote,
                        _ => _.ProductReturnInfo.AdminNote,
                        _ => _.ProductName,
                        _ => _.ReturnTypeName,
                        _ => _.ReturnStatusName
                        );

                });
        }
    }
}