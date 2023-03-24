using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreMediaForEditOutput
    {
        public CreateOrEditStoreMediaDto StoreMedia { get; set; }

        public string StoreName { get; set; }

        public string MediaLibraryName { get; set; }

    }
}