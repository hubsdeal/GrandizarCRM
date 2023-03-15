using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class GetHubTypeForEditOutput
    {
        public CreateOrEditHubTypeDto HubType { get; set; }

    }
}