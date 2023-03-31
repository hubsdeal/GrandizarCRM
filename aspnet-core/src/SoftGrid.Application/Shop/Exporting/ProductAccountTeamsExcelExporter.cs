using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductAccountTeamsExcelExporter : NpoiExcelExporterBase, IProductAccountTeamsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductAccountTeamsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductAccountTeamForViewDto> productAccountTeams)
        {
            return CreateExcelPackage(
                "ProductAccountTeams.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductAccountTeams"));

                    AddHeader(
                        sheet,
                        L("Primary"),
                        L("Active"),
                        L("RemoveDate"),
                        L("AssignDate"),
                        (L("Employee")) + L("Name"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, productAccountTeams,
                        _ => _.ProductAccountTeam.Primary,
                        _ => _.ProductAccountTeam.Active,
                        _ => _timeZoneConverter.Convert(_.ProductAccountTeam.RemoveDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.ProductAccountTeam.AssignDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.EmployeeName,
                        _ => _.ProductName
                        );

                    for (var i = 1; i <= productAccountTeams.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3); for (var i = 1; i <= productAccountTeams.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(4);
                });
        }
    }
}