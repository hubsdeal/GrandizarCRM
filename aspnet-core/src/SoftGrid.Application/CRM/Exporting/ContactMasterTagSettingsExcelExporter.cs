using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class ContactMasterTagSettingsExcelExporter : NpoiExcelExporterBase, IContactMasterTagSettingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ContactMasterTagSettingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetContactMasterTagSettingForViewDto> contactMasterTagSettings)
        {
            return CreateExcelPackage(
                    "ContactMasterTagSettings.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("ContactMasterTagSettings"));

                        AddHeader(
                            sheet,
                        L("DisplaySequence"),
                        L("DisplayPublic"),
                        L("CustomTagTitle"),
                        L("CustomTagChatQuestion"),
                        L("AnswerTypeId"),
                        (L("MasterTag")) + L("Name"),
                        (L("MasterTagCategory")) + L("Name")
                            );

                        AddObjects(
                            sheet, contactMasterTagSettings,
                        _ => _.ContactMasterTagSetting.DisplaySequence,
                        _ => _.ContactMasterTagSetting.DisplayPublic,
                        _ => _.ContactMasterTagSetting.CustomTagTitle,
                        _ => _.ContactMasterTagSetting.CustomTagChatQuestion,
                        _ => _.ContactMasterTagSetting.AnswerTypeId,
                        _ => _.MasterTagName,
                        _ => _.MasterTagCategoryName
                            );

                    });

        }
    }
}