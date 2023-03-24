using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class BusinessUsersExcelExporter : NpoiExcelExporterBase, IBusinessUsersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BusinessUsersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBusinessUserForViewDto> businessUsers)
        {
            return CreateExcelPackage(
                "BusinessUsers.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("BusinessUsers"));

                    AddHeader(
                        sheet,
                        (L("Business")) + L("Name"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, businessUsers,
                        _ => _.BusinessName,
                        _ => _.UserName
                        );

                });
        }
    }
}