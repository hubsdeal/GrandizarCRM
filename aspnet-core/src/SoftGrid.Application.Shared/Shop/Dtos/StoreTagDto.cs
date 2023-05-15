﻿using System;
using Abp.Application.Services.Dto;

namespace SoftGrid.Shop.Dtos
{
    public class StoreTagDto : EntityDto<long>
    {
        public string CustomTag { get; set; }

        public string TagValue { get; set; }

        public bool Verified { get; set; }

        public int? Sequence { get; set; }

        public long StoreId { get; set; }

        public long? MasterTagCategoryId { get; set; }

        public long? MasterTagId { get; set; }

        public bool? Published { get; set; }

    }
}