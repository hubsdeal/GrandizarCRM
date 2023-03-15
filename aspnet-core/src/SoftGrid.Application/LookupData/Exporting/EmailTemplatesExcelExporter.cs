using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class EmailTemplatesExcelExporter : NpoiExcelExporterBase, IEmailTemplatesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmailTemplatesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmailTemplateForViewDto> emailTemplates)
        {
            return CreateExcelPackage(
                "EmailTemplates.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("EmailTemplates"));

                    AddHeader(
                        sheet,
                        L("Subject"),
                        L("Content"),
                        L("Published")
                        );

                    AddObjects(
                        sheet, emailTemplates,
                        _ => _.EmailTemplate.Subject,
                        _ => _.EmailTemplate.Content,
                        _ => _.EmailTemplate.Published
                        );

                });
        }
    }
}