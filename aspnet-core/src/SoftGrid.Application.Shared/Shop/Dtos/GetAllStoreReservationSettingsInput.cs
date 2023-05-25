using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreReservationSettingsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? OfferReservationFilter { get; set; }

        public int? InstantConfirmationFilter { get; set; }

        public int? MessageStoreTeamFilter { get; set; }

        public int? MaxMinNumberOfGuestsFilter { get; set; }
        public int? MinMinNumberOfGuestsFilter { get; set; }

        public int? MaxMaxNumberOfGuestsFilter { get; set; }
        public int? MinMaxNumberOfGuestsFilter { get; set; }

        public int? PublishAvailabilityFilter { get; set; }

        public string StoreNameFilter { get; set; }

    }
}