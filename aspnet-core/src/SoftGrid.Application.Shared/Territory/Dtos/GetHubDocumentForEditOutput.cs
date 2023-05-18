using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Territory.Dtos
{
    public class GetHubDocumentForEditOutput
    {
        public CreateOrEditHubDocumentDto HubDocument { get; set; }

        public string HubName { get; set; }

        public string DocumentTypeName { get; set; }

    }
}