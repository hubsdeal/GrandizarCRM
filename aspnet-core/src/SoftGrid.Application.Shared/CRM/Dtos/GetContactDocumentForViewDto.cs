namespace SoftGrid.CRM.Dtos
{
    public class GetContactDocumentForViewDto
    {
        public ContactDocumentDto ContactDocument { get; set; }

        public string ContactFullName { get; set; }

        public string DocumentTypeName { get; set; }

    }
}