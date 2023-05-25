using SoftGrid.Shop.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoftGrid.PublicCommon.Dtos
{
    public class GetPublicStoreForViewDto: GetStoreForViewDto
    {
        public decimal? RatingScore { get; set; }
        public string Logo { get; set; }

        public int? NumberOfReviews { get; set; }
        public List<GetStoreMediaForViewDto> StoreMedias { get; set; }
        public List<string> StoreTags { get; set; }
        public string PrimaryCategoryName { get; set; }

        public GetPublicStoreForViewDto()
        {
            StoreMedias = new List<GetStoreMediaForViewDto>();
        }
    }

    public class GetStoreForAppViewDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
    }
}
