using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoftGrid.MultiTenancy.HostDashboard.Dto;

namespace SoftGrid.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}