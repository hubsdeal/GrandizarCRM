using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class CreateOrEditBusinessJobMapDto : EntityDto<long?>
    {

        public long BusinessId { get; set; }

        public long JobId { get; set; }

    }
}