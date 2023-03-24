namespace SoftGrid.CRM.Dtos
{
    public class GetContactTagForViewDto
    {
        public ContactTagDto ContactTag { get; set; }

        public string ContactFullName { get; set; }

        public string MasterTagCategoryName { get; set; }

        public string MasterTagName { get; set; }

    }
}