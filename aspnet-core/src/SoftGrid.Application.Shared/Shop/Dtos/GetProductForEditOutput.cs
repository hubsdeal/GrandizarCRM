using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SoftGrid.Shop.Dtos
{
    public class GetProductForEditOutput
    {
        public CreateOrEditProductDto Product { get; set; }

        public string ProductCategoryName { get; set; }

        public string MediaLibraryName { get; set; }

        public string MeasurementUnitName { get; set; }

        public string CurrencyName { get; set; }

        public string RatingLikeName { get; set; }

        public string ContactFullName { get; set; }

        public string StoreName { get; set; }

        public string ProductCategoryTreeViewName { get; set; }
        public long? ProductCategoryParentId { get; set; }
        public string Picture { get; set; }
        public double? RatingScore { get; set; }
        public int NumberOfRatings { get; set; }
        public long? StoreId { get; set; }
        public List<string> StoreTags { get; set; }
        public List<AdditionalCategoryForViewDto> AdditionalCategories { get; set; }
        public List<ProductDashboardTeam> Teams { get; set; }
        public int NumberOfTasks { get; set; }
        public int NumberOfNotes { get; set; }

        public List<string> PickupOrDeliveryTags { get; set; }

        public double? MembershipPrice { get; set; }

        public string MembershipName { get; set; }

        public GetProductForEditOutput()
        {
            StoreTags = new List<string>();
            PickupOrDeliveryTags = new List<string>();
            AdditionalCategories = new List<AdditionalCategoryForViewDto>();
            Teams = new List<ProductDashboardTeam>();
        }

    }

    public class AdditionalCategoryForViewDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class ProductDashboardTeam
    {
        public long Id { get; set; }
        public string EmployeeName { get; set; }
        public string Picture { get; set; }
        public bool IsPrimary { get; set; }
    }
}