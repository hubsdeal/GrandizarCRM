using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreBankAccountForEditOutput
    {
        public CreateOrEditStoreBankAccountDto StoreBankAccount { get; set; }

        public string StoreName { get; set; }

    }
}