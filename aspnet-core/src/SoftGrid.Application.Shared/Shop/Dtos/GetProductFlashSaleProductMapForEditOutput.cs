using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductFlashSaleProductMapForEditOutput
    {
        public CreateOrEditProductFlashSaleProductMapDto ProductFlashSaleProductMap { get; set; }

        public string ProductName { get; set; }

        public string StoreName { get; set; }

        public string MembershipTypeName { get; set; }

    }
}