using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductDocumentForEditOutput
    {
        public CreateOrEditProductDocumentDto ProductDocument { get; set; }

        public string ProductName { get; set; }

        public string DocumentTypeName { get; set; }

    }
}