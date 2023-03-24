using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class CreateOrEditBusinessStoreMapDto : EntityDto<long?>
    {

        public long BusinessId { get; set; }

        public long StoreId { get; set; }

    }
}