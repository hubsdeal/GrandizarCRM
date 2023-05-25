using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class BusinessMasterTagSettingsExcelExporter : NpoiExcelExporterBase, IBusinessMasterTagSettingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BusinessMasterTagSettingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBusinessMasterTagSettingForViewDto> businessMasterTagSettings)
        {
            return CreateExcelPackage(
                    "BusinessMasterTagSettings.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("BusinessMasterTagSettings"));

                        AddHeader(
                            sheet,
                        L("DisplaySequence"),
                        L("DisplayPublic"),
                        L("CustomTagTitle"),
                        L("CustomTagChatQuestion"),
                        L("AnswerTypeId"),
                        (L("MasterTagCategory")) + L("Name"),
                        (L("MasterTag")) + L("Name")
                            );

                        AddObjects(
                            sheet, businessMasterTagSettings,
                        _ => _.BusinessMasterTagSetting.DisplaySequence,
                        _ => _.BusinessMasterTagSetting.DisplayPublic,
                        _ => _.BusinessMasterTagSetting.CustomTagTitle,
                        _ => _.BusinessMasterTagSetting.CustomTagChatQuestion,
                        _ => _.BusinessMasterTagSetting.AnswerTypeId,
                        _ => _.MasterTagCategoryName,
                        _ => _.MasterTagName
                            );

                    });

        }
    }
}