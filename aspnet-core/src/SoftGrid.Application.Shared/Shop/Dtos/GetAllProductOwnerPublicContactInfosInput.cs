using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductOwnerPublicContactInfosInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string MobileFilter { get; set; }

        public string EmailFilter { get; set; }

        public string ShortBioFilter { get; set; }

        public int? PublishFilter { get; set; }

        public Guid? PhotoIdFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

        public string ProductNameFilter { get; set; }

        public string StoreNameFilter { get; set; }

        public string UserNameFilter { get; set; }

    }
}