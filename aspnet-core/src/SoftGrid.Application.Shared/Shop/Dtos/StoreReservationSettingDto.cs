using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreReservationSettingDto : EntityDto<long>
    {
        public bool OfferReservation { get; set; }

        public bool InstantConfirmation { get; set; }

        public bool MessageStoreTeam { get; set; }

        public int? MinNumberOfGuests { get; set; }

        public int? MaxNumberOfGuests { get; set; }

        public bool PublishAvailability { get; set; }

        public long StoreId { get; set; }

    }
}