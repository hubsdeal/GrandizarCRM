using System.Collections.Generic;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData.Exporting
{
    public interface IContractTypesExcelExporter
    {
        FileDto ExportToFile(List<GetContractTypeForViewDto> contractTypes);
    }
}