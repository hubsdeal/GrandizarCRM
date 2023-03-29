using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductPackageDto : EntityDto<long?>
    {

        public long? PackageProductId { get; set; }

        public int? DisplaySequence { get; set; }

        public double? Price { get; set; }

        public int? Quantity { get; set; }

        public long PrimaryProductId { get; set; }

        public long? MediaLibraryId { get; set; }

    }
}