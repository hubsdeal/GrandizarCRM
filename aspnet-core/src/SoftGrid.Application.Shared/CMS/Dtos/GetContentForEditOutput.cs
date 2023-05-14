using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CMS.Dtos
{
    public class GetContentForEditOutput
    {
        public CreateOrEditContentDto Content { get; set; }

        public string MediaLibraryName { get; set; }

    }
}