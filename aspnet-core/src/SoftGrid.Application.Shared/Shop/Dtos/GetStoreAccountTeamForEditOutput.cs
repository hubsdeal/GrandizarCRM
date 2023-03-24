using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreAccountTeamForEditOutput
    {
        public CreateOrEditStoreAccountTeamDto StoreAccountTeam { get; set; }

        public string StoreName { get; set; }

        public string EmployeeName { get; set; }

    }
}