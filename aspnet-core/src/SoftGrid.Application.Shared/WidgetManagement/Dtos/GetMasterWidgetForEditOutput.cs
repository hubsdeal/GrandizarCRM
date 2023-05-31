using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class GetMasterWidgetForEditOutput
    {
        public CreateOrEditMasterWidgetDto MasterWidget { get; set; }

    }
}