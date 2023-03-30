using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class SubscriptionTypesExcelExporter : NpoiExcelExporterBase, ISubscriptionTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SubscriptionTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSubscriptionTypeForViewDto> subscriptionTypes)
        {
            return CreateExcelPackage(
                "SubscriptionTypes.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("SubscriptionTypes"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("NumberOfDays")
                        );

                    AddObjects(
                        sheet, subscriptionTypes,
                        _ => _.SubscriptionType.Name,
                        _ => _.SubscriptionType.NumberOfDays
                        );

                });
        }
    }
}