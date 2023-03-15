using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.LookupData.Dtos
{
    public class GetAllRatingLikesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public int? MaxScoreFilter { get; set; }
        public int? MinScoreFilter { get; set; }

        public string IconLinkFilter { get; set; }

    }
}