using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductReturnInfoForEditOutput
    {
        public CreateOrEditProductReturnInfoDto ProductReturnInfo { get; set; }

        public string ProductName { get; set; }

        public string ReturnTypeName { get; set; }

        public string ReturnStatusName { get; set; }

    }
}