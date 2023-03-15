using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductCategoriesExcelExporter : NpoiExcelExporterBase, IProductCategoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductCategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductCategoryForViewDto> productCategories)
        {
            return CreateExcelPackage(
                "ProductCategories.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductCategories"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("HasParentCategory"),
                        L("ParentCategoryId"),
                        L("Url"),
                        L("MetaTitle"),
                        L("MetaKeywords"),
                        L("DisplaySequence"),
                        L("ProductOrService"),
                        (L("MediaLibrary")) + L("Name")
                        );

                    AddObjects(
                        sheet, productCategories,
                        _ => _.ProductCategory.Name,
                        _ => _.ProductCategory.Description,
                        _ => _.ProductCategory.HasParentCategory,
                        _ => _.ProductCategory.ParentCategoryId,
                        _ => _.ProductCategory.Url,
                        _ => _.ProductCategory.MetaTitle,
                        _ => _.ProductCategory.MetaKeywords,
                        _ => _.ProductCategory.DisplaySequence,
                        _ => _.ProductCategory.ProductOrService,
                        _ => _.MediaLibraryName
                        );

                });
        }
    }
}