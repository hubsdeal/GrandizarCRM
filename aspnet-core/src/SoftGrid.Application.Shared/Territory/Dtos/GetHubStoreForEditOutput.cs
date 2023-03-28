using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Territory.Dtos
{
    public class GetHubStoreForEditOutput
    {
        public CreateOrEditHubStoreDto HubStore { get; set; }

        public string HubName { get; set; }

        public string StoreName { get; set; }

    }
}