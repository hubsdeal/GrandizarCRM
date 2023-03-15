using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class ConnectChannelsExcelExporter : NpoiExcelExporterBase, IConnectChannelsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ConnectChannelsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetConnectChannelForViewDto> connectChannels)
        {
            return CreateExcelPackage(
                "ConnectChannels.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ConnectChannels"));

                    AddHeader(
                        sheet,
                        L("Name")
                        );

                    AddObjects(
                        sheet, connectChannels,
                        _ => _.ConnectChannel.Name
                        );

                });
        }
    }
}