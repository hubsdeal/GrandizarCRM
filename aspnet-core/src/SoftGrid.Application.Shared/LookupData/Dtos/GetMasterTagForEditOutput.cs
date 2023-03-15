using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class GetMasterTagForEditOutput
    {
        public CreateOrEditMasterTagDto MasterTag { get; set; }

        public string MasterTagCategoryName { get; set; }

        public string MediaLibraryName { get; set; }

    }
}