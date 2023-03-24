using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Shop.Dtos
{
    public class GetStoreOwnerTeamForEditOutput
    {
        public CreateOrEditStoreOwnerTeamDto StoreOwnerTeam { get; set; }

        public string StoreName { get; set; }

        public string UserName { get; set; }

    }
}