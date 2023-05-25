using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreTagSettingCategoriesExcelExporter : NpoiExcelExporterBase, IStoreTagSettingCategoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreTagSettingCategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreTagSettingCategoryForViewDto> storeTagSettingCategories)
        {
            return CreateExcelPackage(
                    "StoreTagSettingCategories.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("StoreTagSettingCategories"));

                        AddHeader(
                            sheet,
                        L("Name"),
                        L("ImageId"),
                        L("Description")
                            );

                        AddObjects(
                            sheet, storeTagSettingCategories,
                        _ => _.StoreTagSettingCategory.Name,
                        _ => _.StoreTagSettingCategory.ImageId,
                        _ => _.StoreTagSettingCategory.Description
                            );

                    });

        }
    }
}