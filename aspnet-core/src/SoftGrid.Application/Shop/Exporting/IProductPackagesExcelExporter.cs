using System.Collections.Generic;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop.Exporting
{
    public interface IProductPackagesExcelExporter
    {
        FileDto ExportToFile(List<GetProductPackageForViewDto> productPackages);
    }
}