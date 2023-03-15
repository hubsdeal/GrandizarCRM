using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Territory.Exporting
{
    public class HubsExcelExporter : NpoiExcelExporterBase, IHubsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HubsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHubForViewDto> hubs)
        {
            return CreateExcelPackage(
                "Hubs.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Hubs"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("EstimatedPopulation"),
                        L("HasParentHub"),
                        L("ParentHubId"),
                        L("Latitude"),
                        L("Longitude"),
                        L("Live"),
                        L("Url"),
                        L("OfficeFullAddress"),
                        L("PartnerOrOwned"),
                        L("PictureId"),
                        L("Phone"),
                        L("YearlyRevenue"),
                        L("DisplaySequence"),
                        (L("Country")) + L("Name"),
                        (L("State")) + L("Name"),
                        (L("City")) + L("Name"),
                        (L("County")) + L("Name"),
                        (L("HubType")) + L("Name"),
                        (L("Currency")) + L("Name")
                        );

                    AddObjects(
                        sheet, hubs,
                        _ => _.Hub.Name,
                        _ => _.Hub.Description,
                        _ => _.Hub.EstimatedPopulation,
                        _ => _.Hub.HasParentHub,
                        _ => _.Hub.ParentHubId,
                        _ => _.Hub.Latitude,
                        _ => _.Hub.Longitude,
                        _ => _.Hub.Live,
                        _ => _.Hub.Url,
                        _ => _.Hub.OfficeFullAddress,
                        _ => _.Hub.PartnerOrOwned,
                        _ => _.Hub.PictureId,
                        _ => _.Hub.Phone,
                        _ => _.Hub.YearlyRevenue,
                        _ => _.Hub.DisplaySequence,
                        _ => _.CountryName,
                        _ => _.StateName,
                        _ => _.CityName,
                        _ => _.CountyName,
                        _ => _.HubTypeName,
                        _ => _.CurrencyName
                        );

                });
        }
    }
}