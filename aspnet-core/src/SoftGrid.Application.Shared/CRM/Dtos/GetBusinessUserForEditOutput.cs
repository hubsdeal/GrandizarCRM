using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetBusinessUserForEditOutput
    {
        public CreateOrEditBusinessUserDto BusinessUser { get; set; }

        public string BusinessName { get; set; }

        public string UserName { get; set; }

    }
}