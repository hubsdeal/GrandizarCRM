namespace SoftGrid.CRM.Dtos
{
    public class GetEmployeeForViewDto
    {
        public EmployeeDto Employee { get; set; }

        public string StateName { get; set; }

        public string CountryName { get; set; }

        public string ContactFullName { get; set; }

    }
}