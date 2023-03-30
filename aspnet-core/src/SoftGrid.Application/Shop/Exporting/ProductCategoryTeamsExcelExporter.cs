using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductCategoryTeamsExcelExporter : NpoiExcelExporterBase, IProductCategoryTeamsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductCategoryTeamsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductCategoryTeamForViewDto> productCategoryTeams)
        {
            return CreateExcelPackage(
                "ProductCategoryTeams.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductCategoryTeams"));

                    AddHeader(
                        sheet,
                        L("Primary"),
                        (L("ProductCategory")) + L("Name"),
                        (L("Employee")) + L("Name")
                        );

                    AddObjects(
                        sheet, productCategoryTeams,
                        _ => _.ProductCategoryTeam.Primary,
                        _ => _.ProductCategoryName,
                        _ => _.EmployeeName
                        );

                });
        }
    }
}