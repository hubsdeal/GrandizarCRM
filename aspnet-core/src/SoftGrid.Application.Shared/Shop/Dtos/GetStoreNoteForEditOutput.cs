using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreNoteForEditOutput
    {
        public CreateOrEditStoreNoteDto StoreNote { get; set; }

        public string StoreName { get; set; }

    }
}