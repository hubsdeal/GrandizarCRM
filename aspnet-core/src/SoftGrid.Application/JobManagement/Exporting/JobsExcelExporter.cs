using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.JobManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.JobManagement.Exporting
{
    public class JobsExcelExporter : NpoiExcelExporterBase, IJobsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public JobsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetJobForViewDto> jobs)
        {
            return CreateExcelPackage(
                "Jobs.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("Jobs"));

                    AddHeader(
                        sheet,
                        L("Title"),
                        L("FullTimeJobOrGigWorkProject"),
                        L("RemoteWorkOrOnSiteWork"),
                        L("SalaryBasedOrFixedPrice"),
                        L("SalaryOrStaffingRate"),
                        L("ReferralPoints"),
                        L("Template"),
                        L("NumberOfJobs"),
                        L("MinimumExperience"),
                        L("MaximumExperience"),
                        L("JobDescription"),
                        L("JobLocationFullAddress"),
                        L("ZipCode"),
                        L("Latitude"),
                        L("Longitude"),
                        L("StartDate"),
                        L("HireByDate"),
                        L("PublishDate"),
                        L("ExpirationDate"),
                        L("InternalJobDescription"),
                        L("CityLocation"),
                        L("Published"),
                        L("Url"),
                        (L("MasterTagCategory")) + L("Name"),
                        (L("MasterTag")) + L("Name"),
                        (L("ProductCategory")) + L("Name"),
                        (L("Currency")) + L("Name"),
                        (L("Business")) + L("Name"),
                        (L("Country")) + L("Name"),
                        (L("State")) + L("Name"),
                        (L("City")) + L("Name"),
                        (L("JobStatusType")) + L("Name"),
                        (L("Store")) + L("Name")
                        );

                    AddObjects(
                        sheet, jobs,
                        _ => _.Job.Title,
                        _ => _.Job.FullTimeJobOrGigWorkProject,
                        _ => _.Job.RemoteWorkOrOnSiteWork,
                        _ => _.Job.SalaryBasedOrFixedPrice,
                        _ => _.Job.SalaryOrStaffingRate,
                        _ => _.Job.ReferralPoints,
                        _ => _.Job.Template,
                        _ => _.Job.NumberOfJobs,
                        _ => _.Job.MinimumExperience,
                        _ => _.Job.MaximumExperience,
                        _ => _.Job.JobDescription,
                        _ => _.Job.JobLocationFullAddress,
                        _ => _.Job.ZipCode,
                        _ => _.Job.Latitude,
                        _ => _.Job.Longitude,
                        _ => _timeZoneConverter.Convert(_.Job.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Job.HireByDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Job.PublishDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Job.ExpirationDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Job.InternalJobDescription,
                        _ => _.Job.CityLocation,
                        _ => _.Job.Published,
                        _ => _.Job.Url,
                        _ => _.MasterTagCategoryName,
                        _ => _.MasterTagName,
                        _ => _.ProductCategoryName,
                        _ => _.CurrencyName,
                        _ => _.BusinessName,
                        _ => _.CountryName,
                        _ => _.StateName,
                        _ => _.CityName,
                        _ => _.JobStatusTypeName,
                        _ => _.StoreName
                        );

                    for (var i = 1; i <= jobs.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[16], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(16); for (var i = 1; i <= jobs.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[17], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(17); for (var i = 1; i <= jobs.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[18], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(18); for (var i = 1; i <= jobs.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[19], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(19);
                });
        }
    }
}