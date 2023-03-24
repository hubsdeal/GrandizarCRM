using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.CRM.Dtos
{
    public class BusinessUserDto : EntityDto<long>
    {

        public long BusinessId { get; set; }

        public long UserId { get; set; }

    }
}