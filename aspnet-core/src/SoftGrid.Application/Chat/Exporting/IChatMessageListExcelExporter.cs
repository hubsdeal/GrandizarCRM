using System.Collections.Generic;
using Abp;
using SoftGrid.Chat.Dto;
using SoftGrid.Dto;

namespace SoftGrid.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
