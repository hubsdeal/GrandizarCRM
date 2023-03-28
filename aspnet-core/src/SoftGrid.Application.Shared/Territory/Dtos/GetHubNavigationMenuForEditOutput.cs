﻿using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Territory.Dtos
{
    public class GetHubNavigationMenuForEditOutput
    {
        public CreateOrEditHubNavigationMenuDto HubNavigationMenu { get; set; }

        public string HubName { get; set; }

        public string MasterNavigationMenuName { get; set; }

    }
}