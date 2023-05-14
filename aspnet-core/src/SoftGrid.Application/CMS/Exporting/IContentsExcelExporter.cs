using System.Collections.Generic;
using SoftGrid.CMS.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.CMS.Exporting
{
    public interface IContentsExcelExporter
    {
        FileDto ExportToFile(List<GetContentForViewDto> contents);
    }
}