using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductMediaDto : EntityDto<long?>
    {

        public int? DisplaySequence { get; set; }

        public long ProductId { get; set; }

        public long? MediaLibraryId { get; set; }

    }
}