using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.SalesLeadManagement.Exporting
{
    public class LeadsExcelExporter : NpoiExcelExporterBase, ILeadsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LeadsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLeadForViewDto> leads)
        {
            return CreateExcelPackage(
                "Leads.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Leads"));

                    AddHeader(
                        sheet,
                        L("Title"),
                        L("FirstName"),
                        L("LastName"),
                        L("Email"),
                        L("Phone"),
                        L("Company"),
                        L("JobTitle"),
                        L("Industry"),
                        L("LeadScore"),
                        L("ExpectedSalesAmount"),
                        L("CreatedDate"),
                        L("ExpectedClosingDate"),
                        (L("Contact")) + L("FullName"),
                        (L("Business")) + L("Name"),
                        (L("Product")) + L("Name"),
                        (L("ProductCategory")) + L("Name"),
                        (L("Store")) + L("Name"),
                        (L("Employee")) + L("Name"),
                        (L("LeadSource")) + L("Name"),
                        (L("LeadPipelineStage")) + L("Name"),
                        (L("Hub")) + L("Name")
                        );

                    AddObjects(
                        sheet, leads,
                        _ => _.Lead.Title,
                        _ => _.Lead.FirstName,
                        _ => _.Lead.LastName,
                        _ => _.Lead.Email,
                        _ => _.Lead.Phone,
                        _ => _.Lead.Company,
                        _ => _.Lead.JobTitle,
                        _ => _.Lead.Industry,
                        _ => _.Lead.LeadScore,
                        _ => _.Lead.ExpectedSalesAmount,
                        _ => _timeZoneConverter.Convert(_.Lead.CreatedDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Lead.ExpectedClosingDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ContactFullName,
                        _ => _.BusinessName,
                        _ => _.ProductName,
                        _ => _.ProductCategoryName,
                        _ => _.StoreName,
                        _ => _.EmployeeName,
                        _ => _.LeadSourceName,
                        _ => _.LeadPipelineStageName,
                        _ => _.HubName
                        );

                    for (var i = 1; i <= leads.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[11], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(11); for (var i = 1; i <= leads.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[12], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(12);
                });
        }
    }
}