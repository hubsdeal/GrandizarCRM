using System.Collections.Generic;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.Shop.Exporting
{
    public interface IStoreReviewsExcelExporter
    {
        FileDto ExportToFile(List<GetStoreReviewForViewDto> storeReviews);
    }
}