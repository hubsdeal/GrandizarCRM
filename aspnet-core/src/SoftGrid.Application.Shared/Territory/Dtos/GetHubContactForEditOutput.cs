using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Territory.Dtos
{
    public class GetHubContactForEditOutput
    {
        public CreateOrEditHubContactDto HubContact { get; set; }

        public string HubName { get; set; }

        public string ContactFullName { get; set; }

    }
}