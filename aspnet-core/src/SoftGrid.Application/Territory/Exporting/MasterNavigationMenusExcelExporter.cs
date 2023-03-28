using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Territory.Exporting
{
    public class MasterNavigationMenusExcelExporter : NpoiExcelExporterBase, IMasterNavigationMenusExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MasterNavigationMenusExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMasterNavigationMenuForViewDto> masterNavigationMenus)
        {
            return CreateExcelPackage(
                "MasterNavigationMenus.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("MasterNavigationMenus"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("HasParentMenu"),
                        L("ParentMenuId"),
                        L("IconLink"),
                        L("MediaLink")
                        );

                    AddObjects(
                        sheet, masterNavigationMenus,
                        _ => _.MasterNavigationMenu.Name,
                        _ => _.MasterNavigationMenu.HasParentMenu,
                        _ => _.MasterNavigationMenu.ParentMenuId,
                        _ => _.MasterNavigationMenu.IconLink,
                        _ => _.MasterNavigationMenu.MediaLink
                        );

                });
        }
    }
}