using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace SoftGrid.LookupData.Dtos
{
    public class GetMembershipTypeForEditOutput
    {
        public CreateOrEditMembershipTypeDto MembershipType { get; set; }

    }
}