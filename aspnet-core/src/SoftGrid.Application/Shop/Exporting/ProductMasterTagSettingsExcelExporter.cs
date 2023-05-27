using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class ProductMasterTagSettingsExcelExporter : NpoiExcelExporterBase, IProductMasterTagSettingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductMasterTagSettingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductMasterTagSettingForViewDto> productMasterTagSettings)
        {
            return CreateExcelPackage(
                    "ProductMasterTagSettings.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("ProductMasterTagSettings"));

                        AddHeader(
                            sheet,
                        L("DisplaySequence"),
                        L("CustomTagTitle"),
                        L("CustomTagChatQuestion"),
                        L("DisplayPublic"),
                        L("AnswerTypeId"),
                        (L("ProductCategory")) + L("Name"),
                        (L("MasterTagCategory")) + L("Name")
                            );

                        AddObjects(
                            sheet, productMasterTagSettings,
                        _ => _.ProductMasterTagSetting.DisplaySequence,
                        _ => _.ProductMasterTagSetting.CustomTagTitle,
                        _ => _.ProductMasterTagSetting.CustomTagChatQuestion,
                        _ => _.ProductMasterTagSetting.DisplayPublic,
                        _ => _.ProductMasterTagSetting.AnswerTypeId,
                        _ => _.ProductCategoryName,
                        _ => _.MasterTagCategoryName
                            );

                    });

        }
    }
}