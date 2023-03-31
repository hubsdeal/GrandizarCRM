using SoftGrid.Shop;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using SoftGrid.LookupData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace SoftGrid.Shop
{
    [Table("ProductWholeSalePrices")]
    public class ProductWholeSalePrice : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual double? Price { get; set; }

        public virtual double? ExactQuantity { get; set; }

        public virtual string PackageInfo { get; set; }

        public virtual int? PackageQuantity { get; set; }

        [StringLength(ProductWholeSalePriceConsts.MaxWholeSaleSkuIdLength, MinimumLength = ProductWholeSalePriceConsts.MinWholeSaleSkuIdLength)]
        public virtual string WholeSaleSkuId { get; set; }

        public virtual long? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

        public virtual long? ProductWholeSaleQuantityTypeId { get; set; }

        [ForeignKey("ProductWholeSaleQuantityTypeId")]
        public ProductWholeSaleQuantityType ProductWholeSaleQuantityTypeFk { get; set; }

        public virtual long? MeasurementUnitId { get; set; }

        [ForeignKey("MeasurementUnitId")]
        public MeasurementUnit MeasurementUnitFk { get; set; }

        public virtual long? CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public Currency CurrencyFk { get; set; }

    }
}