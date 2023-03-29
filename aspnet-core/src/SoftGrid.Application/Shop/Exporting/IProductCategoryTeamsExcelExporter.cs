using System.Collections.Generic;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop.Exporting
{
    public interface IProductCategoryTeamsExcelExporter
    {
        FileDto ExportToFile(List<GetProductCategoryTeamForViewDto> productCategoryTeams);
    }
}