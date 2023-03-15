﻿using Abp.Application.Services.Dto;
using System;

namespace SoftGrid.LookupData.Dtos
{
    public class GetAllEmailTemplatesForExcelInput
    {
        public string Filter { get; set; }

        public string SubjectFilter { get; set; }

        public string ContentFilter { get; set; }

        public int? PublishedFilter { get; set; }

    }
}