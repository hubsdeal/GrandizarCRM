using Abp.Application.Services.Dto;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftGrid.WidgetManagement.Dtos
{
    public class HubWidgetStoreMapDto : EntityDto<long>
    {
        public int? DisplaySequence { get; set; }

        public long HubWidgetMapId { get; set; }

        public long StoreId { get; set; }

    }


    /// <summary>
    /// Hub Store Widget Map Json View Dto | Widget
    /// </summary>
    public class HwsMapWidgetJsonViewDto
    {
        public long? Id { get; set; }
        public long? TenantId { get; set; }
        public long? HubId { get; set; }
        public int? DisplaySequence { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DesignCode { get; set; }
        public bool Publish { get; set; }
        public int? InternalDisplayNumber { get; set; }
        public Guid? ThumbnailImageId { get; set; }

        public List<Task<HwsStoreJsonViewDto>> Stores { get; set; }
        public List<HwsProductJsonViewDto> Products { get; set; }
        public List<HwsContentJsonViewDto> Contents { get; set; }

    }

    /// <summary>
    /// Hub Store Widget Map Json View Dto | Store
    /// </summary>
    public class HwsStoreJsonViewDto
    {
        public long? Id { get; set; }
        public long? WidgetId { get; set; }
        public long? HubId { get; set; }
        public string Name { get; set; }
        public int? TenantId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public long? CountryId { get; set; }
        public long? StateId { get; set; }
        public long? StoreCategoryId { get; set; }
        public string CountryName { get; set; }
        public string CountryPhoneCode { get; set; }
        public string CountryPhoneFlagIcon { get; set; }

        public string Description { get; set; }



        public object Country { get; set; }
        public object State { get; set; }
        public object StoreCategory { get; set; }
        public bool? IsVerified { get; set; }
        public int? DisplaySequence { get; set; }
    }



    /// <summary>
    /// Hub Store Widget Map Json View Dto | Store
    /// </summary>
    public class HwsProductJsonViewDto
    {
        public long? Id { get; set; }
        public long? WidgetId { get; set; }
        public long? HubId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? TenantId { get; set; }
        public int? Score { get; set; }
        public long? StoreId { get; set; }
        public long? RatingLikeId { get; set; }
        public int? RatingLikeScore { get; set; }
        public string RatingLikeName { get; set; }
        public long? ContactId { get; set; }
        public int? DisplaySequence { get; set; }
    }


    /// <summary>
    /// Hub Store Widget Map Json View Dto | Store
    /// </summary>
    public class HwsContentJsonViewDto
    {
        public long? Id { get; set; }
        public long? WidgetId { get; set; }
        public long? HubId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int? TenantId { get; set; }
        public int ContentTypeId { get; set; }
        public string PublishTime { get; set; }
        public bool? Published { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoUrl { get; set; }
        public long? BannerMediaLibraryId { get; set; }
        public string BannerMediaLibraryName { get; set; }
        public string BannerMediaLibraryAltTag { get; set; }
        public Guid? BannerMediaLibraryBinaryObjectId { get; set; }
        public string BannerMediaLibraryBinaryVirtualPath { get; set; }
    }
}