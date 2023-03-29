using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductPackageForEditOutput
    {
        public CreateOrEditProductPackageDto ProductPackage { get; set; }

        public string ProductName { get; set; }

        public string MediaLibraryName { get; set; }

    }
}