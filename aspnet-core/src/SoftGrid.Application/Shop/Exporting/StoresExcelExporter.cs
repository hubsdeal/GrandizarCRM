using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoresExcelExporter : NpoiExcelExporterBase, IStoresExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoresExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreForViewDto> stores)
        {
            return CreateExcelPackage(
                "Stores.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Stores"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("StoreUrl"),
                        L("Description"),
                        L("MetaTag"),
                        L("MetaDescription"),
                        L("FullAddress"),
                        L("Address"),
                        L("City"),
                        L("Latitude"),
                        L("Longitude"),
                        L("Phone"),
                        L("Mobile"),
                        L("Email"),
                        L("IsPublished"),
                        L("Facebook"),
                        L("Instagram"),
                        L("LinkedIn"),
                        L("Youtube"),
                        L("Fax"),
                        L("ZipCode"),
                        L("Website"),
                        L("YearOfEstablishment"),
                        L("DisplaySequence"),
                        L("Score"),
                        L("LegalName"),
                        L("IsLocalOrOnlineStore"),
                        L("IsVerified"),
                        (L("MediaLibrary")) + L("Name"),
                        (L("Country")) + L("Name"),
                        (L("State")) + L("Name"),
                        (L("RatingLike")) + L("Name"),
                        (L("MasterTag")) + L("Name")
                        );

                    AddObjects(
                        sheet, stores,
                        _ => _.Store.Name,
                        _ => _.Store.StoreUrl,
                        _ => _.Store.Description,
                        _ => _.Store.MetaTag,
                        _ => _.Store.MetaDescription,
                        _ => _.Store.FullAddress,
                        _ => _.Store.Address,
                        _ => _.Store.City,
                        _ => _.Store.Latitude,
                        _ => _.Store.Longitude,
                        _ => _.Store.Phone,
                        _ => _.Store.Mobile,
                        _ => _.Store.Email,
                        _ => _.Store.IsPublished,
                        _ => _.Store.Facebook,
                        _ => _.Store.Instagram,
                        _ => _.Store.LinkedIn,
                        _ => _.Store.Youtube,
                        _ => _.Store.Fax,
                        _ => _.Store.ZipCode,
                        _ => _.Store.Website,
                        _ => _.Store.YearOfEstablishment,
                        _ => _.Store.DisplaySequence,
                        _ => _.Store.Score,
                        _ => _.Store.LegalName,
                        _ => _.Store.IsLocalOrOnlineStore,
                        _ => _.Store.IsVerified,
                        _ => _.MediaLibraryName,
                        _ => _.CountryName,
                        _ => _.StateName,
                        _ => _.RatingLikeName,
                        _ => _.MasterTagName
                        );

                });
        }
    }
}