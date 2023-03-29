using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductNoteForEditOutput
    {
        public CreateOrEditProductNoteDto ProductNote { get; set; }

        public string ProductName { get; set; }

    }
}