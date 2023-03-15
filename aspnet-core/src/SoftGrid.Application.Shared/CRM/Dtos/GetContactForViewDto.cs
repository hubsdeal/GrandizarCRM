namespace SoftGrid.CRM.Dtos
{
    public class GetContactForViewDto
    {
        public ContactDto Contact { get; set; }

        public string UserName { get; set; }

        public string CountryName { get; set; }

        public string StateName { get; set; }

        public string MembershipTypeName { get; set; }

    }
}