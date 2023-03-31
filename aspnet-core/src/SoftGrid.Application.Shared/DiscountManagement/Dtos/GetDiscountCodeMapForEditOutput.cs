using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.DiscountManagement.Dtos
{
    public class GetDiscountCodeMapForEditOutput
    {
        public CreateOrEditDiscountCodeMapDto DiscountCodeMap { get; set; }

        public string DiscountCodeGeneratorName { get; set; }

        public string StoreName { get; set; }

        public string ProductName { get; set; }

        public string MembershipTypeName { get; set; }

    }
}