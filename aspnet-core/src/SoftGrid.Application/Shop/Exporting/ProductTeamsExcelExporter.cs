using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductTeamsExcelExporter : NpoiExcelExporterBase, IProductTeamsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductTeamsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductTeamForViewDto> productTeams)
        {
            return CreateExcelPackage(
                    "ProductTeams.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("ProductTeams"));

                        AddHeader(
                            sheet,
                        L("Primary"),
                        L("Active"),
                        (L("Employee")) + L("Name"),
                        (L("Product")) + L("Name")
                            );

                        AddObjects(
                            sheet, productTeams,
                        _ => _.ProductTeam.Primary,
                        _ => _.ProductTeam.Active,
                        _ => _.EmployeeName,
                        _ => _.ProductName
                            );

                    });

        }
    }
}