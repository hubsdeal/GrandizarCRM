using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.CRM.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.CRM.Exporting
{
    public class EmployeeTagsExcelExporter : NpoiExcelExporterBase, IEmployeeTagsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeeTagsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeTagForViewDto> employeeTags)
        {
            return CreateExcelPackage(
                "EmployeeTags.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("EmployeeTags"));

                    AddHeader(
                        sheet,
                        L("CustomTag"),
                        L("TagValue"),
                        L("Verified"),
                        L("Sequence"),
                        (L("Employee")) + L("Name"),
                        (L("MasterTagCategory")) + L("Name"),
                        (L("MasterTag")) + L("Name")
                        );

                    AddObjects(
                        sheet, employeeTags,
                        _ => _.EmployeeTag.CustomTag,
                        _ => _.EmployeeTag.TagValue,
                        _ => _.EmployeeTag.Verified,
                        _ => _.EmployeeTag.Sequence,
                        _ => _.EmployeeName,
                        _ => _.MasterTagCategoryName,
                        _ => _.MasterTagName
                        );

                });
        }
    }
}