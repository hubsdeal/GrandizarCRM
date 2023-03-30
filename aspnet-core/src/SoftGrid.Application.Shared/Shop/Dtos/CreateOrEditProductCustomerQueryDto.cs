using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductCustomerQueryDto : EntityDto<long?>
    {

        [StringLength(ProductCustomerQueryConsts.MaxQuestionLength, MinimumLength = ProductCustomerQueryConsts.MinQuestionLength)]
        public string Question { get; set; }

        public string Answer { get; set; }

        public DateTime? AnswerDate { get; set; }

        [StringLength(ProductCustomerQueryConsts.MaxAnswerTimeLength, MinimumLength = ProductCustomerQueryConsts.MinAnswerTimeLength)]
        public string AnswerTime { get; set; }

        public bool Publish { get; set; }

        public long ProductId { get; set; }

        public long? ContactId { get; set; }

        public long? EmployeeId { get; set; }

    }
}