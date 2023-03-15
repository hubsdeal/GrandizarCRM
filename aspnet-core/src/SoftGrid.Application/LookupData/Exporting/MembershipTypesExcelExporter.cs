using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class MembershipTypesExcelExporter : NpoiExcelExporterBase, IMembershipTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MembershipTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMembershipTypeForViewDto> membershipTypes)
        {
            return CreateExcelPackage(
                "MembershipTypes.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("MembershipTypes"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description")
                        );

                    AddObjects(
                        sheet, membershipTypes,
                        _ => _.MembershipType.Name,
                        _ => _.MembershipType.Description
                        );

                });
        }
    }
}