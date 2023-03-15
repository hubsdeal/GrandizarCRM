using System.Collections.Generic;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;

namespace SoftGrid.LookupData.Exporting
{
    public interface IConnectChannelsExcelExporter
    {
        FileDto ExportToFile(List<GetConnectChannelForViewDto> connectChannels);
    }
}