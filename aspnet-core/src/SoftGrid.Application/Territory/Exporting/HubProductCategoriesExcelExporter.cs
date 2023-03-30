using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Territory.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Territory.Exporting
{
    public class HubProductCategoriesExcelExporter : NpoiExcelExporterBase, IHubProductCategoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HubProductCategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHubProductCategoryForViewDto> hubProductCategories)
        {
            return CreateExcelPackage(
                "HubProductCategories.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("HubProductCategories"));

                    AddHeader(
                        sheet,
                        L("Published"),
                        L("DisplayScore"),
                        (L("Hub")) + L("Name"),
                        (L("ProductCategory")) + L("Name")
                        );

                    AddObjects(
                        sheet, hubProductCategories,
                        _ => _.HubProductCategory.Published,
                        _ => _.HubProductCategory.DisplayScore,
                        _ => _.HubName,
                        _ => _.ProductCategoryName
                        );

                });
        }
    }
}