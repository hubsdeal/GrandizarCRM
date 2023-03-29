using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductCustomerQueriesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string QuestionFilter { get; set; }

        public string AnswerFilter { get; set; }

        public DateTime? MaxAnswerDateFilter { get; set; }
        public DateTime? MinAnswerDateFilter { get; set; }

        public string AnswerTimeFilter { get; set; }

        public int? PublishFilter { get; set; }

        public string ProductNameFilter { get; set; }

        public string ContactFullNameFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

    }
}