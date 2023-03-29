using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.Shop.Dtos
{
    public class GetAllProductFaqsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string QuestionFilter { get; set; }

        public string AnswerFilter { get; set; }

        public int? TemplateFilter { get; set; }

        public int? PublishFilter { get; set; }

        public string ProductNameFilter { get; set; }

    }
}