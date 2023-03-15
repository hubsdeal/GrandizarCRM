using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class MasterTagCategoriesExcelExporter : NpoiExcelExporterBase, IMasterTagCategoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MasterTagCategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMasterTagCategoryForViewDto> masterTagCategories)
        {
            return CreateExcelPackage(
                "MasterTagCategories.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("MasterTagCategories"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Description"),
                        L("PictureId")
                        );

                    AddObjects(
                        sheet, masterTagCategories,
                        _ => _.MasterTagCategory.Name,
                        _ => _.MasterTagCategory.Description,
                        _ => _.MasterTagCategory.PictureId
                        );

                });
        }
    }
}