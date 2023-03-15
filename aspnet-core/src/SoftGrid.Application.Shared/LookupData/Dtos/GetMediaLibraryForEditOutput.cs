using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class GetMediaLibraryForEditOutput
    {
        public CreateOrEditMediaLibraryDto MediaLibrary { get; set; }

        public string MasterTagCategoryName { get; set; }

        public string MasterTagName { get; set; }

    }
}