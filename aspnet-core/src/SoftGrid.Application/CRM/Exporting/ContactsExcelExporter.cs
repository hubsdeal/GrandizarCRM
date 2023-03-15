using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class ContactsExcelExporter : NpoiExcelExporterBase, IContactsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ContactsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetContactForViewDto> contacts)
        {
            return CreateExcelPackage(
                "Contacts.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Contacts"));

                    AddHeader(
                        sheet,
                        L("FullName"),
                        L("FirstName"),
                        L("LastName"),
                        L("FullAddress"),
                        L("Address"),
                        L("ZipCode"),
                        L("City"),
                        L("DateOfBirth"),
                        L("Mobile"),
                        L("OfficePhone"),
                        L("CountryCode"),
                        L("PersonalEmail"),
                        L("BusinessEmail"),
                        L("JobTitle"),
                        L("CompanyName"),
                        L("AiDataTag"),
                        L("Facebook"),
                        L("LinkedIn"),
                        L("Referred"),
                        L("Verified"),
                        L("Score"),
                        (L("User")) + L("Name"),
                        (L("Country")) + L("Name"),
                        (L("State")) + L("Name"),
                        (L("MembershipType")) + L("Name")
                        );

                    AddObjects(
                        sheet, contacts,
                        _ => _.Contact.FullName,
                        _ => _.Contact.FirstName,
                        _ => _.Contact.LastName,
                        _ => _.Contact.FullAddress,
                        _ => _.Contact.Address,
                        _ => _.Contact.ZipCode,
                        _ => _.Contact.City,
                        _ => _timeZoneConverter.Convert(_.Contact.DateOfBirth, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Contact.Mobile,
                        _ => _.Contact.OfficePhone,
                        _ => _.Contact.CountryCode,
                        _ => _.Contact.PersonalEmail,
                        _ => _.Contact.BusinessEmail,
                        _ => _.Contact.JobTitle,
                        _ => _.Contact.CompanyName,
                        _ => _.Contact.AiDataTag,
                        _ => _.Contact.Facebook,
                        _ => _.Contact.LinkedIn,
                        _ => _.Contact.Referred,
                        _ => _.Contact.Verified,
                        _ => _.Contact.Score,
                        _ => _.UserName,
                        _ => _.CountryName,
                        _ => _.StateName,
                        _ => _.MembershipTypeName
                        );

                    for (var i = 1; i <= contacts.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[8], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(8);
                });
        }
    }
}