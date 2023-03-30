using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Territory.Dtos
{
    public class CreateOrEditHubContactDto : EntityDto<long?>
    {

        public int? DisplayScore { get; set; }

        public long HubId { get; set; }

        public long ContactId { get; set; }

    }
}