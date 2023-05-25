using SoftGrid.Shop.Enums;

using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.JobManagement.Dtos
{
    public class JobMasterTagSettingDto : EntityDto<long>
    {
        public int? DisplaySequence { get; set; }

        public bool DisplayPublic { get; set; }

        public AnswerType AnswerTypeId { get; set; }

        public string CustomTagTitle { get; set; }

        public string CustomTagChatQuestion { get; set; }

        public long JobCategoryId { get; set; }

        public long? MasterTagCategoryId { get; set; }

    }
}