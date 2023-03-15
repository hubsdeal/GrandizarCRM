using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.CRM.Dtos
{
    public class GetContactForEditOutput
    {
        public CreateOrEditContactDto Contact { get; set; }

        public string UserName { get; set; }

        public string CountryName { get; set; }

        public string StateName { get; set; }

        public string MembershipTypeName { get; set; }

    }
}