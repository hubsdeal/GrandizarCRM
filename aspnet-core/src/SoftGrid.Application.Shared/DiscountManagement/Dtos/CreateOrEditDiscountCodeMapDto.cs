using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.DiscountManagement.Dtos
{
    public class CreateOrEditDiscountCodeMapDto : EntityDto<long?>
    {

        public long? DiscountCodeGeneratorId { get; set; }

        public long? StoreId { get; set; }

        public long? ProductId { get; set; }

        public long? MembershipTypeId { get; set; }

    }
}