using System.Collections.Generic;
using SoftGrid.Auditing.Dto;
using SoftGrid.Dto;

namespace SoftGrid.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
