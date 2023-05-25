using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.JobManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.JobManagement.Exporting
{
    public class JobMasterTagSettingsExcelExporter : NpoiExcelExporterBase, IJobMasterTagSettingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public JobMasterTagSettingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetJobMasterTagSettingForViewDto> jobMasterTagSettings)
        {
            return CreateExcelPackage(
                    "JobMasterTagSettings.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("JobMasterTagSettings"));

                        AddHeader(
                            sheet,
                        L("DisplaySequence"),
                        L("DisplayPublic"),
                        L("AnswerTypeId"),
                        L("CustomTagTitle"),
                        L("CustomTagChatQuestion"),
                        (L("MasterTag")) + L("Name"),
                        (L("MasterTagCategory")) + L("Name")
                            );

                        AddObjects(
                            sheet, jobMasterTagSettings,
                        _ => _.JobMasterTagSetting.DisplaySequence,
                        _ => _.JobMasterTagSetting.DisplayPublic,
                        _ => _.JobMasterTagSetting.AnswerTypeId,
                        _ => _.JobMasterTagSetting.CustomTagTitle,
                        _ => _.JobMasterTagSetting.CustomTagChatQuestion,
                        _ => _.MasterTagName,
                        _ => _.MasterTagCategoryName
                            );

                    });

        }
    }
}