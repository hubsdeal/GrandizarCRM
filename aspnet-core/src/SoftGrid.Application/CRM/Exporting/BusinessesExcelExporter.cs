using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class BusinessesExcelExporter : NpoiExcelExporterBase, IBusinessesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BusinessesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBusinessForViewDto> businesses)
        {
            return CreateExcelPackage(
                "Businesses.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Businesses"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("TradeName"),
                        L("Description"),
                        L("CustomId"),
                        L("YearOfEstablishment"),
                        L("LocationTitle"),
                        L("FullAddress"),
                        L("Address1"),
                        L("Address2"),
                        L("City"),
                        L("ZipCode"),
                        L("Latitude"),
                        L("Longitude"),
                        L("Phone"),
                        L("Fax"),
                        L("Email"),
                        L("Website"),
                        L("EinTaxId"),
                        L("Industry"),
                        L("InternalRemarks"),
                        L("Verified"),
                        L("Facebook"),
                        L("LinkedIn"),
                        (L("Country")) + L("Name"),
                        (L("State")) + L("Name"),
                        (L("City")) + L("Name"),
                        (L("MediaLibrary")) + L("Name")
                        );

                    AddObjects(
                        sheet, businesses,
                        _ => _.Business.Name,
                        _ => _.Business.TradeName,
                        _ => _.Business.Description,
                        _ => _.Business.CustomId,
                        _ => _.Business.YearOfEstablishment,
                        _ => _.Business.LocationTitle,
                        _ => _.Business.FullAddress,
                        _ => _.Business.Address1,
                        _ => _.Business.Address2,
                        _ => _.Business.City,
                        _ => _.Business.ZipCode,
                        _ => _.Business.Latitude,
                        _ => _.Business.Longitude,
                        _ => _.Business.Phone,
                        _ => _.Business.Fax,
                        _ => _.Business.Email,
                        _ => _.Business.Website,
                        _ => _.Business.EinTaxId,
                        _ => _.Business.Industry,
                        _ => _.Business.InternalRemarks,
                        _ => _.Business.Verified,
                        _ => _.Business.Facebook,
                        _ => _.Business.LinkedIn,
                        _ => _.CountryName,
                        _ => _.StateName,
                        _ => _.CityName,
                        _ => _.MediaLibraryName
                        );

                });
        }
    }
}