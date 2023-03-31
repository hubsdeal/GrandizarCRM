using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class CreateOrEditProductReturnInfoDto : EntityDto<long?>
    {

        public string CustomerNote { get; set; }

        public string AdminNote { get; set; }

        public long ProductId { get; set; }

        public long? ReturnTypeId { get; set; }

        public long? ReturnStatusId { get; set; }

    }
}