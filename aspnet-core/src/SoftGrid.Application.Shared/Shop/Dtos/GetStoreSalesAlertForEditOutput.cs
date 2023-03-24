using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreSalesAlertForEditOutput
    {
        public CreateOrEditStoreSalesAlertDto StoreSalesAlert { get; set; }

        public string StoreName { get; set; }

    }
}