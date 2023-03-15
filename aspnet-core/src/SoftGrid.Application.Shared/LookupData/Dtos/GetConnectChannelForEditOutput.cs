using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class GetConnectChannelForEditOutput
    {
        public CreateOrEditConnectChannelDto ConnectChannel { get; set; }

    }
}