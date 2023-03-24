using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllStoreMediasForExcelInput
    {
        public string Filter { get; set; }

        public int? MaxDisplaySequenceFilter { get; set; }
        public int? MinDisplaySequenceFilter { get; set; }

        public string StoreNameFilter { get; set; }

        public string MediaLibraryNameFilter { get; set; }

    }
}