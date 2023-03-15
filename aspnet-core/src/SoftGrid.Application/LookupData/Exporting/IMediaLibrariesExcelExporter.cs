using System.Collections.Generic;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData.Exporting
{
    public interface IMediaLibrariesExcelExporter
    {
        FileDto ExportToFile(List<GetMediaLibraryForViewDto> mediaLibraries);
    }
}