using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.SalesLeadManagement.Exporting
{
    public class LeadReferralRewardsExcelExporter : NpoiExcelExporterBase, ILeadReferralRewardsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LeadReferralRewardsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLeadReferralRewardForViewDto> leadReferralRewards)
        {
            return CreateExcelPackage(
                "LeadReferralRewards.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("LeadReferralRewards"));

                    AddHeader(
                        sheet,
                        L("FirstName"),
                        L("LastName"),
                        L("Phone"),
                        L("Email"),
                        L("RewardType"),
                        L("RewardAmount"),
                        L("RewardStatus"),
                        (L("Lead")) + L("Title"),
                        (L("Contact")) + L("FullName")
                        );

                    AddObjects(
                        sheet, leadReferralRewards,
                        _ => _.LeadReferralReward.FirstName,
                        _ => _.LeadReferralReward.LastName,
                        _ => _.LeadReferralReward.Phone,
                        _ => _.LeadReferralReward.Email,
                        _ => _.LeadReferralReward.RewardType,
                        _ => _.LeadReferralReward.RewardAmount,
                        _ => _.LeadReferralReward.RewardStatus,
                        _ => _.LeadTitle,
                        _ => _.ContactFullName
                        );

                });
        }
    }
}