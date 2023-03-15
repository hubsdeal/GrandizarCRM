using System.Collections.Generic;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData.Exporting
{
    public interface IMembershipTypesExcelExporter
    {
        FileDto ExportToFile(List<GetMembershipTypeForViewDto> membershipTypes);
    }
}