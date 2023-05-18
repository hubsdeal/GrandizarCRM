namespace SoftGrid.JobManagement.Dtos
{
    public class GetJobDocumentForViewDto
    {
        public JobDocumentDto JobDocument { get; set; }

        public string JobTitle { get; set; }

        public string DocumentTypeName { get; set; }

    }
}