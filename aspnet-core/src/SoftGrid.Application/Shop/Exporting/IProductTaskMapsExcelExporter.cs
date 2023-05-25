using System.Collections.Generic;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop.Exporting
{
    public interface IProductTaskMapsExcelExporter
    {
        FileDto ExportToFile(List<GetProductTaskMapForViewDto> productTaskMaps);
    }
}