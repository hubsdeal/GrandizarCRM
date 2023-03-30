using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductOwnerPublicContactInfosExcelExporter : NpoiExcelExporterBase, IProductOwnerPublicContactInfosExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductOwnerPublicContactInfosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductOwnerPublicContactInfoForViewDto> productOwnerPublicContactInfos)
        {
            return CreateExcelPackage(
                "ProductOwnerPublicContactInfos.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ProductOwnerPublicContactInfos"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Mobile"),
                        L("Email"),
                        L("ShortBio"),
                        L("Publish"),
                        L("PhotoId"),
                        (L("Contact")) + L("FullName"),
                        (L("Product")) + L("Name"),
                        (L("Store")) + L("Name"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, productOwnerPublicContactInfos,
                        _ => _.ProductOwnerPublicContactInfo.Name,
                        _ => _.ProductOwnerPublicContactInfo.Mobile,
                        _ => _.ProductOwnerPublicContactInfo.Email,
                        _ => _.ProductOwnerPublicContactInfo.ShortBio,
                        _ => _.ProductOwnerPublicContactInfo.Publish,
                        _ => _.ProductOwnerPublicContactInfo.PhotoId,
                        _ => _.ContactFullName,
                        _ => _.ProductName,
                        _ => _.StoreName,
                        _ => _.UserName
                        );

                });
        }
    }
}