using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Territory.Dtos
{
    public class GetMasterNavigationMenuForEditOutput
    {
        public CreateOrEditMasterNavigationMenuDto MasterNavigationMenu { get; set; }

    }
}