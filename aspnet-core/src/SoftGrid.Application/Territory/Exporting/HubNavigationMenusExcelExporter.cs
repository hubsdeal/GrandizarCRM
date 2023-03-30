using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Territory.Exporting
{
    public class HubNavigationMenusExcelExporter : NpoiExcelExporterBase, IHubNavigationMenusExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HubNavigationMenusExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHubNavigationMenuForViewDto> hubNavigationMenus)
        {
            return CreateExcelPackage(
                "HubNavigationMenus.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("HubNavigationMenus"));

                    AddHeader(
                        sheet,
                        L("CustomName"),
                        L("NavigationLink"),
                        (L("Hub")) + L("Name"),
                        (L("MasterNavigationMenu")) + L("Name")
                        );

                    AddObjects(
                        sheet, hubNavigationMenus,
                        _ => _.HubNavigationMenu.CustomName,
                        _ => _.HubNavigationMenu.NavigationLink,
                        _ => _.HubName,
                        _ => _.MasterNavigationMenuName
                        );

                });
        }
    }
}