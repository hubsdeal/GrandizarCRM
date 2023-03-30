using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductFaqDto : EntityDto<long?>
    {

        [Required]
        [StringLength(ProductFaqConsts.MaxQuestionLength, MinimumLength = ProductFaqConsts.MinQuestionLength)]
        public string Question { get; set; }

        public string Answer { get; set; }

        public bool Template { get; set; }

        public bool Publish { get; set; }

        public long ProductId { get; set; }

    }
}