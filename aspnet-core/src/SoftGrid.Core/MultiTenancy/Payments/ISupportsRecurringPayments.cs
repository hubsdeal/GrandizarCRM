using Abp.Events.Bus.Handlers;

namespace SoftGrid.MultiTenancy.Payments
{
    public interface ISupportsRecurringPayments : 
        IEventHandler<RecurringPaymentsDisabledEventData>, 
        IEventHandler<RecurringPaymentsEnabledEventData>,
        IEventHandler<TenantEditionChangedEventData>
    {

    }
}
