using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreDocumentForEditOutput
    {
        public CreateOrEditStoreDocumentDto StoreDocument { get; set; }

        public string StoreName { get; set; }

        public string DocumentTypeName { get; set; }

    }
}