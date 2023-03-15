using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductsExcelExporter : NpoiExcelExporterBase, IProductsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductForViewDto> products)
        {
            return CreateExcelPackage(
                "Products.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Products"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("ShortDescription"),
                        L("Description"),
                        L("Sku"),
                        L("Url"),
                        L("SeoTitle"),
                        L("MetaKeywords"),
                        L("MetaDescription"),
                        L("RegularPrice"),
                        L("SalePrice"),
                        L("PriceDiscountPercentage"),
                        L("CallForPrice"),
                        L("UnitPrice"),
                        L("MeasurementAmount"),
                        L("IsTaxExempt"),
                        L("StockQuantity"),
                        L("IsDisplayStockQuantity"),
                        L("IsPublished"),
                        L("IsPackageProduct"),
                        L("InternalNotes"),
                        L("IsTemplate"),
                        L("PriceDiscountAmount"),
                        L("IsService"),
                        L("IsWholeSaleProduct"),
                        L("ProductManufacturerSku"),
                        L("ByOrderOnly"),
                        L("Score"),
                        (L("ProductCategory")) + L("Name"),
                        (L("MediaLibrary")) + L("Name"),
                        (L("MeasurementUnit")) + L("Name"),
                        (L("Currency")) + L("Name"),
                        (L("RatingLike")) + L("Name")
                        );

                    AddObjects(
                        sheet, products,
                        _ => _.Product.Name,
                        _ => _.Product.ShortDescription,
                        _ => _.Product.Description,
                        _ => _.Product.Sku,
                        _ => _.Product.Url,
                        _ => _.Product.SeoTitle,
                        _ => _.Product.MetaKeywords,
                        _ => _.Product.MetaDescription,
                        _ => _.Product.RegularPrice,
                        _ => _.Product.SalePrice,
                        _ => _.Product.PriceDiscountPercentage,
                        _ => _.Product.CallForPrice,
                        _ => _.Product.UnitPrice,
                        _ => _.Product.MeasurementAmount,
                        _ => _.Product.IsTaxExempt,
                        _ => _.Product.StockQuantity,
                        _ => _.Product.IsDisplayStockQuantity,
                        _ => _.Product.IsPublished,
                        _ => _.Product.IsPackageProduct,
                        _ => _.Product.InternalNotes,
                        _ => _.Product.IsTemplate,
                        _ => _.Product.PriceDiscountAmount,
                        _ => _.Product.IsService,
                        _ => _.Product.IsWholeSaleProduct,
                        _ => _.Product.ProductManufacturerSku,
                        _ => _.Product.ByOrderOnly,
                        _ => _.Product.Score,
                        _ => _.ProductCategoryName,
                        _ => _.MediaLibraryName,
                        _ => _.MeasurementUnitName,
                        _ => _.CurrencyName,
                        _ => _.RatingLikeName
                        );

                });
        }
    }
}