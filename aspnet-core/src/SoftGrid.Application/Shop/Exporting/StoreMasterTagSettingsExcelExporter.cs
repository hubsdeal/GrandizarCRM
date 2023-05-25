using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.Shop.Exporting
{
    public class StoreMasterTagSettingsExcelExporter : NpoiExcelExporterBase, IStoreMasterTagSettingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StoreMasterTagSettingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStoreMasterTagSettingForViewDto> storeMasterTagSettings)
        {
            return CreateExcelPackage(
                    "StoreMasterTagSettings.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("StoreMasterTagSettings"));

                        AddHeader(
                            sheet,
                        L("CustomTagTitle"),
                        L("CustomTagChatQuestion"),
                        L("DisplayPublic"),
                        L("DisplaySequence"),
                        L("AnswerTypeId"),
                        (L("StoreTagSettingCategory")) + L("Name"),
                        (L("MasterTagCategory")) + L("Name")
                            );

                        AddObjects(
                            sheet, storeMasterTagSettings,
                        _ => _.StoreMasterTagSetting.CustomTagTitle,
                        _ => _.StoreMasterTagSetting.CustomTagChatQuestion,
                        _ => _.StoreMasterTagSetting.DisplayPublic,
                        _ => _.StoreMasterTagSetting.DisplaySequence,
                        _ => _.StoreMasterTagSetting.AnswerTypeId,
                        _ => _.StoreTagSettingCategoryName,
                        _ => _.MasterTagCategoryName
                            );

                    });

        }
    }
}