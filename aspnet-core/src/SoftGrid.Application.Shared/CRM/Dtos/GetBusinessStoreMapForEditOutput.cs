using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetBusinessStoreMapForEditOutput
    {
        public CreateOrEditBusinessStoreMapDto BusinessStoreMap { get; set; }

        public string BusinessName { get; set; }

        public string StoreName { get; set; }

    }
}