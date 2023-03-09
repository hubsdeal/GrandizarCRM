using Abp.Events.Bus;

namespace SoftGrid.MultiTenancy
{
    public class RecurringPaymentsDisabledEventData : EventData
    {
        public int TenantId { get; set; }

        public int EditionId { get; set; }
    }
}
