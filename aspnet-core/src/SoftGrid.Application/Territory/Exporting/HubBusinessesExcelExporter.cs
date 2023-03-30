using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Territory.Exporting
{
    public class HubBusinessesExcelExporter : NpoiExcelExporterBase, IHubBusinessesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HubBusinessesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHubBusinessForViewDto> hubBusinesses)
        {
            return CreateExcelPackage(
                "HubBusinesses.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("HubBusinesses"));

                    AddHeader(
                        sheet,
                        L("Published"),
                        L("DisplayScore"),
                        (L("Hub")) + L("Name"),
                        (L("Business")) + L("Name")
                        );

                    AddObjects(
                        sheet, hubBusinesses,
                        _ => _.HubBusiness.Published,
                        _ => _.HubBusiness.DisplayScore,
                        _ => _.HubName,
                        _ => _.BusinessName
                        );

                });
        }
    }
}