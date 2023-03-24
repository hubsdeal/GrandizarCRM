using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class EmployeesExcelExporter : NpoiExcelExporterBase, IEmployeesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeForViewDto> employees)
        {
            return CreateExcelPackage(
                "Employees.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Employees"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("FirstName"),
                        L("LastName"),
                        L("FullAddress"),
                        L("Address"),
                        L("ZipCode"),
                        L("City"),
                        L("DateOfBirth"),
                        L("Mobile"),
                        L("OfficePhone"),
                        L("PersonalEmail"),
                        L("BusinessEmail"),
                        L("JobTitle"),
                        L("CompanyName"),
                        L("Profile"),
                        L("HireDate"),
                        L("Facebook"),
                        L("LinkedIn"),
                        L("Fax"),
                        L("ProfilePictureId"),
                        L("CurrentEmployee"),
                        (L("State")) + L("Name"),
                        (L("Country")) + L("Name"),
                        (L("Contact")) + L("FullName")
                        );

                    AddObjects(
                        sheet, employees,
                        _ => _.Employee.Name,
                        _ => _.Employee.FirstName,
                        _ => _.Employee.LastName,
                        _ => _.Employee.FullAddress,
                        _ => _.Employee.Address,
                        _ => _.Employee.ZipCode,
                        _ => _.Employee.City,
                        _ => _timeZoneConverter.Convert(_.Employee.DateOfBirth, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Employee.Mobile,
                        _ => _.Employee.OfficePhone,
                        _ => _.Employee.PersonalEmail,
                        _ => _.Employee.BusinessEmail,
                        _ => _.Employee.JobTitle,
                        _ => _.Employee.CompanyName,
                        _ => _.Employee.Profile,
                        _ => _timeZoneConverter.Convert(_.Employee.HireDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Employee.Facebook,
                        _ => _.Employee.LinkedIn,
                        _ => _.Employee.Fax,
                        _ => _.Employee.ProfilePictureId,
                        _ => _.Employee.CurrentEmployee,
                        _ => _.StateName,
                        _ => _.CountryName,
                        _ => _.ContactFullName
                        );

                    for (var i = 1; i <= employees.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[8], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(8); for (var i = 1; i <= employees.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[16], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(16);
                });
        }
    }
}