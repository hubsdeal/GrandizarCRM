using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class CreateOrEditBusinessUserDto : EntityDto<long?>
    {

        public long BusinessId { get; set; }

        public long UserId { get; set; }

    }
}