using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class GetStateForEditOutput
    {
        public CreateOrEditStateDto State { get; set; }

        public string CountryName { get; set; }

    }
}