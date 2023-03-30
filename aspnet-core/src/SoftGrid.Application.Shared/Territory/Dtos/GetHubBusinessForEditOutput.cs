using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Territory.Dtos
{
    public class GetHubBusinessForEditOutput
    {
        public CreateOrEditHubBusinessDto HubBusiness { get; set; }

        public string HubName { get; set; }

        public string BusinessName { get; set; }

    }
}