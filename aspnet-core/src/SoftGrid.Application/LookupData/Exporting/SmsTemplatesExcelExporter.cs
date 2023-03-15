using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class SmsTemplatesExcelExporter : NpoiExcelExporterBase, ISmsTemplatesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SmsTemplatesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSmsTemplateForViewDto> smsTemplates)
        {
            return CreateExcelPackage(
                "SmsTemplates.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("SmsTemplates"));

                    AddHeader(
                        sheet,
                        L("Title"),
                        L("Content"),
                        L("Published")
                        );

                    AddObjects(
                        sheet, smsTemplates,
                        _ => _.SmsTemplate.Title,
                        _ => _.SmsTemplate.Content,
                        _ => _.SmsTemplate.Published
                        );

                });
        }
    }
}