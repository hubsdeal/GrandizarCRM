namespace SoftGrid.JobManagement.Dtos
{
    public class GetJobMasterTagSettingForViewDto
    {
        public JobMasterTagSettingDto JobMasterTagSetting { get; set; }

        public string MasterTagName { get; set; }

        public string MasterTagCategoryName { get; set; }

    }
}