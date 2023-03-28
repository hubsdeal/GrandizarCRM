using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Territory.Exporting
{
    public class HubStoresExcelExporter : NpoiExcelExporterBase, IHubStoresExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HubStoresExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHubStoreForViewDto> hubStores)
        {
            return CreateExcelPackage(
                "HubStores.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("HubStores"));

                    AddHeader(
                        sheet,
                        L("Published"),
                        L("DisplaySequence"),
                        (L("Hub")) + L("Name"),
                        (L("Store")) + L("Name")
                        );

                    AddObjects(
                        sheet, hubStores,
                        _ => _.HubStore.Published,
                        _ => _.HubStore.DisplaySequence,
                        _ => _.HubName,
                        _ => _.StoreName
                        );

                });
        }
    }
}