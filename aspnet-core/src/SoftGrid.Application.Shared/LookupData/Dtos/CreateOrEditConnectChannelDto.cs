using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class CreateOrEditConnectChannelDto : EntityDto<long?>
    {

        public string Name { get; set; }

    }
}