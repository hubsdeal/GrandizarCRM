using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class ProductCustomerQueryDto : EntityDto<long>
    {
        public string Question { get; set; }

        public string Answer { get; set; }

        public DateTime? AnswerDate { get; set; }

        public string AnswerTime { get; set; }

        public bool Publish { get; set; }

        public long ProductId { get; set; }

        public long? ContactId { get; set; }

        public long? EmployeeId { get; set; }

    }
}