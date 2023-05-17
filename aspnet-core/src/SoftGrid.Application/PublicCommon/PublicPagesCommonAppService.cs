using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using SoftGrid.Authorization.Users;
using SoftGrid.CMS;
using SoftGrid.DiscountManagement;
using SoftGrid.EntityFrameworkCore.Repositories;
using SoftGrid.LookupData;
using SoftGrid.Net.Sms;
using SoftGrid.Notifications;
using SoftGrid.OrderManagement;
using SoftGrid.PublicCommon.Dtos;
using SoftGrid.Shop;
using SoftGrid.Storage;
using SoftGrid.Territory;
using SoftGrid.Url;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using Product = SoftGrid.Shop.Product;

namespace SoftGrid.PublicCommon
{
    // [AbpAllowAnonymous]
    public class PublicPagesCommonAppService : SoftGridAppServiceBase, IPublicPagesCommonAppService
    {
        private readonly IRepository<ProductCategory, long> _productCategoryRepository;
        private readonly IRepository<MediaLibrary, long> _lookup_mediaLibraryRepository;
        private readonly IBinaryObjectManager _binaryObjectManager;
        //private readonly IRepository<StoreDynamicWidgetMap, long> _storeDynamicWidgetMapRepository;
        //private readonly IRepository<StoreSliderWidgetPhotoMap, long> _storeSliderWidgetPhotoMapRepository;
        //private readonly IRepository<StoreTopMessage, long> _storeTopMessageRepository;
        //private readonly IRepository<StoreWidgetProductMap, long> _storeWidgetProductMapRepository;
        private readonly IRepository<Product, long> _productRepository;
        private readonly IRepository<LookupData.MeasurementUnit, long> _measurementUnitRepository;
        private readonly IRepository<Currency, long> _currencyRepository;
        private readonly IRepository<RatingLike, long> _ratingLikeRepository;
        private readonly IRepository<Store, long> _storeRepository;
        private readonly IRepository<ProductMedia, long> _productMediaRepository;
        //private readonly IRepository<ProductRelatedProduct, long> _productRelatedProductRepository;
        //private readonly IRepository<CrossSellProduct, long> _crossSellProductRepository;
        public readonly IRepository<StoreRelevantStore, long> _storeRelevantStoreRepository;
        //private readonly IRepository<StoreWidgetProductCategoryMap, long> _storeWidgetProductCategoryMapRepository;
        //private readonly IRepository<CustomerSubscriptionList, long> _customerSubscriptionListRepository;
        private readonly IStoredProcedureRepository _storedProcedureRepository;
        private readonly IRepository<ProductReview, long> _productReviewRepository;
        //private readonly IRepository<StoreWidgetBrandManufacturerMap, long> _storeWidgetBrandManufacturerMapRepository;
        //private readonly IRepository<BrandManufacturer, long> _brandManufacturerRepository;
        private readonly IRepository<State, long> _stateRepository;
        private readonly IRepository<Country, long> _countryRepository;
        private readonly IRepository<City, long> _cityRepository;
        private readonly IRepository<StoreReview, long> _storeReviewRepository;
        private readonly IRepository<StoreMedia, long> _storeMediaRepository;
        private readonly IRepository<StoreTag, long> _storeTagRepository;
        private readonly IRepository<MasterTagCategory, long> _masterTagCategoryRepository;
        private readonly IRepository<MasterTag, long> _masterTagRepository;
        //private readonly IRepository<StoreHour, long> _storeHourRepository;
        private readonly IRepository<StoreProductMap, long> _storeProductMapRepository;
        //private readonly IRepository<StoreCategoryMap, long> _storeCategoryMapRepository;
        //private SendGridEmailSenderConfiguration _sendGridEmailSenderConfiguration;
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;
        //private readonly IRepository<Shop.Order, long> _orderRepository;
        private readonly IRepository<OrderProductInfo, long> _orderProductInfoRepository;
        private readonly IRepository<OrderPaymentInfo, long> _orderPaymentInfoRepository;
        private readonly IRepository<PaymentType, long> _paymentTypeRepository;
        //private readonly IRepository<StoreWidgetHubMap, long> _storeWidgetHubMapRepository;
        private readonly IRepository<Hub, long> _hubRepository;
        //private readonly IRepository<HubsProductCategory, long> _hubsProductCategoryRepository;
        //private readonly IRepository<HubsProduct, long> _hubsProductRepository;
        //private readonly IRepository<HubsStore, long> _hubsStoreRepository;
        //private readonly IRepository<HubsBrand, long> _hubsBrandRepository;
        //private readonly ICustomEmailSender _emailSender;
        private readonly IUserEmailer _userEmailer;
        private readonly ISmsSender _smsSender;
        //private readonly IRepository<ProductTeam, long> _productTeamRepository;
        private readonly IRepository<CRM.Employee, long> _employeeRepository;
        private readonly IRepository<ProductByVariant, long> _productByVariantRepository;
        //private readonly IRepository<VariantCategory, long> _variantCategoryRepository;
        //private readonly IRepository<VariantType, long> _variantTypeRepository;
        private readonly IRepository<OrderProductVariant, long> _orderProductVariantRepository;
        private readonly IRepository<OrderFulfillmentStatus, long> _orderFulfillmentStatusRepository;
        //private readonly IRepository<RewardPointHistory, long> _rewardPointHistoryRepository;
        private readonly IRepository<Content, long> _contentRepository;
        private readonly IRepository<ProductFlashSaleProductMap, long> _productFlashSaleProductMapRepository;
        //private readonly IRepository<TaxRate, long> _taxRateRepository;
        //private readonly IRepository<OrderStatusType, long> _orderStatusTypeRepository;
        //private readonly IRepository<OrderPickupStatusType, long> _orderPickupStatusTypeRepository;
        public IAppUrlService AppUrlService { get; set; }
        private readonly IRepository<ProductTag, long> _productTagRepository;
        private readonly IRepository<ProductPackage, long> _productPackageRepository;
        //private readonly IRepository<MembershipAndProductMap, long> _membershipAndProductMapRepository;
        private readonly IRepository<CustomerWallet, long> _customerWalletRepository;
        //private readonly IRepository<WalletTransaction, long> _walletTransactionRepository;
        private readonly IRepository<ProductFaq, long> _productFaqRepository;
        //private readonly IRepository<DeliveryType, int> _deliveryTypeRepository;
        //private readonly IRepository<StoreDeliveryTypeMap, long> _storeDeliveryTypeMapRepository;
        private readonly IAppNotifier _appNotifier;
        private readonly IRepository<StoreZipCodeMap, long> _storeZipCodeMapRepository;
        private readonly IRepository<StoreLocation, long> _storeLocationRepository;
        //private readonly IRepository<ReservationTimeSlot, long> _reservationTimeSlotRepository;
        //private readonly IRepository<Reservation,long> _reservationRepository;
        public PublicPagesCommonAppService(
            IRepository<ProductCategory, long> productCategoryRepository,
            IRepository<MediaLibrary, long> lookup_mediaLibraryRepository,
            IBinaryObjectManager binaryObjectManager,
            //IRepository<StoreDynamicWidgetMap, long> storeDynamicWidgetMapRepository,
            //IRepository<StoreSliderWidgetPhotoMap, long> storeSliderWidgetPhotoMapRepository,
            //IRepository<StoreTopMessage, long> storeTopMessageRepository,
            //IRepository<StoreWidgetProductMap, long> storeWidgetProductMapRepository,
            IRepository<Product, long> productRepository,
            IRepository<LookupData.MeasurementUnit, long> measurementUnitRepository,
            IRepository<Currency, long> currencyRepository,
            IRepository<RatingLike, long> ratingLikeRepository,
            IRepository<Store, long> storeRepository,
            IRepository<ProductMedia, long> productMediaRepository,
            //IRepository<ProductRelatedProduct, long> productRelatedProductRepository,
            //IRepository<CrossSellProduct, long> crossSellProductRepository,
            IRepository<StoreRelevantStore, long> storeRelevantStoreRepository,
            //IRepository<StoreWidgetProductCategoryMap, long> storeWidgetProductCategoryMapRepository,
            //IRepository<CustomerSubscriptionList, long> customerSubscriptionListRepository,
            IStoredProcedureRepository storedProcedureRepository,
            IRepository<ProductReview, long> productReviewRepository,
            //IRepository<StoreWidgetBrandManufacturerMap, long> storeWidgetBrandManufacturerMapRepository,
            //IRepository<BrandManufacturer, long> brandManufacturerRepository,
            IRepository<State, long> stateRepository,
            IRepository<Country, long> countryRepository,
            IRepository<StoreReview, long> storeReviewRepository,
            IRepository<StoreMedia, long> storeMediaRepository,
            IRepository<StoreTag, long> storeTagRepository,
            IRepository<MasterTagCategory, long> masterTagCategoryRepository,
            IRepository<MasterTag, long> masterTagRepository,
            //IRepository<StoreHour, long> storeHourRepository,
            IRepository<StoreProductMap, long> storeProductMapRepository,
            //IRepository<StoreCategoryMap, long> storeCategoryMapRepository,
            //ICustomEmailSender emailSender,
            //SendGridEmailSenderConfiguration sendGridEmailSenderConfiguration,
            IWebHostEnvironment env,
            //IRepository<Shop.Order, long> orderRepository,
            IRepository<OrderProductInfo, long> orderProductInfoRepository,
            IRepository<OrderPaymentInfo, long> orderPaymentInfoRepository,
            IRepository<PaymentType, long> paymentTypeRepository,
            //IRepository<StoreWidgetHubMap, long> storeWidgetHubMapRepository,
            IRepository<Hub, long> hubRepository,
            //IRepository<HubsProductCategory, long> hubsProductCategoryRepository,
            //IRepository<HubsProduct, long> hubsProductRepository,
            //IRepository<HubsStore, long> hubsStoreRepository,
            //IRepository<HubsBrand, long> hubsBrandRepository,
            IRepository<City, long> cityRepository,
            IUserEmailer userEmailer,
            ISmsSender smsSender,
            //IRepository<ProductTeam, long> productTeamRepository,
            IRepository<CRM.Employee, long> employeeRepository,
            IRepository<ProductByVariant, long> productByVariantRepository,
            //IRepository<VariantCategory, long> variantCategoryRepository,
            //IRepository<VariantType, long> variantTypeRepository,
            IRepository<OrderProductVariant, long> orderProductVariantRepository,
            IRepository<OrderFulfillmentStatus, long> orderFulfillmentStatusRepository,
            //IRepository<RewardPointHistory, long> rewardPointHistoryRepository,
            IRepository<ProductTag, long> productTagRepository,
            IRepository<Content, long> contentRepository,
            IRepository<ProductFlashSaleProductMap, long> productFlashSaleProductMapRepository,
            //IRepository<TaxRate, long> taxRateRepository,
            //IRepository<OrderStatusType, long> orderStatusTypeRepository,
            //IRepository<OrderPickupStatusType, long> orderPickupStatusTypeRepository,
            IRepository<ProductPackage, long> productPackageRepository,
            //IRepository<MembershipAndProductMap, long> membershipAndProductMapRepository,
            IRepository<CustomerWallet, long> customerWalletRepository,
            //IRepository<WalletTransaction, long> walletTransactionRepository,
            IRepository<ProductFaq, long> productFaqRepository,
            IAppNotifier appNotifier,
            //IRepository<DeliveryType, int> deliveryTypeRepository,
            //IRepository<StoreDeliveryTypeMap, long> storeDeliveryTypeMapRepository,
            IRepository<StoreZipCodeMap, long> storeZipCodeMapRepository,
            IRepository<StoreLocation, long> storeLocationRepository
            //IRepository<ReservationTimeSlot, long> reservationTimeSlotRepository,
            //IRepository<Reservation, long> reservationRepository
            )
        {
            _productCategoryRepository = productCategoryRepository;
            _lookup_mediaLibraryRepository = lookup_mediaLibraryRepository;
            _binaryObjectManager = binaryObjectManager;
            //_storeDynamicWidgetMapRepository = storeDynamicWidgetMapRepository;
            //_storeSliderWidgetPhotoMapRepository = storeSliderWidgetPhotoMapRepository;
            //_storeTopMessageRepository = storeTopMessageRepository;
            //_storeWidgetProductMapRepository = storeWidgetProductMapRepository;
            _productRepository = productRepository;
            _measurementUnitRepository = measurementUnitRepository;
            _currencyRepository = currencyRepository;
            _ratingLikeRepository = ratingLikeRepository;
            _storeRepository = storeRepository;
            _productMediaRepository = productMediaRepository;
            //_productRelatedProductRepository = productRelatedProductRepository;
            //_crossSellProductRepository = crossSellProductRepository;
            _storeRelevantStoreRepository = storeRelevantStoreRepository;
            //_storeWidgetProductCategoryMapRepository = storeWidgetProductCategoryMapRepository;
            //_customerSubscriptionListRepository = customerSubscriptionListRepository;
            _storedProcedureRepository = storedProcedureRepository;
            _productReviewRepository = productReviewRepository;
            //_storeWidgetBrandManufacturerMapRepository = storeWidgetBrandManufacturerMapRepository;
            //_brandManufacturerRepository = brandManufacturerRepository;
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
            _storeReviewRepository = storeReviewRepository;
            _storeMediaRepository = storeMediaRepository;
            _storeTagRepository = storeTagRepository;
            _masterTagCategoryRepository = masterTagCategoryRepository;
            _masterTagRepository = masterTagRepository;
            //_storeHourRepository = storeHourRepository;
            //_storeProductMapRepository = storeProductMapRepository;
            //_storeCategoryMapRepository = storeCategoryMapRepository;
            //_emailSender = emailSender;
            //_sendGridEmailSenderConfiguration = sendGridEmailSenderConfiguration;
            _env = env;
            //_appConfiguration = env.GetAppConfiguration();
            //_orderRepository = orderRepository;
            //_orderProductInfoRepository = orderProductInfoRepository;
            //_orderPaymentInfoRepository = orderPaymentInfoRepository;
            //_paymentTypeRepository = paymentTypeRepository;
            //_storeWidgetHubMapRepository = storeWidgetHubMapRepository;
            //_hubRepository = hubRepository;
            //_hubsProductCategoryRepository = hubsProductCategoryRepository;
            //_hubsProductRepository = hubsProductRepository;
            //_hubsStoreRepository = hubsStoreRepository;
            //_hubsBrandRepository = hubsBrandRepository;
            //_cityRepository = cityRepository;
            //_userEmailer = userEmailer;
            //_smsSender = smsSender;
            //AppUrlService = NullAppUrlService.Instance;
            //_productTeamRepository = productTeamRepository;
            //_employeeRepository = employeeRepository;
            //_productByVariantRepository = productByVariantRepository;
            //_variantCategoryRepository = variantCategoryRepository;
            //_variantTypeRepository = variantTypeRepository;
            //_orderProductVariantRepository = orderProductVariantRepository;
            //_orderFulfillmentStatusRepository = orderFulfillmentStatusRepository;
            //_rewardPointHistoryRepository = rewardPointHistoryRepository;
            //_productTagRepository = productTagRepository;
            _contentRepository = contentRepository;
            //_productFlashSaleProductMapRepository = productFlashSaleProductMapRepository;
            //_taxRateRepository = taxRateRepository;
            //_orderStatusTypeRepository = orderStatusTypeRepository;
            //_orderPickupStatusTypeRepository = orderPickupStatusTypeRepository;
            //_productPackageRepository = productPackageRepository;
            //_membershipAndProductMapRepository = membershipAndProductMapRepository;
            //_customerWalletRepository = customerWalletRepository;
            //_walletTransactionRepository = walletTransactionRepository;
            //_productFaqRepository = productFaqRepository;
            //_appNotifier = appNotifier;
            //_deliveryTypeRepository = deliveryTypeRepository;
            //_storeDeliveryTypeMapRepository = storeDeliveryTypeMapRepository;
            //_storeZipCodeMapRepository = storeZipCodeMapRepository;
            //_storeLocationRepository = storeLocationRepository;
            //_reservationTimeSlotRepository = reservationTimeSlotRepository;
            //_reservationRepository = reservationRepository;
        }

        //[AbpAllowAnonymous]
        //public async Task<List<ProductCategoryTreeNode>> GetAllByParentChildForTreeView()
        //{

        //    var productCategories = _productCategoryRepository.GetAll().Where(e => e.Published == true).OrderBy(e => e.DisplaySequence).ToList();

        //    var parentList = productCategories.Where(x => x.HasParentCategory == false).ToList();

        //    List<ProductCategoryTreeNode> productCategoryTreeNodes = new List<ProductCategoryTreeNode>();

        //    foreach (var parent in parentList)
        //    {
        //        ProductCategoryTreeNode productCategoryTreeNode = new ProductCategoryTreeNode();
        //        Data data = ObjectMapper.Map<Data>(parent);
        //        if (data.MediaLibraryId != null)
        //        {
        //            var mediaLibrary = _lookup_mediaLibraryRepository.Get((long)data.MediaLibraryId);
        //            if (mediaLibrary.BinaryObjectId != null && mediaLibrary.BinaryObjectId != Guid.Empty)
        //            {
        //                //var binaryObject = await _binaryObjectManager.GetOrNullAsync(mediaLibrary.BinaryObjectId);
        //                //data.Picture = Convert.ToBase64String(binaryObject.Bytes);
        //                data.Picture = await _binaryObjectManager.GetProductPictureUrlAsync(mediaLibrary.BinaryObjectId, ".png");

        //            }
        //        }

        //        productCategoryTreeNode.Data = data;
        //        productCategoryTreeNode.Children = await GetChildCategory(parent, productCategories);
        //        productCategoryTreeNode.Leaf = true;
        //        if (productCategoryTreeNode.Children != null && productCategoryTreeNode.Children.Count > 0)
        //        {
        //            productCategoryTreeNode.Expanded = true;
        //        }

        //        productCategoryTreeNodes.Add(productCategoryTreeNode);

        //    }

        //    return productCategoryTreeNodes;

        //}

        //private async Task<List<ProductCategoryTreeNode>> GetChildCategory(ProductCategory productCategory, List<ProductCategory> productCategories)
        //{

        //    List<ProductCategoryTreeNode> productCategoryTreeNodes = new List<ProductCategoryTreeNode>();

        //    var childList = productCategories.Where(x => x.ParentCategoryId == productCategory.Id).OrderBy(e => e.DisplaySequence).ToList();

        //    foreach (var child in childList)
        //    {
        //        ProductCategoryTreeNode productCategoryTreeNode = new ProductCategoryTreeNode();

        //        Data data = ObjectMapper.Map<Data>(child); ;
        //        if (data.MediaLibraryId != null)
        //        {
        //            var mediaLibrary = _lookup_mediaLibraryRepository.Get((long)data.MediaLibraryId);
        //            if (mediaLibrary.BinaryObjectId != null && mediaLibrary.BinaryObjectId != Guid.Empty)
        //            {
        //                //var binaryObject = await _binaryObjectManager.GetOrNullAsync(mediaLibrary.BinaryObjectId);
        //                //data.Picture = Convert.ToBase64String(binaryObject.Bytes);
        //                data.Picture = await _binaryObjectManager.GetProductPictureUrlAsync(mediaLibrary.BinaryObjectId, ".png");
        //            }
        //        }

        //        productCategoryTreeNode.Data = data;
        //        productCategoryTreeNode.Children = await GetChildCategory(child, productCategories);
        //        productCategoryTreeNode.Leaf = true;
        //        if (productCategoryTreeNode.Children != null && productCategoryTreeNode.Children.Count > 0)
        //        {
        //            productCategoryTreeNode.Expanded = true;
        //        }

        //        productCategoryTreeNodes.Add(productCategoryTreeNode);

        //    }
        //    return productCategoryTreeNodes;
        //}
        //[AbpAllowAnonymous]
        //public async Task<List<GetStoreSliderWidgetPhotoMapForViewDto>> GetSliderPhoto(long storeId, long storeWidgetId)
        //{
        //    var storeDynamicWidgetMap = await _storeDynamicWidgetMapRepository.FirstOrDefaultAsync(e => e.StoreId == storeId && e.StoreWidgetId == storeWidgetId);
        //    var sliderList = new List<GetStoreSliderWidgetPhotoMapForViewDto>();
        //    if (storeDynamicWidgetMap != null)
        //    {
        //        var sliders = _storeSliderWidgetPhotoMapRepository.GetAll().Where(e => e.StoreDynamicWidgetMapId == storeDynamicWidgetMap.Id).OrderBy(e => e.DisplaySequence);
        //        foreach (var slider in sliders)
        //        {
        //            string picture = null;
        //            if (slider.PhotoBinaryObjectId != null && slider.PhotoBinaryObjectId != Guid.Empty)
        //            {
        //                //var binaryObject = await _binaryObjectManager.GetOrNullAsync(slider.PhotoBinaryObjectId);
        //                //picture = Convert.ToBase64String(binaryObject.Bytes);
        //                picture = await _binaryObjectManager.GetOthersPictureUrlAsync(slider.PhotoBinaryObjectId, ".png");
        //            }
        //            sliderList.Add(new GetStoreSliderWidgetPhotoMapForViewDto { StoreSliderWidgetPhotoMap = ObjectMapper.Map<StoreSliderWidgetPhotoMapDto>(slider), Picture = picture });
        //        }
        //    }
        //    return sliderList;
        //}
        //[AbpAllowAnonymous]
        //public async Task<GetStoreTopMessageForViewDto> GetStoreTopMessageByStoreId(long storeId)
        //{

        //    var topMessage = await _storeTopMessageRepository.FirstOrDefaultAsync(e => e.StoreId == storeId);
        //    var output = new GetStoreTopMessageForViewDto();
        //    if (topMessage != null)
        //    {
        //        output = new GetStoreTopMessageForViewDto { StoreTopMessage = ObjectMapper.Map<StoreTopMessageDto>(topMessage) };
        //    }

        //    return output;
        //}
        //[AbpAllowAnonymous]
        //public async Task<GetPublicProductWidgetWithProductsForViewDto> GetWidgetProductsByStore(long storeId, long storeWidgetId)
        //{
        //    var output = new GetPublicProductWidgetWithProductsForViewDto();
        //    var storeDynamicWidgetMap = await _storeDynamicWidgetMapRepository.FirstOrDefaultAsync(e => e.StoreId == storeId && e.StoreWidgetId == storeWidgetId);
        //    output.StoreDynamicWidgetMap = ObjectMapper.Map<StoreDynamicWidgetMapDto>(storeDynamicWidgetMap);

        //    var productIds = storeDynamicWidgetMap != null ? _storeWidgetProductMapRepository.GetAll().Where(e => e.StoreDynamicWidgetMapId == storeDynamicWidgetMap.Id).OrderBy(e => e.DisplaySequence).Select(e => e.ProductId) : null;

        //    var filteredProducts = _productRepository.GetAll()
        //                .Include(e => e.ProductCategoryFk)
        //                .Include(e => e.MediaLibraryFk)
        //                .Include(e => e.MeasurementUnitFk)
        //                .Include(e => e.CurrencyFk)
        //                .Include(e => e.RatingLikeFk)
        //                .WhereIf(productIds != null, e => productIds.Contains(e.Id));

        //    var pagedAndFilteredProducts = filteredProducts;

        //    var products = from o in pagedAndFilteredProducts
        //                   join o1 in _productCategoryRepository.GetAll() on o.ProductCategoryId equals o1.Id into j1
        //                   from s1 in j1.DefaultIfEmpty()

        //                   join o2 in _lookup_mediaLibraryRepository.GetAll() on o.MediaLibraryId equals o2.Id into j2
        //                   from s2 in j2.DefaultIfEmpty()

        //                   join o3 in _measurementUnitRepository.GetAll() on o.MeasurementUnitId equals o3.Id into j3
        //                   from s3 in j3.DefaultIfEmpty()

        //                   join o4 in _currencyRepository.GetAll() on o.CurrencyId equals o4.Id into j4
        //                   from s4 in j4.DefaultIfEmpty()

        //                   join o5 in _ratingLikeRepository.GetAll() on o.RatingLikeId equals o5.Id into j5
        //                   from s5 in j5.DefaultIfEmpty()

        //                   select new GetPublicProductForViewDto()
        //                   {
        //                       Product = new ProductDto
        //                       {
        //                           Name = o.Name,
        //                           ShortDescription = o.ShortDescription,
        //                           Description = o.Description,
        //                           Sku = o.Sku,
        //                           Url = o.Url,
        //                           SeoTitle = o.SeoTitle,
        //                           MetaKeywords = o.MetaKeywords,
        //                           MetaDescription = o.MetaDescription,
        //                           RegularPrice = o.RegularPrice,
        //                           SalesPrice = o.SalesPrice,
        //                           PriceDiscountPercentage = o.PriceDiscountPercentage,
        //                           CallForPrice = o.CallForPrice,
        //                           UnitPrice = o.UnitPrice,
        //                           MeasureAmount = o.MeasureAmount,
        //                           IsTaxExempt = o.IsTaxExempt,
        //                           StockQuantity = o.StockQuantity,
        //                           IsDisplayStockQuantity = o.IsDisplayStockQuantity,
        //                           IsPublished = o.IsPublished,
        //                           IsPackageProduct = o.IsPackageProduct,
        //                           InternalNotes = o.InternalNotes,
        //                           Icon = o.Icon,
        //                           Id = o.Id,
        //                           IsWholeSaleProduct = o.IsWholeSaleProduct,
        //                           MediaLibraryId = o.MediaLibraryId,
        //                           ProductCategoryId = o.ProductCategoryId,
        //                           RatingLikeId = o.RatingLikeId
        //                       },
        //                       ProductCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
        //                       MediaLibraryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
        //                       MeasurementUnitName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
        //                       CurrencyName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
        //                       CurrencyIcon = s4 == null || s4.Icon == null ? "$" : s4.Icon.ToString(),
        //                       RatingLikeName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
        //                       RatingScore = s5 == null || s5.Score == null ? 0 : s5.Score
        //                   };

        //    var results = products.ToList();

        //    foreach (var result in results)
        //    {
        //        if (result.Product.MediaLibraryId != null)
        //        {
        //            var media = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync(e => e.Id == result.Product.MediaLibraryId);
        //            if (media.BinaryObjectId != null)
        //            {
        //                //var binaryObject = await _binaryObjectManager.GetOrNullAsync(media.BinaryObjectId);
        //                //result.Picture= Convert.ToBase64String(binaryObject.Bytes);
        //                result.Picture = await _binaryObjectManager.GetProductPictureUrlAsync(media.BinaryObjectId, ".png");
        //            }
        //        }

        //        result.HasVariant = await _productByVariantRepository.CountAsync(e => e.ProductId == result.Product.Id) > 0 ? true : false;
        //    }

        //    output.Products = results;

        //    return output;
        //}
        //[AbpAllowAnonymous]
        //public async Task<GetStoreForEditOutput> GetStoreDetails(EntityDto<long> input)
        //{
        //    var store = await _storeRepository.FirstOrDefaultAsync(input.Id);

        //    var output = new GetStoreForEditOutput { Store = ObjectMapper.Map<CreateOrEditStoreDto>(store) };

        //    return output;
        //}
        //[AbpAllowAnonymous]
        //public async Task<GetPublicProductForViewDto> GetProductDetails(string url)
        //{
        //    var output = new GetPublicProductForViewDto();
        //    var product = await _productRepository.FirstOrDefaultAsync(e => e.Url.Equals(url));
        //    if (product != null)
        //    {
        //        output.Product = ObjectMapper.Map<ProductDto>(product);
        //        var flasSales = _productFlashSaleProductMapRepository.GetAll().Where(e => e.ProductId == output.Product.Id && e.StartDate != null && e.StartTime != null && e.EndDate != null && e.EndTime != null);

        //        var currentDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm tt");

        //        foreach (var item in flasSales)
        //        {
        //            var newStartDate = item.StartDate.ToString().Split(' ')[0] + " " + item.StartTime;
        //            var newEndDate = item.EndDate.ToString().Split(' ')[0] + " " + item.EndTime;
        //            var newSDate = DateTime.Parse(newStartDate);
        //            var newEDate = DateTime.Parse(newEndDate);
        //            var newCDate = DateTime.Parse(currentDate);
        //            if (newSDate <= newCDate && newEDate >= newCDate)
        //            {
        //                output.FlashSaleRemainingDuration = (newEDate - newCDate).TotalSeconds;
        //                output.IsFlashSale = true;
        //                output.Product.SalesPrice = (decimal)item.FlashSalePrice;
        //                output.Product.PriceDiscountPercentage = item.DiscountPercentage;
        //                output.Product.PriceDiscountAmount = item.DiscountAmount;
        //                output.Product.StockQuantity = item.FlashSaleQuantity;
        //                break;
        //            }


        //        }

        //        if (output.Product.ProductCategoryId != null)
        //        {
        //            var _lookupProductCategory = await _productCategoryRepository.FirstOrDefaultAsync((long)output.Product.ProductCategoryId);
        //            output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
        //            output.ProductCategoryUrl = _lookupProductCategory?.Url?.ToString();
        //        }
        //        if (output.Product.MeasurementUnitId != null)
        //        {
        //            var _lookupMeasurementUnit = await _measurementUnitRepository.FirstOrDefaultAsync((long)output.Product.MeasurementUnitId);
        //            output.MeasurementUnitName = _lookupMeasurementUnit?.Name?.ToString();
        //        }

        //        if (output.Product.CurrencyId != null)
        //        {
        //            var _lookupCurrency = await _currencyRepository.FirstOrDefaultAsync((long)output.Product.CurrencyId);
        //            output.CurrencyName = _lookupCurrency?.Name?.ToString();
        //            output.CurrencyIcon = _lookupCurrency != null ? _lookupCurrency?.Icon?.ToString() : "$";
        //        }
        //        output.NumberOfRatings = await _productReviewRepository.CountAsync(e => e.ProductId == output.Product.Id && e.IsPublish == true);

        //        if (output.NumberOfRatings > 0)
        //        {
        //            output.RatingScore = (_productReviewRepository.GetAll().Where(e => e.ProductId == output.Product.Id && e.RatingLikeId != null).Select(e => e.RatingLikeId).Sum()) / output.NumberOfRatings;

        //        }
        //        else
        //        {
        //            output.RatingScore = 0;

        //        }

        //        if (output.Product.RatingLikeId != null)
        //        {
        //            var _lookupRatingLike = await _ratingLikeRepository.FirstOrDefaultAsync((long)output.Product.RatingLikeId);
        //            output.RatingLikeName = _lookupRatingLike?.Name?.ToString();
        //            //output.RatingScore = _lookupRatingLike?.Score;
        //        }

        //        if (output.Product.MediaLibraryId != null)
        //        {
        //            var media = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync(e => e.Id == output.Product.MediaLibraryId);
        //            if (media.BinaryObjectId != null && media.BinaryObjectId != Guid.Empty)
        //            {
        //                //var binaryObject = await _binaryObjectManager.GetOrNullAsync(media.BinaryObjectId);
        //                //output.Picture = Convert.ToBase64String(binaryObject.Bytes);
        //                output.Picture = await _binaryObjectManager.GetProductPictureUrlAsync(media.BinaryObjectId, ".png");
        //            }
        //        }

        //        var tags = _productTagRepository.GetAll().Include(e => e.MasterTagFk)
        //                           .Where(e => e.ProductId == output.Product.Id).OrderBy(e => e.DisplaySequenceNumber).Take(10);

        //        foreach (var tag in tags)
        //        {
        //            if (tag.MasterTagFk != null && tag.MasterTagFk.Name != null)
        //            {
        //                output.ProductTags.Add(tag.MasterTagFk.Name);
        //            }
        //            else if (tag.CustomTag != null)
        //            {
        //                output.ProductTags.Add(tag.CustomTag);
        //            }
        //        }

        //        var store = _storeProductMapRepository.GetAll().Include(e => e.StoreFk).Where(e => e.ProductId == output.Product.Id && e.StoreId != null).FirstOrDefault();

        //        if (store != null)
        //        {
        //            output.StoreId = store.StoreId;
        //            output.StoreName = store.StoreFk?.Name;
        //            var storeTags = _storeTagRepository.GetAll().Include(e => e.MasterTagFk).Where(e => e.StoreId == store.Id).Take(3).OrderBy(e => e.DisplaySequence);

        //            foreach (var tag in storeTags)
        //            {
        //                if (tag.MasterTagFk != null && tag.MasterTagFk.Name != null)
        //                {
        //                    output.StoreTags.Add(tag.MasterTagFk.Name);
        //                }
        //                else if (tag.CustomTag != null)
        //                {
        //                    output.StoreTags.Add(tag.CustomTag);
        //                }
        //            }
        //        }

        //        output.PickupOrDeliveryTags.Add(output.ProductCategoryName);

        //        //output.PickupOrDeliveryTags = _productTagRepository.GetAll().Include(e => e.MasterTagFk).Where(e => e.MasterTagCategoryId == 38 && e.ProductId == output.Product.Id && e.MasterTagId != null).Select(e => e.MasterTagFk.Name).ToList();
        //        output.PickupOrDeliveryTags = _productTagRepository.GetAll().Include(e => e.MasterTagFk).Where(e => e.MasterTagCategoryId == (long)ProductEnum.ProductAdditionalCategory && e.ProductId == output.Product.Id && e.MasterTagId != null).Select(e => e.MasterTagFk.Name).ToList();


        //        output.ProductMedias = await GetAllProductMediasByProductId(product.Id);
        //    }
        //    output.HasVariant = await _productByVariantRepository.CountAsync(e => e.ProductId == output.Product.Id) > 0 ? true : false;

        //    if (output.Product.IsPackageProduct)
        //    {
        //        var packageProducts = _productPackageRepository.GetAll().Include(e => e.PackageProductFk).Where(e => e.PrimaryProductId == output.Product.Id && e.PrimaryProductId != null && e.PackageProductId != null);

        //        foreach (var p in packageProducts)
        //        {
        //            var item = new GetPackageProductForPublicViewDto();
        //            item.ProductId = p.PackageProductId;
        //            item.Name = p.PackageProductFk.Name;
        //            item.Url = p.PackageProductFk.Url;
        //            item.Quantity = p.Quantity;
        //            if (p.PackageProductFk.MediaLibraryId != null)
        //            {
        //                var media = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync(e => e.Id == p.PackageProductFk.MediaLibraryId);
        //                if (media != null && media.BinaryObjectId != null && media.BinaryObjectId != Guid.Empty)
        //                {
        //                    item.Picture = await _binaryObjectManager.GetProductPictureUrlAsync(media.BinaryObjectId, ".png");
        //                }
        //            }
        //            output.ProductPackages.Add(item);
        //        }
        //    }

        //    output.HasFaq = await _productFaqRepository.CountAsync(e => e.ProductId == output.Product.Id && e.Publish == true) > 0 ? true : false;

        //    output.MembershipPrice = _membershipAndProductMapRepository.GetAll().Where(e => e.ProductId == output.Product.Id && e.MembershipTypeId == (long)MembershipTypeEnum.Snack_Pass).Select(e => e.Price).FirstOrDefault();
        //    output.MembershipName = _membershipAndProductMapRepository.GetAll().Include(e => e.MembershipTypeFk).Where(e => e.ProductId == output.Product.Id && e.MembershipTypeId == (long)MembershipTypeEnum.Snack_Pass).Select(e => e.MembershipTypeFk.Name).FirstOrDefault();

        //    return output;
        //}
        //[AbpAllowAnonymous]
        //public async Task<GetPublicProductForViewDto> GetProductDetailsById(long productId)
        //{
        //    var output = new GetPublicProductForViewDto();
        //    var product = await _productRepository.FirstOrDefaultAsync(e => e.Id == productId);
        //    if (product != null)
        //    {
        //        output.Product = ObjectMapper.Map<ProductDto>(product);
        //        var flasSales = _productFlashSaleProductMapRepository.GetAll().Where(e => e.ProductId == output.Product.Id && e.StartDate != null && e.StartTime != null && e.EndDate != null && e.EndTime != null);

        //        var currentDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm tt");

        //        foreach (var item in flasSales)
        //        {
        //            var newStartDate = item.StartDate.ToString().Split(' ')[0] + " " + item.StartTime;
        //            var newEndDate = item.EndDate.ToString().Split(' ')[0] + " " + item.EndTime;
        //            var newSDate = DateTime.Parse(newStartDate);
        //            var newEDate = DateTime.Parse(newEndDate);
        //            var newCDate = DateTime.Parse(currentDate);
        //            if (newSDate <= newCDate && newEDate >= newCDate)
        //            {
        //                output.FlashSaleRemainingDuration = (newEDate - newCDate).TotalSeconds;
        //                output.IsFlashSale = true;
        //                output.Product.SalesPrice = (decimal)item.FlashSalePrice;
        //                output.Product.PriceDiscountPercentage = item.DiscountPercentage;
        //                output.Product.PriceDiscountAmount = item.DiscountAmount;
        //                output.Product.StockQuantity = item.FlashSaleQuantity;
        //                break;
        //            }


        //        }

        //        if (output.Product.ProductCategoryId != null)
        //        {
        //            var _lookupProductCategory = await _productCategoryRepository.FirstOrDefaultAsync((long)output.Product.ProductCategoryId);
        //            output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
        //            output.ProductCategoryUrl = _lookupProductCategory?.Url?.ToString();
        //        }
        //        if (output.Product.MeasurementUnitId != null)
        //        {
        //            var _lookupMeasurementUnit = await _measurementUnitRepository.FirstOrDefaultAsync((long)output.Product.MeasurementUnitId);
        //            output.MeasurementUnitName = _lookupMeasurementUnit?.Name?.ToString();
        //        }

        //        if (output.Product.CurrencyId != null)
        //        {
        //            var _lookupCurrency = await _currencyRepository.FirstOrDefaultAsync((long)output.Product.CurrencyId);
        //            output.CurrencyName = _lookupCurrency?.Name?.ToString();
        //            output.CurrencyIcon = _lookupCurrency != null ? _lookupCurrency?.Icon?.ToString() : "$";
        //        }
        //        output.NumberOfRatings = await _productReviewRepository.CountAsync(e => e.ProductId == output.Product.Id && e.IsPublish == true);

        //        if (output.NumberOfRatings > 0)
        //        {
        //            output.RatingScore = (_productReviewRepository.GetAll().Where(e => e.ProductId == output.Product.Id && e.RatingLikeId != null).Select(e => e.RatingLikeId).Sum()) / output.NumberOfRatings;

        //        }
        //        else
        //        {
        //            output.RatingScore = 0;

        //        }

        //        if (output.Product.RatingLikeId != null)
        //        {
        //            var _lookupRatingLike = await _ratingLikeRepository.FirstOrDefaultAsync((long)output.Product.RatingLikeId);
        //            output.RatingLikeName = _lookupRatingLike?.Name?.ToString();
        //            //output.RatingScore = _lookupRatingLike?.Score;
        //        }

        //        if (output.Product.MediaLibraryId != null)
        //        {
        //            var media = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync(e => e.Id == output.Product.MediaLibraryId);
        //            if (media.BinaryObjectId != null && media.BinaryObjectId != Guid.Empty)
        //            {
        //                //var binaryObject = await _binaryObjectManager.GetOrNullAsync(media.BinaryObjectId);
        //                //output.Picture = Convert.ToBase64String(binaryObject.Bytes);
        //                output.Picture = await _binaryObjectManager.GetProductPictureUrlAsync(media.BinaryObjectId, ".png");
        //            }
        //        }

        //        var tags = _productTagRepository.GetAll().Include(e => e.MasterTagFk)
        //                           .Where(e => e.ProductId == output.Product.Id).OrderBy(e => e.DisplaySequenceNumber).Take(10);

        //        foreach (var tag in tags)
        //        {
        //            if (tag.MasterTagFk != null && tag.MasterTagFk.Name != null)
        //            {
        //                output.ProductTags.Add(tag.MasterTagFk.Name);
        //            }
        //            else if (tag.CustomTag != null)
        //            {
        //                output.ProductTags.Add(tag.CustomTag);
        //            }
        //        }

        //        var store = _storeProductMapRepository.GetAll().Include(e => e.StoreFk).Where(e => e.ProductId == output.Product.Id && e.StoreId != null).FirstOrDefault();

        //        if (store != null)
        //        {
        //            output.StoreId = store.StoreId;
        //            output.StoreName = store.StoreFk?.Name;
        //            var storeTags = _storeTagRepository.GetAll().Include(e => e.MasterTagFk).Where(e => e.StoreId == store.Id).Take(3).OrderBy(e => e.DisplaySequence);

        //            foreach (var tag in storeTags)
        //            {
        //                if (tag.MasterTagFk != null && tag.MasterTagFk.Name != null)
        //                {
        //                    output.StoreTags.Add(tag.MasterTagFk.Name);
        //                }
        //                else if (tag.CustomTag != null)
        //                {
        //                    output.StoreTags.Add(tag.CustomTag);
        //                }
        //            }
        //        }

        //        output.PickupOrDeliveryTags = _productTagRepository.GetAll().Include(e => e.MasterTagFk).Where(e => e.MasterTagCategoryId == 38 && e.ProductId == output.Product.Id && e.MasterTagId != null).Select(e => e.MasterTagFk.Name).ToList();


        //        output.ProductMedias = await GetAllProductMediasByProductId(product.Id);
        //    }
        //    output.HasVariant = await _productByVariantRepository.CountAsync(e => e.ProductId == output.Product.Id) > 0 ? true : false;
        //    output.MembershipPrice = _membershipAndProductMapRepository.GetAll().Where(e => e.ProductId == output.Product.Id && e.MembershipTypeId == (long)MembershipTypeEnum.Snack_Pass).Select(e => e.Price).FirstOrDefault();
        //    if (output.Product.Url != null)
        //    {
        //        output.ShareUrl = _appConfiguration["App:CurrentClientRootAddress"] + "/shop/product/" + output.Product.Url;
        //    }
        //    return output;
        //}
        //[AbpAllowAnonymous]
        //public async Task<List<GetProductMediaForViewDto>> GetAllProductMediasByProductId(long productId)
        //{

        //    var filteredProductMedias = _productMediaRepository.GetAll()
        //                .Include(e => e.ProductFk)
        //                .Include(e => e.MediaLibraryFk)
        //                .Where(e => e.ProductId == productId);

        //    var pagedAndFilteredProductMedias = filteredProductMedias
        //        .OrderBy("displaySequenceNumber asc");

        //    var productMedias = from o in pagedAndFilteredProductMedias
        //                        join o1 in _productRepository.GetAll() on o.ProductId equals o1.Id into j1
        //                        from s1 in j1.DefaultIfEmpty()

        //                        join o2 in _lookup_mediaLibraryRepository.GetAll() on o.MediaLibraryId equals o2.Id into j2
        //                        from s2 in j2.DefaultIfEmpty()

        //                        select new GetProductMediaForViewDto()
        //                        {
        //                            ProductMedia = new ProductMediaDto
        //                            {
        //                                DisplaySequenceNumber = o.DisplaySequenceNumber,
        //                                Id = o.Id,
        //                                ProductId = o.ProductId,
        //                                MediaLibraryId = o.MediaLibraryId
        //                            },
        //                            //ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
        //                            //MediaLibraryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
        //                        };

        //    var result = await productMedias.ToListAsync();

        //    foreach (var productMedia in result)
        //    {
        //        if (productMedia.ProductMedia.MediaLibraryId != null)
        //        {
        //            var media = _lookup_mediaLibraryRepository.Get((long)productMedia.ProductMedia.MediaLibraryId);
        //            if (media.BinaryObjectId != null && media.BinaryObjectId != Guid.Empty)
        //            {
        //                //var binaryObject = await _binaryObjectManager.GetOrNullAsync(media.BinaryObjectId);
        //                //productMedia.Picture = Convert.ToBase64String(binaryObject.Bytes);
        //                productMedia.Picture = await _binaryObjectManager.GetProductPictureUrlAsync(media.BinaryObjectId, ".png");
        //            }

        //            if (media.VideoLink != null)
        //            {
        //                productMedia.VideoUrl = media.VideoLink;
        //            }
        //        }

        //    }

        //    return result;
        //}
        //[AbpAllowAnonymous]
        //public async Task<List<GetPublicProductForViewDto>> GetRelatedProduct(long? productId)
        //{
        //    var filteredProducts = _productRelatedProductRepository.GetAll()
        //                .Include(e => e.PrimaryProductFk)
        //                .Include(e => e.RelatedProductFk)
        //                .WhereIf(productId != null, e => e.PrimaryProductId == productId).OrderBy(e => e.DisplaySequence);

        //    var pagedAndFilteredProducts = filteredProducts;

        //    var products = from o in pagedAndFilteredProducts
        //                   join o1 in _productRepository.GetAll() on o.RelatedProductId equals o1.Id into j1
        //                   from s1 in j1.DefaultIfEmpty()

        //                   join o2 in _productCategoryRepository.GetAll() on s1.ProductCategoryId equals o2.Id into j2
        //                   from s2 in j2.DefaultIfEmpty()

        //                   join o3 in _lookup_mediaLibraryRepository.GetAll() on s1.MediaLibraryId equals o3.Id into j3
        //                   from s3 in j3.DefaultIfEmpty()

        //                   join o4 in _measurementUnitRepository.GetAll() on s1.MeasurementUnitId equals o4.Id into j4
        //                   from s4 in j4.DefaultIfEmpty()

        //                   join o5 in _currencyRepository.GetAll() on s1.CurrencyId equals o5.Id into j5
        //                   from s5 in j5.DefaultIfEmpty()

        //                   join o6 in _ratingLikeRepository.GetAll() on s1.RatingLikeId equals o6.Id into j6
        //                   from s6 in j6.DefaultIfEmpty()

        //                   select new GetPublicProductForViewDto()
        //                   {
        //                       Product = new ProductDto
        //                       {
        //                           Name = s1.Name,
        //                           ShortDescription = s1.ShortDescription,
        //                           Description = s1.Description,
        //                           Sku = s1.Sku,
        //                           Url = s1.Url,
        //                           SeoTitle = s1.SeoTitle,
        //                           MetaKeywords = s1.MetaKeywords,
        //                           MetaDescription = s1.MetaDescription,
        //                           RegularPrice = s1.RegularPrice,
        //                           SalesPrice = s1.SalesPrice,
        //                           PriceDiscountPercentage = s1.PriceDiscountPercentage,
        //                           CallForPrice = s1.CallForPrice,
        //                           UnitPrice = s1.UnitPrice,
        //                           MeasureAmount = s1.MeasureAmount,
        //                           IsTaxExempt = s1.IsTaxExempt,
        //                           StockQuantity = s1.StockQuantity,
        //                           IsDisplayStockQuantity = s1.IsDisplayStockQuantity,
        //                           IsPublished = s1.IsPublished,
        //                           IsPackageProduct = s1.IsPackageProduct,
        //                           InternalNotes = s1.InternalNotes,
        //                           Icon = s1.Icon,
        //                           Id = s1.Id,
        //                           IsWholeSaleProduct = s1.IsWholeSaleProduct,
        //                           MediaLibraryId = s1.MediaLibraryId,
        //                           ProductCategoryId = s1.ProductCategoryId,
        //                           RatingLikeId = s1.RatingLikeId
        //                       },
        //                       ProductCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
        //                       ProductCategoryUrl = s2 == null || s2.Url == null ? "" : s2.Url.ToString(),
        //                       MediaLibraryName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
        //                       MeasurementUnitName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
        //                       CurrencyName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
        //                       CurrencyIcon = s5 == null || s5.Icon == null ? "$" : s5.Icon.ToString(),
        //                       RatingLikeName = s6 == null || s6.Name == null ? "" : s6.Name.ToString(),
        //                       RatingScore = s6 == null || s6.Score == null ? 0 : s6.Score
        //                   };

        //    var results = products.ToList();

        //    foreach (var result in results)
        //    {
        //        var tags = _productTagRepository.GetAll().Include(e => e.MasterTagFk)
        //                          .Where(e => e.ProductId == result.Product.Id).OrderBy(e => e.DisplaySequenceNumber).Take(3);

        //        foreach (var tag in tags)
        //        {
        //            if (tag.MasterTagFk != null && tag.MasterTagFk.Name != null)
        //            {
        //                result.ProductTags.Add(tag.MasterTagFk.Name);
        //            }
        //            else if (tag.CustomTag != null)
        //            {
        //                result.ProductTags.Add(tag.CustomTag);
        //            }
        //        }

        //        var store = await _storeProductMapRepository.GetAll().Include(e => e.StoreFk).Where(e => e.ProductId == result.Product.Id).FirstOrDefaultAsync();
        //        if (store != null)
        //        {
        //            result.StoreId = store.StoreFk.Id;
        //            result.StoreName = store.StoreFk.Name;
        //            result.StoreLogo = store.StoreFk.StoreLogoLink != Guid.Empty ? await _binaryObjectManager.GetStorePictureUrlAsync(store.StoreFk.StoreLogoLink, ".png") : null;
        //        }
        //        if (result.Product.MediaLibraryId != null)
        //        {
        //            var media = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync(e => e.Id == result.Product.MediaLibraryId);
        //            if (media.BinaryObjectId != null)
        //            {
        //                //var binaryObject = await _binaryObjectManager.GetOrNullAsync(media.BinaryObjectId);
        //                //result.Picture = Convert.ToBase64String(binaryObject.Bytes);
        //                result.Picture = await _binaryObjectManager.GetProductPictureUrlAsync(media.BinaryObjectId, ".png");
        //            }
        //        }

        //        result.HasVariant = await _productByVariantRepository.CountAsync(e => e.ProductId == result.Product.Id) > 0 ? true : false;
        //        result.MembershipPrice = _membershipAndProductMapRepository.GetAll().Where(e => e.ProductId == result.Product.Id && e.MembershipTypeId == (long)MembershipTypeEnum.Snack_Pass).Select(e => e.Price).FirstOrDefault();
        //        result.MembershipName = _membershipAndProductMapRepository.GetAll().Include(e => e.MembershipTypeFk).Where(e => e.ProductId == result.Product.Id && e.MembershipTypeId == (long)MembershipTypeEnum.Snack_Pass).Select(e => e.MembershipTypeFk.Name).FirstOrDefault();
        //    }

        //    return results;
        //}
        //[AbpAllowAnonymous]
        //public async Task<List<GetPublicProductForViewDto>> GetCrossSellProduct(long? productId)
        //{
        //    var filteredProducts = _crossSellProductRepository.GetAll()
        //                .Include(e => e.PrimaryProductFk)
        //                .Include(e => e.CrossProductFk)
        //                .WhereIf(productId != null, e => e.PrimaryProductId == productId).OrderBy(e => e.DisplaySequence);

        //    var pagedAndFilteredProducts = filteredProducts;

        //    var products = from o in pagedAndFilteredProducts
        //                   join o1 in _productRepository.GetAll() on o.CrossProductId equals o1.Id into j1
        //                   from s1 in j1.DefaultIfEmpty()

        //                   join o2 in _productCategoryRepository.GetAll() on s1.ProductCategoryId equals o2.Id into j2
        //                   from s2 in j2.DefaultIfEmpty()

        //                   join o3 in _lookup_mediaLibraryRepository.GetAll() on s1.MediaLibraryId equals o3.Id into j3
        //                   from s3 in j3.DefaultIfEmpty()

        //                   join o4 in _measurementUnitRepository.GetAll() on s1.MeasurementUnitId equals o4.Id into j4
        //                   from s4 in j4.DefaultIfEmpty()

        //                   join o5 in _currencyRepository.GetAll() on s1.CurrencyId equals o5.Id into j5
        //                   from s5 in j5.DefaultIfEmpty()

        //                   join o6 in _ratingLikeRepository.GetAll() on s1.RatingLikeId equals o6.Id into j6
        //                   from s6 in j6.DefaultIfEmpty()

        //                   select new GetPublicProductForViewDto()
        //                   {
        //                       Product = new ProductDto
        //                       {
        //                           Name = s1.Name,
        //                           ShortDescription = s1.ShortDescription,
        //                           Description = s1.Description,
        //                           Sku = s1.Sku,
        //                           Url = s1.Url,
        //                           SeoTitle = s1.SeoTitle,
        //                           MetaKeywords = s1.MetaKeywords,
        //                           MetaDescription = s1.MetaDescription,
        //                           RegularPrice = s1.RegularPrice,
        //                           SalesPrice = s1.SalesPrice,
        //                           PriceDiscountPercentage = s1.PriceDiscountPercentage,
        //                           CallForPrice = s1.CallForPrice,
        //                           UnitPrice = s1.UnitPrice,
        //                           MeasureAmount = s1.MeasureAmount,
        //                           IsTaxExempt = s1.IsTaxExempt,
        //                           StockQuantity = s1.StockQuantity,
        //                           IsDisplayStockQuantity = s1.IsDisplayStockQuantity,
        //                           IsPublished = s1.IsPublished,
        //                           IsPackageProduct = s1.IsPackageProduct,
        //                           InternalNotes = s1.InternalNotes,
        //                           Icon = s1.Icon,
        //                           Id = s1.Id,
        //                           IsWholeSaleProduct = s1.IsWholeSaleProduct,
        //                           MediaLibraryId = s1.MediaLibraryId,
        //                           ProductCategoryId = s1.ProductCategoryId,
        //                           RatingLikeId = s1.RatingLikeId
        //                       },
        //                       ProductCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
        //                       ProductCategoryUrl = s2 == null || s2.Url == null ? "" : s2.Url.ToString(),
        //                       MediaLibraryName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
        //                       MeasurementUnitName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
        //                       CurrencyName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
        //                       CurrencyIcon = s5 == null || s5.Icon == null ? "$" : s5.Icon.ToString(),
        //                       RatingLikeName = s6 == null || s6.Name == null ? "" : s6.Name.ToString(),
        //                       RatingScore = s6 == null || s6.Score == null ? 0 : s6.Score
        //                   };

        //    var results = products.ToList();

        //    foreach (var result in results)
        //    {
        //        var tags = _productTagRepository.GetAll().Include(e => e.MasterTagFk)
        //                           .Where(e => e.ProductId == result.Product.Id).OrderBy(e => e.DisplaySequenceNumber).Take(3);

        //        foreach (var tag in tags)
        //        {
        //            if (tag.MasterTagFk != null && tag.MasterTagFk.Name != null)
        //            {
        //                result.ProductTags.Add(tag.MasterTagFk.Name);
        //            }
        //            else if (tag.CustomTag != null)
        //            {
        //                result.ProductTags.Add(tag.CustomTag);
        //            }
        //        }

        //        var store = await _storeProductMapRepository.GetAll().Include(e => e.StoreFk).Where(e => e.ProductId == result.Product.Id).FirstOrDefaultAsync();
        //        if (store != null)
        //        {
        //            result.StoreId = store.StoreFk.Id;
        //            result.StoreName = store.StoreFk.Name;
        //            result.StoreLogo = store.StoreFk.StoreLogoLink != Guid.Empty ? await _binaryObjectManager.GetStorePictureUrlAsync(store.StoreFk.StoreLogoLink, ".png") : null;
        //        }

        //        if (result.Product.MediaLibraryId != null)
        //        {
        //            var media = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync(e => e.Id == result.Product.MediaLibraryId);
        //            if (media.BinaryObjectId != null)
        //            {
        //                //var binaryObject = await _binaryObjectManager.GetOrNullAsync(media.BinaryObjectId);
        //                //result.Picture = Convert.ToBase64String(binaryObject.Bytes);
        //                result.Picture = await _binaryObjectManager.GetProductPictureUrlAsync(media.BinaryObjectId, ".png");
        //            }
        //        }
        //        result.HasVariant = await _productByVariantRepository.CountAsync(e => e.ProductId == result.Product.Id) > 0 ? true : 0false;
        //        result.MembershipPrice = _membershipAndProductMapRepository.GetAll().Where(e => e.ProductId == result.Product.Id && e.MembershipTypeId == (long)MembershipTypeEnum.Snack_Pass).Select(e => e.Price).FirstOrDefault();
        //        result.MembershipName = _membershipAndProductMapRepository.GetAll().Include(e => e.MembershipTypeFk).Where(e => e.ProductId == result.Product.Id && e.MembershipTypeId == (long)MembershipTypeEnum.Snack_Pass).Select(e => e.MembershipTypeFk.Name).FirstOrDefault();
        //    }

        //    return results;
        //}


        //[AbpAllowAnonymous]
        //public async Task<List<ProductProductCategoryLookupTableForPublicDto>> GetAllProductCategoryForSideBar()
        //{
        //    //var results = await _productCategoryRepository.GetAll().Include(e => e.MediaLibraryFk).Where(e => e.Published == true)
        //    //    .Select(productCategory => new ProductProductCategoryLookupTableForPublicDto
        //    //    {
        //    //        Id = productCategory.Id,
        //    //        DisplayName = productCategory == null || productCategory.Name == null ? "" : productCategory.Name.ToString(),
        //    //        Url = productCategory == null || productCategory.Url == null ? "" : productCategory.Url.ToString(),
        //    //        DisplaySequence = productCategory != null ? productCategory.DisplaySequence : null,
        //    //        PictureId = productCategory.MediaLibraryFk != null ? productCategory.MediaLibraryFk.BinaryObjectId : Guid.Empty
        //    //    }).ToListAsync();

        //    List<SqlParameter> sqlParameters = new List<SqlParameter>();

        //    var results = await _storedProcedureRepository.ExecuteStoredProcedure<ProductCategoryDirectoryPublicViewBySp>("usp_GetAllCategoriesForPublicView", CommandType.StoredProcedure, sqlParameters.ToArray());

        //    if (results.Categories != null)
        //    {
        //        foreach (var result in results.Categories)
        //        {
        //            if (result.PictureId != Guid.Empty && result.PictureId != null)
        //            {
        //                result.Picture = await _binaryObjectManager.GetProductPictureUrlAsync((Guid)result.PictureId, ".png");
        //            }
        //        }
        //    }

        //    return results.Categories;
        //}


        //[AbpAllowAnonymous]
        //public async Task<RelevantStorePublicWidgetMapDto> GetRelevantStore(long primaryStoreId, long storeWidgetId)
        //{
        //    var output = new RelevantStorePublicWidgetMapDto();
        //    var widget = _storeDynamicWidgetMapRepository.FirstOrDefault(e => e.StoreId == primaryStoreId && e.StoreWidgetId == storeWidgetId);
        //    if (widget != null)
        //    {
        //        output.StoreDynamicWidgetMap = ObjectMapper.Map<StoreDynamicWidgetMapDto>(widget);
        //    }
        //    var filteredStores = _storeRelevantStoreRepository.GetAll()
        //                .Include(e => e.PrimaryStoreFk)
        //                .Include(e => e.RelevantStoreFk)
        //                .Where(e => e.PrimaryStoreId == primaryStoreId).OrderBy(e => e.DisplaySequence).Take(12);

        //    var pagedAndFilteredStores = filteredStores;

        //    var stores = from o in pagedAndFilteredStores

        //                 join o1 in _storeRepository.GetAll() on o.RelevantStoreId equals o1.Id into j1
        //                 from s1 in j1.DefaultIfEmpty()

        //                 select new RelevantStorePublicViewDto()
        //                 {
        //                     Id = s1.Id,
        //                     Name = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
        //                     Url = s1 == null || s1.StoreUrl == null ? "" : s1.StoreUrl.ToString(),
        //                     StoreLogoLink = s1.StoreLogoLink != null && s1.StoreLogoLink != Guid.Empty ? s1.StoreLogoLink : Guid.Empty,
        //                     DisplaySequence = o.DisplaySequence
        //                 };
        //    var result = await stores.ToListAsync();
        //    foreach (var store in result)
        //    {
        //        if (store.StoreLogoLink != null && store.StoreLogoLink != Guid.Empty)
        //        {
        //            //var binaryObject = await _binaryObjectManager.GetOrNullAsync((Guid)store.StoreLogoLink);
        //            //store.Logo = Convert.ToBase64String(binaryObject.Bytes);
        //            store.Logo = await _binaryObjectManager.GetStorePictureUrlAsync((Guid)store.StoreLogoLink, ".png");
        //        }
        //    }

        //    output.Stores = result.OrderBy(e => e.DisplaySequence).ToList();
        //    return output;
        //}
        //[AbpAllowAnonymous]
        //public async Task<TopProductCategoriesPublicWidgetMapDto> GetTopProductCategories(long storeId, long storeWidgetId)
        //{
        //    var output = new TopProductCategoriesPublicWidgetMapDto();
        //    var widget = _storeDynamicWidgetMapRepository.FirstOrDefault(e => e.StoreId == storeId && e.StoreWidgetId == storeWidgetId);
        //    if (widget != null)
        //    {
        //        output.StoreDynamicWidgetMap = ObjectMapper.Map<StoreDynamicWidgetMapDto>(widget);
        //    }

        //    var productcategoryIdsIds = widget != null ? _storeWidgetProductCategoryMapRepository.GetAll().Where(e => e.StoreDynamicWidgetMapId == widget.Id).OrderBy(e => e.DisplaySequence).Select(e => e.ProductCategoryId) : null;
        //    if (productcategoryIdsIds == null)
        //    {
        //        return output;
        //    }
        //    var filteredProductCategories = _productCategoryRepository.GetAll()
        //                .Include(e => e.MediaLibraryFk)
        //                .WhereIf(productcategoryIdsIds != null, e => productcategoryIdsIds.Contains(e.Id));

        //    var pagedAndFilteredCategories = filteredProductCategories;

        //    var categories = from o in pagedAndFilteredCategories

        //                     join o1 in _lookup_mediaLibraryRepository.GetAll() on o.MediaLibraryId equals o1.Id into j1
        //                     from s1 in j1.DefaultIfEmpty()

        //                     select new TopProductCategoryPublicViewDto()
        //                     {
        //                         Id = o.Id,
        //                         Name = o == null || o.Name == null ? "" : o.Name.ToString(),
        //                         Url = o == null || o.Url == null ? "" : o.Url.ToString(),
        //                         PictureLink = s1.BinaryObjectId != null && s1.BinaryObjectId != Guid.Empty ? s1.BinaryObjectId : Guid.Empty

        //                     };
        //    var result = await categories.ToListAsync();
        //    foreach (var category in result)
        //    {
        //        if (category.PictureLink != null && category.PictureLink != Guid.Empty)
        //        {

        //            //var binaryObject = await _binaryObjectManager.GetOrNullAsync((Guid)category.PictureLink);
        //            //category.Picture = Convert.ToBase64String(binaryObject.Bytes);
        //            category.Picture = await _binaryObjectManager.GetProductPictureUrlAsync((Guid)category.PictureLink, ".png");

        //        }

        //    }

        //    output.ProductCategories = result;
        //    return output;
        //}
        //[AbpAllowAnonymous]
        //public async Task CreateCustomerSubscription(CreateOrEditCustomerSubscriptionListDto input)
        //{
        //    var customer = _customerSubscriptionListRepository.FirstOrDefault(e => e.EmailAddress.Equals(input.EmailAddress));

        //    if (customer == null)
        //    {
        //        var customerSubscriptionList = ObjectMapper.Map<CustomerSubscriptionList>(input);


        //        if (AbpSession.TenantId != null)
        //        {
        //            customerSubscriptionList.TenantId = (int?)AbpSession.TenantId;
        //        }


        //        await _customerSubscriptionListRepository.InsertAsync(customerSubscriptionList);
        //    }

        //}
        //[AbpAllowAnonymous]
        //public async Task<GetProductReviewsByProductBySpForView> GetAllProductReviewsByProductBySp(GetAllProductReviewsByProductIdForSpInput input)
        //{
        //    List<SqlParameter> parameters = PrepareSearchParameterForGetAllProductReviewsByProductBySp(input);
        //    var result = await _storedProcedureRepository.ExecuteStoredProcedure<GetProductReviewsByProductBySpForView>("usp_GetAllProductReviewsByProductIdAndContactId", CommandType.StoredProcedure, parameters.ToArray());
        //    return result;
        //}

        //private static List<SqlParameter> PrepareSearchParameterForGetAllProductReviewsByProductBySp(GetAllProductReviewsByProductIdForSpInput input)
        //{
        //    List<SqlParameter> sqlParameters = new List<SqlParameter>();

        //    if (input.Filter != null)
        //    {
        //        input.Filter = input.Filter[0] == '"' && input.Filter[input.Filter.Length - 1] == '"' ? "*" + input.Filter + "*" : '"' + "*" + input.Filter + "*" + '"';
        //    }
        //    SqlParameter filter = new SqlParameter("@Filter", input.Filter == null ? "\"\"" : input.Filter);
        //    sqlParameters.Add(filter);

        //    SqlParameter productIdFilter = new SqlParameter("@ProductId", input.ProductIdFilter == null ? (object)DBNull.Value : input.ProductIdFilter);
        //    sqlParameters.Add(productIdFilter);

        //    SqlParameter contactIdFilter = new SqlParameter("@ContactId", input.ContactIdFilter == null ? (object)DBNull.Value : input.ContactIdFilter);
        //    sqlParameters.Add(contactIdFilter);

        //    SqlParameter isPublishFilter = new SqlParameter("@IsPublished", input.IsPublish == -1 ? (object)DBNull.Value : input.IsPublish);
        //    sqlParameters.Add(isPublishFilter);

        //    SqlParameter skipCount = new SqlParameter("@SkipCount", input.SkipCount);
        //    sqlParameters.Add(skipCount);

        //    SqlParameter maxResultCount = new SqlParameter("@MaxResultCount", input.MaxResultCount);
        //    sqlParameters.Add(maxResultCount);

        //    return sqlParameters;
        //}
        //[AbpAllowAnonymous]
        //public async Task<GetAllProductsForPublicProductsBySp> GetProductsByCategorySlug(string categoryUrl, int skipCount, int maxCount)
        //{

        //    var productCategory = await _productCategoryRepository.FirstOrDefaultAsync(e => e.Url.Equals(categoryUrl));

        //    List<SqlParameter> sqlParameters = new List<SqlParameter>();

        //    SqlParameter sCount = new SqlParameter("@SkipCount", skipCount);
        //    sqlParameters.Add(sCount);

        //    SqlParameter maxResultCount = new SqlParameter("@MaxResultCount", maxCount);
        //    sqlParameters.Add(maxResultCount);

        //    if (productCategory != null)
        //    {
        //        SqlParameter categoryIdFilter = new SqlParameter("@CategoryIdFilter", productCategory.Id);
        //        sqlParameters.Add(categoryIdFilter);
        //    }


        //    var response = await _storedProcedureRepository.ExecuteStoredProcedure<GetAllProductsForPublicProductsBySp>("usp_GetAllPublicProductList", CommandType.StoredProcedure, sqlParameters.ToArray());



        //    if (response.Products != null)
        //    {
        //        foreach (var result in response.Products)
        //        {
        //            var store = await _storeProductMapRepository.GetAll().Include(e => e.StoreFk).Where(e => e.ProductId == result.Product.Id).FirstOrDefaultAsync();
        //            if (store != null)
        //            {
        //                result.StoreId = store.StoreFk.Id;
        //                result.StoreName = store.StoreFk.Name;
        //                result.StoreLogo = store.StoreFk.StoreLogoLink != Guid.Empty ? await _binaryObjectManager.GetStorePictureUrlAsync(store.StoreFk.StoreLogoLink, ".png") : null;
        //            }
        //            if (result.Product.MediaLibraryId != null)
        //            {
        //                var media = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync(e => e.Id == result.Product.MediaLibraryId);
        //                if (media.BinaryObjectId != null)
        //                {
        //                    //var binaryObject = await _binaryObjectManager.GetOrNullAsync(media.BinaryObjectId);
        //                    //result.Picture = Convert.ToBase64String(binaryObject.Bytes);
        //                    result.Picture = await _binaryObjectManager.GetProductPictureUrlAsync(media.BinaryObjectId, ".png");
        //                }
        //            }
        //            result.HasVariant = await _productByVariantRepository.CountAsync(e => e.ProductId == result.Product.Id) > 0 ? true : false;
        //            result.MembershipPrice = _membershipAndProductMapRepository.GetAll().Where(e => e.ProductId == result.Product.Id && e.MembershipTypeId == (long)MembershipTypeEnum.Snack_Pass).Select(e => e.Price).FirstOrDefault();
        //            result.MembershipName = _membershipAndProductMapRepository.GetAll().Include(e => e.MembershipTypeFk).Where(e => e.ProductId == result.Product.Id && e.MembershipTypeId == (long)MembershipTypeEnum.Snack_Pass).Select(e => e.MembershipTypeFk.Name).FirstOrDefault();

        //        }
        //    }

        //    return response;
        //}

        //[AbpAuthorize(AppPermissions.Pages_Contact_MyProductReviews)]
        //public async Task CreateProductReview(CreateOrEditProductReviewDto input)
        //{
        //    var productReview = ObjectMapper.Map<ProductReview>(input);

        //    productReview.PostDate = DateTime.Now;
        //    productReview.PostTime = DateTime.Now.ToString("hh:mm tt");
        //    if (AbpSession.TenantId != null)
        //    {
        //        productReview.TenantId = (int?)AbpSession.TenantId;
        //    }


        //    await _productReviewRepository.InsertAsync(productReview);
        //}
        //[AbpAllowAnonymous]
        //public async Task<TopBrandManufacturerPublicWidgetMapDto> GetTopBrandManufacturers(long storeId, long storeWidgetId)
        //{
        //    var output = new TopBrandManufacturerPublicWidgetMapDto();
        //    var widget = _storeDynamicWidgetMapRepository.FirstOrDefault(e => e.StoreId == storeId && e.StoreWidgetId == storeWidgetId);
        //    if (widget != null)
        //    {
        //        output.StoreDynamicWidgetMap = ObjectMapper.Map<StoreDynamicWidgetMapDto>(widget);
        //    }

        //    var brandManufacturerIds = widget != null ? _storeWidgetBrandManufacturerMapRepository.GetAll().Where(e => e.StoreDynamicWidgetMapId == widget.Id).OrderBy(e => e.DisplaySequence).Select(e => e.BrandManufacturerId) : null;

        //    var filteredBrandManufacturers = _brandManufacturerRepository.GetAll()
        //                .WhereIf(brandManufacturerIds != null, e => brandManufacturerIds.Contains(e.Id));

        //    var pagedAndFilteredBrandManufacturers = filteredBrandManufacturers;

        //    var topBrands = from o in pagedAndFilteredBrandManufacturers

        //                    select new TopBrandManufacturerPublicViewDto()
        //                    {
        //                        Id = o.Id,
        //                        Name = o == null || o.Name == null ? "" : o.Name.ToString(),
        //                        LogoBinaryObjectId = o.Logo != null && o.Logo != Guid.Empty ? o.Logo : Guid.Empty

        //                    };
        //    var result = await topBrands.ToListAsync();
        //    foreach (var item in result)
        //    {
        //        if (item.LogoBinaryObjectId != null && item.LogoBinaryObjectId != Guid.Empty)
        //        {

        //            //var binaryObject = await _binaryObjectManager.GetOrNullAsync((Guid)item.LogoBinaryObjectId);
        //            //item.Logo = Convert.ToBase64String(binaryObject.Bytes);
        //            item.Logo = await _binaryObjectManager.GetOthersPictureUrlAsync(item.LogoBinaryObjectId, ".png");

        //        }

        //    }

        //    output.BrandManufacturers = result;
        //    return output;
        //}
        //[AbpAllowAnonymous]
        //public async Task<ProductCategoryDto> GetProductCategoryBySlug(string categoryUrl)
        //{
        //    var productCategory = await _productCategoryRepository.FirstOrDefaultAsync(e => e.Url.Equals(categoryUrl));
        //    return ObjectMapper.Map<ProductCategoryDto>(productCategory);
        //}
        //[AbpAllowAnonymous]
        //public async Task<PagedResultDto<PublicStoreListViewDto>> GetStoreList(PublicStoreListInput input)
        //{
        //    var storeIdsByHub = new List<long?>();
        //    var storeIdsByCity = new List<long?>();
        //    var storeIdsByZipCode = new List<long>();

        //    if (input.HubId != null)
        //    {
        //        var ids = _hubsStoreRepository.GetAll().Where(e => e.HubId == input.HubId && e.StoreId != null).Select(e => e.StoreId).ToList();
        //        storeIdsByHub.AddRange(ids);
        //    }

        //    if (input.ZipCode != null)
        //    {
        //        var ids = _storeZipCodeMapRepository.GetAll().Where(e => e.ZipCode == input.ZipCode).Select(e => e.StoreId).ToList();
        //        storeIdsByZipCode.AddRange(ids);
        //    }

        //    if (input.City != null)
        //    {
        //        var ids = _storeLocationRepository.GetAll()
        //            .WhereIf(!string.IsNullOrWhiteSpace(input.City), e => false || e.LocationName.Contains(input.City) || e.City.Contains(input.City))
        //            .Select(e => e.StoreId).ToList();
        //        storeIdsByCity.AddRange(ids);
        //    }


        //    var filteredStores = _storeRepository.GetAll()
        //                //.Where(e => e.Id != 1)
        //                .Where(e => e.IsPublished == true)
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.City), e => false || e.City.Contains(input.City) || storeIdsByCity.Contains(e.Id))
        //                .WhereIf(input.HubId != null, e => storeIdsByHub.Contains(e.Id))
        //                .WhereIf(input.ZipCode != null, e => storeIdsByZipCode.Contains(e.Id));

        //    var pagedAndFilteredStores = filteredStores.OrderBy(input.Sorting ?? "displaySequence asc")
        //        .PageBy(input); ;

        //    var stores = from o in pagedAndFilteredStores

        //                 select new PublicStoreListViewDto()
        //                 {
        //                     Id = o.Id,
        //                     Name = o == null || o.Name == null ? "" : o.Name.ToString(),
        //                     Url = o == null || o.StoreUrl == null ? "" : o.StoreUrl.ToString(),
        //                     StoreLogoLink = o.StoreLogoLink != null && o.StoreLogoLink != Guid.Empty ? o.StoreLogoLink : Guid.Empty,

        //                 };
        //    var result = await stores.ToListAsync();
        //    foreach (var store in result)
        //    {
        //        if (store.StoreLogoLink != null && store.StoreLogoLink != Guid.Empty)
        //        {
        //            //var binaryObject = await _binaryObjectManager.GetOrNullAsync((Guid)store.StoreLogoLink);
        //            //store.Logo = Convert.ToBase64String(binaryObject.Bytes);
        //            store.Logo = await _binaryObjectManager.GetStorePictureUrlAsync((Guid)store.StoreLogoLink, ".png");
        //        }
        //    }

        //    var totalCount = await filteredStores.CountAsync();

        //    return new PagedResultDto<PublicStoreListViewDto>(
        //        totalCount,
        //        result
        //    );
        //}

        //[AbpAllowAnonymous]
        //public async Task<GetPublicStoreForViewDto> GetStoreDetailsByUrl(string url)
        //{

        //    var output = new GetPublicStoreForViewDto();

        //    var store = await _storeRepository.FirstOrDefaultAsync(e => e.StoreUrl.Equals(url));

        //    if (store != null)
        //    {
        //        output.Store = ObjectMapper.Map<StoreDto>(store);

        //        if (store.StateId != null)
        //        {
        //            var _lookupState = await _stateRepository.FirstOrDefaultAsync((long)store.StateId);
        //            output.StateName = _lookupState?.Name?.ToString();
        //        }

        //        if (store.CountryId != null)
        //        {
        //            var _lookupCountry = await _countryRepository.FirstOrDefaultAsync((long)store.CountryId);
        //            output.CountryName = _lookupCountry?.Name?.ToString();
        //        }

        //        if (store.RatingLikeId != null)
        //        {
        //            var _lookupRating = await _ratingLikeRepository.FirstOrDefaultAsync((long)store.RatingLikeId);
        //            output.RatingScore = _lookupRating?.Score;
        //        }

        //        if (store.StoreLogoLink != null && store.StoreLogoLink != Guid.Empty)
        //        {
        //            //var binaryObject = await _binaryObjectManager.GetOrNullAsync((Guid)store.StoreLogoLink);
        //            //output.Logo = Convert.ToBase64String(binaryObject.Bytes);
        //            output.Logo = await _binaryObjectManager.GetStorePictureUrlAsync(store.StoreLogoLink, ".png");
        //        }
        //        output.NumberOfReviews = await _storeReviewRepository.CountAsync(e => e.StoreId == store.Id && e.IsPublish == true);

        //        output.StoreMedias = await GetAllStoreMediasByStoreId(store.Id);

        //        output.StoreTags = await GetAllStoreTagsByStoreId(store.Id);

        //        var primaryCategory = _storeTagRepository.GetAll().Include(e => e.MasterTagFk).Where(e => e.StoreId == output.Store.Id && e.MasterTagCategoryId == 37 && e.MasterTagId != null).FirstOrDefault();
        //        if (primaryCategory != null)
        //        {
        //            output.PrimaryCategoryName = primaryCategory.MasterTagFk.Name;
        //        }
        //    }


        //    return output;
        //}


        //private async Task<List<GetStoreMediaForViewDto>> GetAllStoreMediasByStoreId(long? storeId)
        //{

        //    var filteredStoreMedias = _storeMediaRepository.GetAll()
        //                .Include(e => e.StoreFk)
        //                .Include(e => e.MediaLibraryFk)
        //                .WhereIf(storeId != null, e => e.StoreId == storeId);

        //    var pagedAndFilteredStoreMedias = filteredStoreMedias
        //        .OrderBy("id desc");

        //    var storeMedias = from o in pagedAndFilteredStoreMedias
        //                      join o1 in _storeRepository.GetAll() on o.StoreId equals o1.Id into j1
        //                      from s1 in j1.DefaultIfEmpty()

        //                      join o2 in _lookup_mediaLibraryRepository.GetAll() on o.MediaLibraryId equals o2.Id into j2
        //                      from s2 in j2.DefaultIfEmpty()

        //                      select new GetStoreMediaForViewDto()
        //                      {
        //                          StoreMedia = new StoreMediaDto
        //                          {
        //                              DisplaySequence = o.DisplaySequence,
        //                              Id = o.Id,
        //                              StoreId = o.StoreId,
        //                              MediaLibraryId = o.MediaLibraryId
        //                          },
        //                          StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
        //                          MediaLibraryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
        //                      };

        //    var result = await storeMedias.ToListAsync();

        //    foreach (var storeMedia in result)
        //    {
        //        if (storeMedia.StoreMedia.MediaLibraryId != null)
        //        {
        //            var media = _lookup_mediaLibraryRepository.Get((long)storeMedia.StoreMedia.MediaLibraryId);
        //            if (media.BinaryObjectId != null && media.BinaryObjectId != Guid.Empty)
        //            {
        //                //var binaryObject = await _binaryObjectManager.GetOrNullAsync(media.BinaryObjectId);
        //                //storeMedia.Picture = Convert.ToBase64String(binaryObject.Bytes);
        //                storeMedia.Picture = await _binaryObjectManager.GetStorePictureUrlAsync(media.BinaryObjectId, ".png");
        //            }

        //            if (media.VideoLink != null)
        //            {
        //                storeMedia.VideoUrl = media.VideoLink;
        //            }
        //        }

        //    }
        //    return result;
        //}

        //private async Task<List<string>> GetAllStoreTagsByStoreId(long? storeId)
        //{
        //    var output = new List<string>();
        //    var filteredStoreTags = _storeTagRepository.GetAll()
        //                .Include(e => e.StoreFk)
        //                .Include(e => e.MasterTagCategoryFk)
        //                .Include(e => e.MasterTagFk)
        //                .WhereIf(storeId != null, e => e.StoreId == storeId);

        //    var pagedAndFilteredStoreTags = filteredStoreTags
        //        .OrderBy("id desc");

        //    var storeTags = from o in pagedAndFilteredStoreTags
        //                    join o1 in _storeRepository.GetAll() on o.StoreId equals o1.Id into j1
        //                    from s1 in j1.DefaultIfEmpty()

        //                    join o2 in _masterTagCategoryRepository.GetAll() on o.MasterTagCategoryId equals o2.Id into j2
        //                    from s2 in j2.DefaultIfEmpty()

        //                    join o3 in _masterTagRepository.GetAll() on o.MasterTagId equals o3.Id into j3
        //                    from s3 in j3.DefaultIfEmpty()

        //                    select new GetStoreTagForViewDto()
        //                    {
        //                        StoreTag = new StoreTagDto
        //                        {
        //                            DisplaySequence = o.DisplaySequence,
        //                            CustomTag = o.CustomTag,
        //                            Id = o.Id
        //                        },
        //                        StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
        //                        MasterTagCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
        //                        MasterTagName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
        //                    };
        //    foreach (var storeTag in storeTags)
        //    {
        //        if (storeTag.StoreTag.CustomTag != null)
        //        {
        //            output.Add(storeTag.StoreTag.CustomTag);
        //        }
        //        else if (storeTag.MasterTagName != null)
        //        {
        //            output.Add(storeTag.MasterTagName);
        //        }
        //    }

        //    return output;
        //}
        //[AbpAllowAnonymous]
        //public async Task<GetStoreHourForViewDto> GetStoreHourForView(long storeId)
        //{
        //    var output = new GetStoreHourForViewDto();

        //    var storeHour = await _storeHourRepository.FirstOrDefaultAsync(e => e.StoreId == storeId);

        //    if (storeHour != null)
        //    {
        //        output.StoreHour = ObjectMapper.Map<StoreHourDto>(storeHour);

        //        if (output.StoreHour.MasterTagCategoryId != null)
        //        {
        //            var _lookupMasterTagCategory = await _masterTagCategoryRepository.FirstOrDefaultAsync((long)output.StoreHour.MasterTagCategoryId);
        //            output.MasterTagCategoryName = _lookupMasterTagCategory?.Name?.ToString();
        //        }

        //        if (output.StoreHour.MasterTagId != null)
        //        {
        //            var _lookupMasterTag = await _masterTagRepository.FirstOrDefaultAsync((long)output.StoreHour.MasterTagId);
        //            output.MasterTagName = _lookupMasterTag?.Name?.ToString();
        //        }

        //    }


        //    return output;
        //}
        //[AbpAllowAnonymous]
        //public async Task<GetStoreReviewsByStoreBySpForView> GetAllStoreReviewsByStore(GetAllStoreReviewsByStoreIdForSpInput input)
        //{
        //    List<SqlParameter> parameters = PrepareSearchParameterForGetAllStoreReviewsByStoreBySp(input);
        //    var result = await _storedProcedureRepository.ExecuteStoredProcedure<GetStoreReviewsByStoreBySpForView>("usp_GetAllStoreReviewsByStoreIdAndContactId", CommandType.StoredProcedure, parameters.ToArray());
        //    return result;
        //}

        //private static List<SqlParameter> PrepareSearchParameterForGetAllStoreReviewsByStoreBySp(GetAllStoreReviewsByStoreIdForSpInput input)
        //{
        //    List<SqlParameter> sqlParameters = new List<SqlParameter>();

        //    if (input.Filter != null)
        //    {
        //        input.Filter = input.Filter[0] == '"' && input.Filter[input.Filter.Length - 1] == '"' ? "*" + input.Filter + "*" : '"' + "*" + input.Filter + "*" + '"';
        //    }
        //    SqlParameter filter = new SqlParameter("@Filter", input.Filter == null ? "\"\"" : input.Filter);
        //    sqlParameters.Add(filter);

        //    SqlParameter productIdFilter = new SqlParameter("@StoreId", input.StoreIdFilter == null ? (object)DBNull.Value : input.StoreIdFilter);
        //    sqlParameters.Add(productIdFilter);

        //    SqlParameter contactIdFilter = new SqlParameter("@ContactId", input.ContactIdFilter == null ? (object)DBNull.Value : input.ContactIdFilter);
        //    sqlParameters.Add(contactIdFilter);

        //    SqlParameter isPublishFilter = new SqlParameter("@IsPublished", input.IsPublish == -1 ? (object)DBNull.Value : input.IsPublish);
        //    sqlParameters.Add(isPublishFilter);

        //    SqlParameter skipCount = new SqlParameter("@SkipCount", input.SkipCount);
        //    sqlParameters.Add(skipCount);

        //    SqlParameter maxResultCount = new SqlParameter("@MaxResultCount", input.MaxResultCount);
        //    sqlParameters.Add(maxResultCount);

        //    return sqlParameters;
        //}

        //[AbpAuthorize(AppPermissions.Pages_Contact_MyStoreReviews)]
        //public async Task CreateStoreReview(CreateOrEditStoreReviewDto input)
        //{
        //    var storeReview = ObjectMapper.Map<StoreReview>(input);

        //    storeReview.PostDate = DateTime.Now;
        //    storeReview.PostTime = DateTime.Now.ToString("hh:mm tt");
        //    if (AbpSession.TenantId != null)
        //    {
        //        storeReview.TenantId = (int?)AbpSession.TenantId;
        //    }


        //    await _storeReviewRepository.InsertAsync(storeReview);
        //}
        //[AbpAllowAnonymous]
        //public async Task<List<GetPublicProductForViewDto>> GetProductsByStore(long? storeId)
        //{

        //    //var storeProductIds = storeId != null ? _storeProductMapRepository.GetAll().Where(e => e.StoreId == storeId).Select(e => new{ e.ProductId,e.DisplaySequence}) : null;

        //    var filteredProducts = _storeProductMapRepository.GetAll()
        //                .Include(e => e.ProductFk)
        //                .Where(e => e.StoreId == storeId && e.ProductId != null).OrderBy(e => e.DisplaySequence);

        //    var pagedAndFilteredProducts = filteredProducts;

        //    var products = from o in pagedAndFilteredProducts
        //                   join o1 in _productCategoryRepository.GetAll() on o.ProductFk.ProductCategoryId equals o1.Id into j1
        //                   from s1 in j1.DefaultIfEmpty()

        //                   join o2 in _lookup_mediaLibraryRepository.GetAll() on o.ProductFk.MediaLibraryId equals o2.Id into j2
        //                   from s2 in j2.DefaultIfEmpty()

        //                   join o3 in _measurementUnitRepository.GetAll() on o.ProductFk.MeasurementUnitId equals o3.Id into j3
        //                   from s3 in j3.DefaultIfEmpty()

        //                   join o4 in _currencyRepository.GetAll() on o.ProductFk.CurrencyId equals o4.Id into j4
        //                   from s4 in j4.DefaultIfEmpty()

        //                   join o5 in _ratingLikeRepository.GetAll() on o.ProductFk.RatingLikeId equals o5.Id into j5
        //                   from s5 in j5.DefaultIfEmpty()

        //                   select new GetPublicProductForViewDto()
        //                   {
        //                       Product = new ProductDto
        //                       {
        //                           Name = o.ProductFk.Name,
        //                           ShortDescription = o.ProductFk.ShortDescription,
        //                           Description = o.ProductFk.Description,
        //                           Sku = o.ProductFk.Sku,
        //                           Url = o.ProductFk.Url,
        //                           SeoTitle = o.ProductFk.SeoTitle,
        //                           MetaKeywords = o.ProductFk.MetaKeywords,
        //                           MetaDescription = o.ProductFk.MetaDescription,
        //                           RegularPrice = o.ProductFk.RegularPrice,
        //                           SalesPrice = o.ProductFk.SalesPrice,
        //                           PriceDiscountPercentage = o.ProductFk.PriceDiscountPercentage,
        //                           CallForPrice = o.ProductFk.CallForPrice,
        //                           UnitPrice = o.ProductFk.UnitPrice,
        //                           MeasureAmount = o.ProductFk.MeasureAmount,
        //                           IsTaxExempt = o.ProductFk.IsTaxExempt,
        //                           StockQuantity = o.ProductFk.StockQuantity,
        //                           IsDisplayStockQuantity = o.ProductFk.IsDisplayStockQuantity,
        //                           IsPublished = o.ProductFk.IsPublished,
        //                           IsPackageProduct = o.ProductFk.IsPackageProduct,
        //                           InternalNotes = o.ProductFk.InternalNotes,
        //                           Icon = o.ProductFk.Icon,
        //                           Id = o.ProductFk.Id,
        //                           IsWholeSaleProduct = o.ProductFk.IsWholeSaleProduct,
        //                           MediaLibraryId = o.ProductFk.MediaLibraryId,
        //                           ProductCategoryId = o.ProductFk.ProductCategoryId,
        //                           RatingLikeId = o.ProductFk.RatingLikeId
        //                       },
        //                       ProductCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
        //                       ProductCategoryUrl = s1 == null || s1.Url == null ? "" : s1.Url.ToString(),
        //                       MediaLibraryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
        //                       MeasurementUnitName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
        //                       CurrencyName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
        //                       CurrencyIcon = s4 == null || s4.Icon == null ? "$" : s4.Icon.ToString(),
        //                       RatingLikeName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
        //                       RatingScore = s5 == null || s5.Score == null ? 0 : s5.Score,
        //                       StoreId = storeId,
        //                       DisplaySequence = o.DisplaySequence
        //                   };

        //    var results = products.ToList();

        //    foreach (var result in results)
        //    {
        //        if (result.Product.MediaLibraryId != null)
        //        {
        //            var media = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync(e => e.Id == result.Product.MediaLibraryId);
        //            if (media.BinaryObjectId != null)
        //            {
        //                //var binaryObject = await _binaryObjectManager.GetOrNullAsync(media.BinaryObjectId);
        //                //result.Picture = Convert.ToBase64String(binaryObject.Bytes);
        //                result.Picture = await _binaryObjectManager.GetProductPictureUrlAsync(media.BinaryObjectId, ".png");
        //            }
        //        }
        //        result.HasVariant = await _productByVariantRepository.CountAsync(e => e.ProductId == result.Product.Id) > 0 ? true : false;
        //    }

        //    return results.OrderBy(e => e.DisplaySequence).ToList();
        //}
        //[AbpAllowAnonymous]
        //public async Task<List<GetPublicStoreCategoriesForViewDto>> GetCategoryWiseProductsByStore(long? storeId, int skipCount)
        //{

        //    List<SqlParameter> sqlParameters = new List<SqlParameter>();

        //    SqlParameter sCount = new SqlParameter("@SkipCount", skipCount);
        //    sqlParameters.Add(sCount);

        //    SqlParameter maxResultCount = new SqlParameter("@MaxResultCount", 2);
        //    sqlParameters.Add(maxResultCount);

        //    SqlParameter storeIdFilter = new SqlParameter("@StoreIdFilter", storeId);
        //    sqlParameters.Add(storeIdFilter);

        //    var results = await _storedProcedureRepository.ExecuteStoredProcedure<GetPublicCategoryWiseProductsForViewDto>("usp_GetCategoryWiseProductsByStore", CommandType.StoredProcedure, sqlParameters.ToArray());

        //    foreach (var item in results.Categories)
        //    {
        //        if (item.Products != null && item.Products.Count > 0)
        //        {
        //            foreach (var result in item.Products)
        //            {
        //                if (result.Product.MediaLibraryId != null)
        //                {
        //                    var media = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync(e => e.Id == result.Product.MediaLibraryId);
        //                    if (media.BinaryObjectId != null)
        //                    {
        //                        result.Picture = await _binaryObjectManager.GetProductPictureUrlAsync(media.BinaryObjectId, ".png");
        //                    }
        //                }
        //                result.HasVariant = await _productByVariantRepository.CountAsync(e => e.ProductId == result.Product.Id) > 0 ? true : false;
        //                result.MembershipPrice = _membershipAndProductMapRepository.GetAll().Where(e => e.ProductId == result.Product.Id && e.MembershipTypeId == (long)MembershipTypeEnum.Snack_Pass).Select(e => e.Price).FirstOrDefault();
        //                result.MembershipName = _membershipAndProductMapRepository.GetAll().Include(e => e.MembershipTypeFk).Where(e => e.ProductId == result.Product.Id && e.MembershipTypeId == (long)MembershipTypeEnum.Snack_Pass).Select(e => e.MembershipTypeFk.Name).FirstOrDefault();
        //            }
        //        }
        //    }

        //    return results.Categories;
        //}
        //[AbpAllowAnonymous]
        //public async Task<List<ProductProductCategoryLookupTableForPublicDto>> GetAllProductCategoryByStoreForSideBar(long storeId)
        //{
        //    var productCategoryIds = _storeCategoryMapRepository.GetAll().Where(e => e.StoreId == storeId && e.Published == true).Select(e => e.ProductCategoryId);
        //    var results = await _productCategoryRepository.GetAll().WhereIf(productCategoryIds != null, e => productCategoryIds.Contains(e.Id))
        //        .Select(productCategory => new ProductProductCategoryLookupTableForPublicDto
        //        {
        //            Id = productCategory.Id,
        //            DisplayName = productCategory == null || productCategory.Name == null ? "" : productCategory.Name.ToString(),
        //            Url = productCategory == null || productCategory.Url == null ? "" : productCategory.Url.ToString()
        //        }).ToListAsync();

        //    foreach (var result in results)
        //    {
        //        result.NumberOfProducts = _storeProductMapRepository.GetAll().Include(e => e.ProductFk).Where(e => e.StoreId == storeId && e.ProductFk.ProductCategoryId == result.Id).Count();
        //        result.DisplaySequence = _storeCategoryMapRepository.GetAll().Where(e => e.StoreId == storeId && e.ProductCategoryId == result.Id).Select(e => e.DisplaySequence).FirstOrDefault();
        //    }
        //    return results.OrderBy(e => e.DisplaySequence).ToList();
        //}
        //[AbpAllowAnonymous]
        //public async Task<List<ProductProductCategoryLookupTableForPublicDto>> GetAllProductCategoryByHub(long hubId)
        //{
        //    var productCategoryIds = _hubsProductCategoryRepository.GetAll().Where(e => e.HubId == hubId && e.Published == true).Select(e => e.ProductCategoryId);
        //    var results = await _productCategoryRepository.GetAll().WhereIf(productCategoryIds != null, e => productCategoryIds.Contains(e.Id))
        //        .Select(productCategory => new ProductProductCategoryLookupTableForPublicDto
        //        {
        //            Id = productCategory.Id,
        //            DisplayName = productCategory == null || productCategory.Name == null ? "" : productCategory.Name.ToString(),
        //            Url = productCategory == null || productCategory.Url == null ? "" : productCategory.Url.ToString()
        //        }).ToListAsync();

        //    foreach (var result in results)
        //    {
        //        result.NumberOfProducts = _hubsProductRepository.GetAll().Include(e => e.ProductFk).Where(e => e.HubId == hubId && e.ProductFk.ProductCategoryId == result.Id).Count();
        //        result.DisplaySequence = _hubsProductCategoryRepository.GetAll().Where(e => e.HubId == hubId && e.ProductCategoryId == result.Id).Select(e => e.DisplaySequence).FirstOrDefault();
        //    }
        //    return results.OrderBy(e => e.DisplaySequence).ToList();
        //}
        //[AbpAllowAnonymous]
        //public async Task SendEmailForContactUs(GetPublicContactUsForInputDto input)
        //{
        //    string contactInfo = "<br/>" +
        //                        "<b>Contact Information</b>" +
        //                        "<br/> " +
        //                        "Name: " + input.Name + "<br/> " +
        //                        "Mobile: " + input.Phone + "<br/> " +
        //                        "Email: " + input.Email + "<br/> "
        //                        ;
        //    await _emailSender.SendAsync(
        //        from: input.Email,
        //        to: _sendGridEmailSenderConfiguration.FromEmail,
        //        subject: input.Subject,
        //        body: input.Enquiry + contactInfo,
        //        isBodyHtml: true
        //    );
        //}

        //[AbpAuthorize]
        //public async Task<long> CreateOrder(CreateOrderForPublicInputDto input)
        //{
        //    var order = new Shop.Order();

        //    order.DeliveryOrPickup = input.IsDeliveryOrPickup;
        //    order.PaymentCompleted = input.IsPaymentCompleted;
        //    order.FullName = input.FullName;
        //    order.DeliveryAddress = input.DeliveryAddress;
        //    order.Email = input.Email;
        //    order.Phone = input.Phone;
        //    order.ZipCode = input.ZipCode;
        //    order.Notes = input.CustomerInstruction;
        //    order.DeliveryFee = input.DeliveryFee;
        //    order.SubTotalExcludedTax = input.SubTotalExcludedTax;
        //    order.TotalDiscountAmount = input.TotalDiscountAmount;
        //    order.TotalTaxAmount = input.TotalTaxAmount;
        //    order.TotalAmount = input.TotalAmount;
        //    order.ContactId = input.ContactId;
        //    order.ContactAddressId = input.ContactAddressId;
        //    order.CurrencyId = input.CurrencyId;
        //    order.TaxRateId = input.TaxRateId;
        //    order.City = input.City;
        //    order.StateId = input.StateId;
        //    order.CountryId = input.CountryId;
        //    order.DeliveryCharge = input.DeliveryCharge;
        //    order.ServiceCharge = input.ServiceCharge;
        //    order.GratuityAmount = input.GratuityAmount;
        //    order.StoreId = input.StoreId;
        //    if ((input.GratuityPercentage == null || input.GratuityPercentage == 0) && input.GratuityAmount != 0)
        //    {
        //        input.GratuityPercentage = (input.GratuityAmount / ((double)order.TotalAmount - input.GratuityAmount)) * 100;
        //    }
        //    else
        //    {
        //        order.GratuityPercentage = input.GratuityPercentage;

        //    }

        //    var status = new OrderFulfillmentStatus();
        //    if (input.IsDeliveryOrPickup)
        //    {
        //        order.OrderStatusTypeId = Convert.ToInt32(_appConfiguration["OrderStatusTypes:Received"]);
        //        status.OrderStatusTypeId = Convert.ToInt32(_appConfiguration["OrderStatusTypes:Received"]);
        //    }
        //    else
        //    {
        //        order.OrderPickupStatusTypeId = Convert.ToInt32(_appConfiguration["PickupStatusTypes:Order_Placed"]);
        //        status.OrderPickupStatusTypeId = Convert.ToInt32(_appConfiguration["PickupStatusTypes:Order_Placed"]);
        //    }

        //    long orderId = _orderRepository.InsertAndGetId(order);

        //    if (input.ContactId != null && input.TotalAmount != null)
        //    {
        //        await UpdateCustomerRewardPointForOrder(input.ContactId, input.TotalAmount, orderId);
        //    }


        //    status.OrderId = orderId;
        //    await _orderFulfillmentStatusRepository.InsertAsync(status);

        //    string currentDate = DateTime.Now.Year.ToString();
        //    string currentMonth = DateTime.Now.Month.ToString();
        //    string currentDay = DateTime.Now.Day.ToString();
        //    order.InvoiceNumber = "INV-CN" + order.ContactId + "-" + currentDate + currentMonth + currentDay + "-" + orderId;
        //    await _orderRepository.UpdateAsync(order);

        //    foreach (var item in input.OrderItems)
        //    {
        //        var orderItem = ObjectMapper.Map<OrderProductInfo>(item);
        //        orderItem.OrderId = orderId;
        //        long orderProductInfoId = _orderProductInfoRepository.InsertAndGetId(orderItem);
        //        foreach (var category in item.OrderProductVariantCategories)
        //        {
        //            foreach (var type in category.OrderProductVariantList)
        //            {
        //                var orderProductVariantType = new OrderProductVariant();
        //                orderProductVariantType.Price = type.Price != null ? (double)type.Price : 0;
        //                orderProductVariantType.VariantCategoryId = type.VariantCategoryId;
        //                orderProductVariantType.VariantTypeId = type.Id;
        //                orderProductVariantType.OrderProductInfoId = orderProductInfoId;
        //                if (AbpSession.TenantId != null)
        //                {
        //                    orderProductVariantType.TenantId = AbpSession.TenantId;
        //                }
        //                await _orderProductVariantRepository.InsertAsync(orderProductVariantType);
        //            }
        //        }

        //        await UpdateProductStockQuantity(orderItem.ProductId, orderItem.Quantity);
        //    }

        //    var orderPaymentInfo = new OrderPaymentInfo();
        //    orderPaymentInfo.OrderId = orderId;
        //    orderPaymentInfo.PaymentTypeId = input.PaymentTypeId;
        //    _orderPaymentInfoRepository.Insert(orderPaymentInfo);
        //    return orderId;
        //}
        //[AbpAllowAnonymous]
        //public async Task UpdateCustomerRewardPointForOrder(long? contactId, decimal? totalAmount, long orderId)
        //{
        //    var reward = new RewardPointHistory();
        //    if (AbpSession.TenantId != null)
        //    {
        //        reward.TenantId = AbpSession.TenantId;
        //    }
        //    reward.EarnedOrRedeemed = true;
        //    reward.Date = DateTime.Now;
        //    reward.OrderId = orderId;
        //    reward.PointsEarned = (int)Decimal.Round((decimal)totalAmount);
        //    var item = _rewardPointHistoryRepository.GetAll().Where(e => e.ContactId == contactId).OrderByDescending(e => e.Id).FirstOrDefault();
        //    if (item != null && item.PointsBalance != null)
        //    {
        //        reward.PointsBalance += (int)Decimal.Round((decimal)totalAmount);
        //    }
        //    else
        //    {
        //        reward.PointsBalance = (int)Decimal.Round((decimal)totalAmount);
        //    }

        //    reward.RewardPointTypeId = Convert.ToInt32(_appConfiguration["RewardPointTypes:Order_Purchase"]);
        //    reward.ContactId = contactId;
        //    await _rewardPointHistoryRepository.InsertAsync(reward);
        //}
        //[AbpAllowAnonymous]
        //public async Task SendOrderDetailsToCustomer(long orderId)
        //{
        //    var orderDetails = await GetCheckOutDetails(orderId);
        //    string orderProductInfos = null;
        //    // var orderProductInfoIds = orderDetails.Products.Select(e => e.OrderProductInfoId);

        //    foreach (var item in orderDetails.Products)
        //    {
        //        orderProductInfos += "<tr>";
        //        orderProductInfos += "<td style=\"width: 80 %;\">" + System.Environment.NewLine + "<div class=\"display_flex\">";
        //        if (item.ProductPicture != null)
        //        {
        //            orderProductInfos += "<img class=\"images_pr\" src=\"" + item.ProductPicture + "\"" + "alt=\"Image\">";

        //        }
        //        else
        //        {
        //            orderProductInfos += "<img class=\"images_pr\" src=\"" + "http://api.hubsdeal.com/common/images/emailTemplateImages/product_placeholder.png" + "\"" + "alt=\"Image\">";

        //        }

        //        orderProductInfos += "<div class=\"pr_name\">" + System.Environment.NewLine + "<p style =\"display: grid;line-height: 0.85rem;\" >" + System.Environment.NewLine;
        //        orderProductInfos += item.ProducName + " (" + item.UnitPrice.ToString() + "x" + item.Quantity.ToString() + ")" + System.Environment.NewLine;
        //        orderProductInfos += "<small>" + item.StoreName + "</ small >" + System.Environment.NewLine;

        //        var variantCategoryIds = _orderProductVariantRepository.GetAll().Where(e => e.OrderProductInfoId == item.OrderProductInfoId).Select(e => e.VariantCategoryId).Distinct();

        //        foreach (var categoryId in variantCategoryIds)
        //        {
        //            orderProductInfos += "<small>";
        //            var category = await _variantCategoryRepository.FirstOrDefaultAsync(e => e.Id == categoryId);
        //            orderProductInfos += category.Name;
        //            var variantTypes = _orderProductVariantRepository.GetAll().Include(e => e.VariantTypeFk).Where(e => e.VariantCategoryId == categoryId && e.OrderProductInfoId == item.OrderProductInfoId && e.VariantTypeId != null);
        //            orderProductInfos += "(";
        //            // var last = variantTypes.Last();
        //            var count = 0;
        //            foreach (var type in variantTypes)
        //            {
        //                count += 1;
        //                orderProductInfos += type.VariantTypeFk?.Name + "$" + type.Price.ToString();
        //                if (count != variantTypes.Count())
        //                {
        //                    orderProductInfos += ", ";
        //                }
        //            }
        //            orderProductInfos += ")";
        //        }
        //        orderProductInfos += "</p>" + System.Environment.NewLine + "</div>" + System.Environment.NewLine + "</div>" + System.Environment.NewLine + "</td>";
        //        orderProductInfos += "<td style =\"width:20 %;\">" + System.Environment.NewLine;
        //        orderProductInfos += "<div class=\"pr_price\">" + System.Environment.NewLine;
        //        orderProductInfos += "<p style = \"display: grid; line - height: 0.85rem; \" >" + System.Environment.NewLine;
        //        orderProductInfos += "$" + (item.UnitPrice * item.Quantity).ToString() + System.Environment.NewLine;
        //        orderProductInfos += "<small> Price: $" + item.UnitPrice.ToString() + "x" + item.Quantity.ToString() + "</ small >" + System.Environment.NewLine;

        //        var addOnPrice = _orderProductVariantRepository.GetAll().Where(e => e.OrderProductInfoId == item.OrderProductInfoId && e.Price != null).Select(e => e.Price).Sum();
        //        if (addOnPrice > 0 && addOnPrice != null)
        //        {
        //            orderProductInfos += "<small>Add-ons: $" + addOnPrice.ToString() + "</small>" + System.Environment.NewLine;
        //        }
        //        orderProductInfos += "</p>";
        //        orderProductInfos += "</div>";
        //        orderProductInfos += "</td>";
        //        orderProductInfos += "</tr>";

        //    }

        //    try
        //    {
        //        await _userEmailer.SendCustomerOrderDetails(
        //            orderDetails.Email,
        //            orderDetails.Phone,
        //            AppUrlService.CreateEmailUnsubscribeUrlFormat(orderDetails.Email),
        //            orderDetails.Name,
        //            orderId,
        //            orderDetails.CreationTime,
        //            orderDetails.DeliveryAddress,
        //            orderDetails.IsDeliveryOrPickup ? "Delivery" : "Pickup",
        //            orderDetails.OrderStatus,
        //            orderDetails.PaymentType,
        //            orderDetails.IsPaymentCompleted || orderDetails.PaymentTypeId == 1 ? "Completed" : "Pending",
        //            orderProductInfos,
        //            orderDetails.TotalTaxAmount != null ? (double)(orderDetails.TotalAmount - orderDetails.TotalTaxAmount) : (double)orderDetails.TotalAmount,
        //            (double)orderDetails.TotalTaxAmount,
        //            0,
        //            (double)orderDetails.TotalAmount,
        //            orderDetails.OrderInstruction);
        //    }
        //    catch
        //    {

        //    }

        //    try
        //    {
        //        await _smsSender.SendAsync(orderDetails.Phone, "Your order has been placed successfully. Your order tracking No. is: " + orderId + ". Please check your email for more details.");
        //    }
        //    catch
        //    {
        //    }
        //}
        //[AbpAllowAnonymous]
        //public async Task SendOrderDetailsToProductManager(long orderId)
        //{
        //    var orderDetails = await GetCheckOutDetails(orderId);
        //    foreach (var item in orderDetails.Products)
        //    {
        //        var productTeam = _productTeamRepository.FirstOrDefault(e => e.ProductId == item.ProductId && e.Primary == true);
        //        if (productTeam != null && productTeam.EmployeeId != null)
        //        {
        //            var employee = _employeeRepository.FirstOrDefault(e => e.Id == productTeam.EmployeeId);
        //            if (employee.BusinessEmail != null)
        //            {

        //                await _userEmailer.SendOrderDetailToProductManager(employee.BusinessEmail, employee.Name, item.ProducName, orderId, item.Quantity, (double)item.UnitPrice, orderDetails.CreationTime, orderDetails.DeliveryAddress);
        //            }
        //        }
        //    }

        //}
        //[AbpAllowAnonymous]
        //public async Task SendOrderDetailsToStoreOwner(long orderId)
        //{
        //    var orderDetails = await GetCheckOutDetails(orderId);
        //    var total = orderDetails.TotalAmount;
        //    string orderProductInfos = null;
        //    var storeIds = orderDetails.Products.Select(e => e.StoreId).Distinct();
        //    foreach (var id in storeIds)
        //    {
        //        var store = _storeRepository.FirstOrDefault(e => e.Id == id);

        //        orderDetails.TotalAmount = 0;

        //        if (store != null && store.Email != null)
        //        {
        //            orderDetails.Products = orderDetails.Products.Where(e => e.StoreId == store.Id).ToList();

        //            foreach (var item in orderDetails.Products)
        //            {
        //                orderProductInfos += "<tr>";
        //                orderProductInfos += "<td style=\"width: 80 %;\">" + System.Environment.NewLine + "<div class=\"display_flex\">";
        //                if (item.ProductPicture != null)
        //                {
        //                    orderProductInfos += "<img class=\"images_pr\" src=\"" + item.ProductPicture + "\"" + "alt=\"Image\">";

        //                }
        //                else
        //                {
        //                    orderProductInfos += "<img class=\"images_pr\" src=\"" + "http://api.hubsdeal.com/common/images/emailTemplateImages/product_placeholder.png" + "\"" + "alt=\"Image\">";

        //                }

        //                orderProductInfos += "<div class=\"pr_name\">" + System.Environment.NewLine + "<p style =\"display: grid;line-height: 0.85rem;\" >" + System.Environment.NewLine;
        //                orderProductInfos += item.ProducName + " (" + item.UnitPrice.ToString() + "x" + item.Quantity.ToString() + ")" + System.Environment.NewLine;
        //                orderProductInfos += "<small>" + item.StoreName + "</ small >" + System.Environment.NewLine;

        //                var variantCategoryIds = _orderProductVariantRepository.GetAll().Include(e => e.VariantCategoryFk).Where(e => e.OrderProductInfoId == item.OrderProductInfoId).Select(e => e.VariantCategoryId).Distinct();

        //                foreach (var categoryId in variantCategoryIds)
        //                {
        //                    orderProductInfos += "<small>";
        //                    var category = await _variantCategoryRepository.FirstOrDefaultAsync(e => e.Id == categoryId);
        //                    orderProductInfos += category.Name;
        //                    var variantTypes = _orderProductVariantRepository.GetAll().Include(e => e.VariantTypeFk).Where(e => e.VariantCategoryId == categoryId && e.OrderProductInfoId == item.OrderProductInfoId && e.VariantTypeId != null);
        //                    orderProductInfos += "(";
        //                    //var last = variantTypes.Last();
        //                    var count = 0;
        //                    foreach (var type in variantTypes)
        //                    {
        //                        count += 1;
        //                        orderProductInfos += type.VariantTypeFk?.Name + "$" + type.Price.ToString();
        //                        if (count != variantTypes.Count())
        //                        {
        //                            orderProductInfos += ", ";
        //                        }
        //                    }
        //                    orderProductInfos += ")";
        //                }
        //                orderProductInfos += "</p>" + System.Environment.NewLine + "</div>" + System.Environment.NewLine + "</div>" + System.Environment.NewLine + "</td>";
        //                orderProductInfos += "<td style =\"width:20 %;\">" + System.Environment.NewLine;
        //                orderProductInfos += "<div class=\"pr_price\">" + System.Environment.NewLine;
        //                orderProductInfos += "<p style = \"display: grid; line - height: 0.85rem; \" >" + System.Environment.NewLine;
        //                orderProductInfos += "$" + (item.UnitPrice * item.Quantity).ToString() + System.Environment.NewLine;
        //                orderProductInfos += "<small> Price: $" + item.UnitPrice.ToString() + "x" + item.Quantity.ToString() + "</ small >" + System.Environment.NewLine;

        //                var addOnPrice = _orderProductVariantRepository.GetAll().Where(e => e.OrderProductInfoId == item.OrderProductInfoId && e.Price != null).Select(e => e.Price).Sum();
        //                if (addOnPrice > 0 && addOnPrice != null)
        //                {
        //                    orderProductInfos += "<small>Add-ons: $" + addOnPrice.ToString() + "</small>" + System.Environment.NewLine;
        //                }
        //                orderProductInfos += "</p>";
        //                orderProductInfos += "</div>";
        //                orderProductInfos += "</td>";
        //                orderProductInfos += "</tr>";

        //            }
        //            //foreach (var item in products)
        //            //{
        //            //    orderProductInfos += "<tr>";
        //            //    orderProductInfos += "<td>" + count.ToString() + "</td>" + System.Environment.NewLine;
        //            //    orderProductInfos += "<td><span>" + item.ProductFk.Name + "</span></td>" + System.Environment.NewLine;
        //            //    orderProductInfos += "<td>" + item.Quantity.ToString() + "</td>" + System.Environment.NewLine;
        //            //    orderProductInfos += "<td>$" + item.UnitPrice.ToString() + "</td>" + System.Environment.NewLine;
        //            //    orderProductInfos += "<td>$0</td>" + System.Environment.NewLine;
        //            //    orderProductInfos += "<td>$" + item.ByProductTotalAmount.ToString() + "</td>" + System.Environment.NewLine;
        //            //    orderProductInfos += "</tr>" + System.Environment.NewLine;
        //            //    count += 1;
        //            //    orderDetails.TotalAmount += (decimal)item.ByProductTotalAmount;
        //            //}
        //            await _userEmailer.SendOrderDetailToStoreOwner(
        //                store.Email,
        //                (long)orderDetails.OrderId,
        //                orderProductInfos,
        //                orderDetails.Name,
        //                orderDetails.Email,
        //                orderDetails.Phone,
        //                orderDetails.CreationTime,
        //                orderDetails.DeliveryAddress,
        //                orderDetails.TotalTaxAmount != null ? (total - orderDetails.TotalTaxAmount).ToString() : total.ToString(),
        //                orderDetails.TotalTaxAmount != null ? (double)orderDetails.TotalTaxAmount : 0, 0,
        //                total.ToString());
        //        }

        //        try
        //        {
        //            await _smsSender.SendAsync(store.Mobile, "An order has been placed successfully from your store. Order tracking No. is: " + orderId + ". Please check email for more details.");
        //        }
        //        catch
        //        {
        //        }
        //    }

        //}


        //private async Task UpdateProductStockQuantity(long? productId, int? orderQuantity)
        //{
        //    var product = _productRepository.Get((long)productId);
        //    product.StockQuantity = product.StockQuantity - orderQuantity;
        //    await _productRepository.UpdateAsync(product);
        //}
        //[AbpAllowAnonymous]
        //public async Task<List<CityStateLookupTableDto>> GetAllStateForTableDropdown(long countryId)
        //{
        //    return await _stateRepository.GetAll().Where(e => e.CountryId == countryId)
        //        .Select(state => new CityStateLookupTableDto
        //        {
        //            Id = state.Id,
        //            DisplayName = state == null || state.Name == null ? "" : state.Name.ToString()
        //        }).ToListAsync();
        //}
        //[AbpAllowAnonymous]
        //public async Task<List<CityCountyLookupTableDto>> GetAllCountyForTableDropdown()
        //{
        //    return await _countryRepository.GetAll()
        //        .Select(county => new CityCountyLookupTableDto
        //        {
        //            Id = county.Id,
        //            DisplayName = county == null || county.Name == null ? "" : county.Name.ToString()
        //        }).ToListAsync();
        //}
        //[AbpAllowAnonymous]
        //public async Task<List<OrderPaymentInfoPaymentTypeLookupTableDto>> GetAllPaymentTypeForTableDropdown()
        //{
        //    var query = _paymentTypeRepository.GetAll();

        //    var paymentTypeList = await query.ToListAsync();

        //    var lookupTableDtoList = new List<OrderPaymentInfoPaymentTypeLookupTableDto>();
        //    foreach (var paymentType in paymentTypeList)
        //    {
        //        lookupTableDtoList.Add(new OrderPaymentInfoPaymentTypeLookupTableDto
        //        {
        //            Id = paymentType.Id,
        //            DisplayName = paymentType.Name?.ToString()
        //        });
        //    }

        //    return lookupTableDtoList;
        //}
        //[AbpAllowAnonymous]
        //public async Task<GetOrderDetailsForPublicViewDto> GetCheckOutDetails(long orderId)
        //{
        //    var output = new GetOrderDetailsForPublicViewDto();
        //    var order = await _orderRepository.GetAsync(orderId);
        //    output.OrderId = order.Id;
        //    output.CreationTime = order.CreationTime;
        //    output.TotalAmount = (decimal)order.TotalAmount;
        //    output.TotalTaxAmount = (decimal)order.TotalTaxAmount;
        //    output.OrderInstruction = order.Notes;
        //    output.IsDeliveryOrPickup = order.DeliveryOrPickup;
        //    output.City = order.City;
        //    output.ZipCode = order.ZipCode;
        //    output.DeliveryCharge = order.DeliveryCharge;
        //    output.ServiceCharge = order.ServiceCharge;
        //    output.GratuityAmount = order.GratuityAmount;
        //    output.GratuityPercentage = order.GratuityPercentage;

        //    if (order.StateId != null)
        //    {
        //        var state = _stateRepository.Get((long)order.StateId);
        //        output.StateName = state.Name;
        //    }

        //    if (order.OrderStatusTypeId != null && order.DeliveryOrPickup)
        //    {
        //        var status = await _orderStatusTypeRepository.FirstOrDefaultAsync(e => e.Id == order.OrderStatusTypeId);
        //        output.OrderStatus = status.Name;
        //    }
        //    else if (order.OrderPickupStatusTypeId != null && !order.DeliveryOrPickup)
        //    {
        //        var status = await _orderPickupStatusTypeRepository.FirstOrDefaultAsync(e => e.Id == order.OrderPickupStatusTypeId);
        //        output.OrderStatus = status.Name;
        //    }

        //    if (order.CountryId != null)
        //    {
        //        var country = _countryRepository.Get((long)order.CountryId);
        //        output.CountryName = country.Name;
        //    }
        //    output.DeliveryAddress = order.DeliveryAddress;
        //    output.Name = order.FullName;
        //    output.Email = order.Email;
        //    output.Phone = order.Phone;
        //    output.IsPaymentCompleted = order.PaymentCompleted;
        //    var payementInfo = _orderPaymentInfoRepository.FirstOrDefault(e => e.OrderId == order.Id);
        //    if (payementInfo.PaymentTypeId != null)
        //    {
        //        output.PaymentTypeId = payementInfo.PaymentTypeId;
        //        var paymentType = _paymentTypeRepository.Get((long)payementInfo.PaymentTypeId);
        //        output.PaymentType = paymentType.Name;
        //    }
        //    output.Products = await GetCheckOutProducts(order.Id);
        //    return output;
        //}

        //private async Task UpdateWalletForGiftCardPurchase(long? contactId, long orderId)
        //{
        //    double amount = 0;
        //    long? customerWalletId = null;
        //    bool isPurchaseGiftCard = false;
        //    var orderedProducts = _orderProductInfoRepository.GetAll().Where(e => e.OrderId == orderId);

        //    foreach (var item in orderedProducts)
        //    {
        //        if (item.ProductId == (long)ProductEnum.HubsDeal_Gift_Card_500)
        //        {
        //            amount += 500;
        //            isPurchaseGiftCard = true;
        //        }
        //        else if (item.ProductId == (long)ProductEnum.HubsDeal_Gift_Card_200)
        //        {
        //            amount += 200;
        //            isPurchaseGiftCard = true;

        //        }
        //        else if (item.ProductId == (long)ProductEnum.HubsDeal_Gift_Card_100)
        //        {
        //            amount += 100;
        //            isPurchaseGiftCard = true;

        //        }
        //        else if (item.ProductId == (long)ProductEnum.HubsDeal_Gift_Card_50)
        //        {
        //            amount += 50;
        //            isPurchaseGiftCard = true;
        //        }
        //    }

        //    if (isPurchaseGiftCard)
        //    {
        //        var wallet = await _customerWalletRepository.FirstOrDefaultAsync(e => e.ContactId == contactId);
        //        if (wallet == null)
        //        {

        //            var item = new CustomerWallet();
        //            item.WalletOpeningDate = DateTime.Now;
        //            item.BalanceDate = DateTime.Now;
        //            item.ContactId = contactId;
        //            item.BalanceAmount = amount;
        //            customerWalletId = await _customerWalletRepository.InsertAndGetIdAsync(item);
        //        }
        //        else
        //        {
        //            customerWalletId = wallet.Id;
        //            wallet.BalanceDate = DateTime.Now;
        //            wallet.BalanceAmount = wallet.BalanceAmount + (double)amount;
        //            await _customerWalletRepository.UpdateAsync(wallet);
        //        }

        //        if (customerWalletId != null)
        //        {
        //            var transaction = _walletTransactionRepository.GetAll().Where(e => e.CustomerWalletId == customerWalletId).OrderByDescending(e => e.Id).FirstOrDefault();
        //            if (transaction == null)
        //            {
        //                var item = new WalletTransaction();
        //                item.BalanceBeforeTransaction = 0;
        //                item.AddOrDeduct = true;
        //                item.Amount = amount;
        //                item.TransactionDate = DateTime.Now;
        //                item.TransactionTime = DateTime.Now.ToString("hh:mm tt");
        //                item.CurrentBalanceAfterTransaction = amount;
        //                item.WalletTransactionTypeId = 1;
        //                item.CustomerWalletId = customerWalletId;
        //                item.OrderId = orderId;
        //                await _walletTransactionRepository.InsertAsync(item);
        //            }
        //            else
        //            {
        //                var item = new WalletTransaction();
        //                item.BalanceBeforeTransaction = transaction.CurrentBalanceAfterTransaction;
        //                item.AddOrDeduct = true;
        //                item.Amount = amount;
        //                item.TransactionDate = DateTime.Now;
        //                item.TransactionTime = DateTime.Now.ToString("hh:mm tt");
        //                item.CurrentBalanceAfterTransaction = transaction.CurrentBalanceAfterTransaction + amount;
        //                item.WalletTransactionTypeId = 1;
        //                item.CustomerWalletId = customerWalletId;
        //                item.OrderId = orderId;
        //                await _walletTransactionRepository.InsertAsync(item);
        //            }

        //        }
        //    }


        //}

        //private async Task<List<PublicCheckOutProductForViewDto>> GetCheckOutProducts(long orderId)
        //{
        //    var products = new List<PublicCheckOutProductForViewDto>();
        //    var orderProductInfos = _orderProductInfoRepository.GetAll().Include(e => e.ProductFk).Include(e => e.StoreFk).Where(e => e.OrderId == orderId);

        //    foreach (var info in orderProductInfos)
        //    {
        //        var product = new PublicCheckOutProductForViewDto();
        //        product.OrderProductInfoId = info.Id;
        //        product.ProductId = info.ProductFk.Id;
        //        product.ProducName = info.ProductFk.Name;
        //        product.RegularPrice = info.ProductFk.RegularPrice;
        //        product.SalesPrice = info.ProductFk.SalesPrice;
        //        product.MembershipPrice = _membershipAndProductMapRepository.GetAll().Where(e => e.ProductId == info.ProductFk.Id && e.MembershipTypeId == (long)MembershipTypeEnum.Snack_Pass).Select(e => e.Price).FirstOrDefault();
        //        product.MembershipName = _membershipAndProductMapRepository.GetAll().Include(e => e.MembershipTypeFk).Where(e => e.ProductId == product.ProductId && e.MembershipTypeId == (long)MembershipTypeEnum.Snack_Pass).Select(e => e.MembershipTypeFk.Name).FirstOrDefault();
        //        if (info.ProductFk.MediaLibraryId != null)
        //        {
        //            var media = _lookup_mediaLibraryRepository.Get((long)info.ProductFk.MediaLibraryId);
        //            product.ProductPicture = await _binaryObjectManager.GetProductPictureUrlAsync(media.BinaryObjectId, ".png");
        //        }
        //        product.Quantity = (int)info.Quantity;
        //        product.UnitPrice = (decimal)info.UnitPrice;
        //        product.TotalPrice = (decimal)info.ByProductTotalAmount;
        //        product.StoreId = info.StoreId;
        //        product.StoreName = info.StoreFk != null ? info.StoreFk.Name : null;
        //        product.AddOnsPrice = _orderProductVariantRepository.GetAll().Where(e => e.OrderProductInfoId == info.Id && e.Price != null).Select(e => e.Price).Sum();

        //        var variantCategories = _orderProductVariantRepository.GetAll().Where(e => e.OrderProductInfoId == info.Id).Select(e => e.VariantCategoryId).Distinct();
        //        foreach (var category in variantCategories)
        //        {
        //            var variantCategory = new OrderProductVariantCategory();
        //            var item = await _variantCategoryRepository.FirstOrDefaultAsync(e => e.Id == category);
        //            variantCategory.Id = item.Id;
        //            variantCategory.DisplayName = item.Name;
        //            var variantTypes = _orderProductVariantRepository.GetAll().Include(e => e.VariantTypeFk).Where(e => e.VariantCategoryId == category && e.OrderProductInfoId == info.Id);
        //            foreach (var type in variantTypes)
        //            {
        //                var variantType = new OrderProductVariantList();
        //                variantType.Id = type.VariantTypeFk.Id;
        //                variantType.DisplayName = type.VariantTypeFk.Name;
        //                variantCategory.OrderProductVariantList.Add(variantType);
        //            }
        //            product.OrderProductVariantCategories.Add(variantCategory);
        //        }

        //        products.Add(product);
        //    }
        //    return products;
        //}
        //[AbpAllowAnonymous]
        //public async Task UpdateOrderPaymentInfo(long paymentId, long orderId)
        //{


        //    var orderPaymentInfo = await _orderPaymentInfoRepository.FirstOrDefaultAsync(e => e.OrderId == orderId);
        //    if (orderPaymentInfo.PaymentTypeId == null)
        //    {
        //        orderPaymentInfo.PaymentTypeId = paymentId;

        //        await _orderPaymentInfoRepository.UpdateAsync(orderPaymentInfo);

        //        var order = await _orderRepository.FirstOrDefaultAsync(orderId);

        //        if (paymentId == 1)
        //        {
        //            order.PaymentCompleted = true;
        //            await _orderRepository.UpdateAsync(order);
        //        }

        //        await SendOrderDetailsToProductManager(orderId);
        //        await SendOrderDetailsToStoreOwner(orderId);
        //        await SendOrderDetailsToCustomer(orderId);
        //        await UpdateWalletForGiftCardPurchase(order.ContactId, orderId);
        //        await _appNotifier.NewOrderPlacementAsync((long)order.ContactId, orderId);

        //    }
        //}
        //[AbpAllowAnonymous]
        //public async Task<GetSearchedProductsBySpForView> GetProductsBySearchBySp(string filter, int count)
        //{
        //    List<SqlParameter> parameters = PrepareSearchParameterForGetProductsBySearchBySp(filter, count);
        //    var result = await _storedProcedureRepository.ExecuteStoredProcedure<GetSearchedProductsBySpForView>("usp_GetProductsByPublicSearch", CommandType.StoredProcedure, parameters.ToArray());
        //    foreach (var item in result.Products)
        //    {
        //        if (item.PictureId != null)
        //        {
        //            item.Picture = await _binaryObjectManager.GetProductPictureUrlAsync((Guid)item.PictureId, ".png");
        //        }
        //    }
        //    return result;
        //}

        //private static List<SqlParameter> PrepareSearchParameterForGetProductsBySearchBySp(string filterByName, int countItem)
        //{
        //    List<SqlParameter> sqlParameters = new List<SqlParameter>();
        //    if (filterByName != null)
        //    {
        //        filterByName = filterByName[0] == '"' && filterByName[filterByName.Length - 1] == '"' ? "*" + filterByName + "*" : '"' + "*" + filterByName + "*" + '"';
        //    }
        //    SqlParameter filter = new SqlParameter("@Filter", filterByName == null ? "\"\"" : filterByName);
        //    sqlParameters.Add(filter);

        //    SqlParameter count = new SqlParameter("@ReturnItemNumber", countItem);
        //    sqlParameters.Add(count);

        //    return sqlParameters;
        //}
        //[AbpAllowAnonymous]
        //public async Task<TopHubsPublicWidgetMapDto> GetTopHubs(long storeId, long storeWidgetId)
        //{
        //    var output = new TopHubsPublicWidgetMapDto();
        //    var widget = _storeDynamicWidgetMapRepository.FirstOrDefault(e => e.StoreId == storeId && e.StoreWidgetId == storeWidgetId);
        //    if (widget != null)
        //    {
        //        output.StoreDynamicWidgetMap = ObjectMapper.Map<StoreDynamicWidgetMapDto>(widget);
        //    }

        //    //var hubIds = widget != null ? _storeWidgetHubMapRepository.GetAll().Where(e => e.StoreDynamicWidgetMapId == widget.Id).OrderBy(e => e.DisplaySequence).Select(e => e.HubId) : null;
        //    //if (hubIds == null)
        //    //{
        //    //    return output;
        //    //}
        //    var filteredHubs = _storeWidgetHubMapRepository.GetAll()
        //                        .Where(e => e.StoreDynamicWidgetMapId == widget.Id);

        //    var pagedAndFilteredhubs = filteredHubs;

        //    var hubs = from o in pagedAndFilteredhubs
        //               join o1 in _hubRepository.GetAll() on o.HubId equals o1.Id into j1
        //               from s1 in j1.DefaultIfEmpty()
        //               select new TopHubPublicViewDto()
        //               {
        //                   Id = s1.Id,
        //                   Name = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
        //                   Url = s1 == null || s1.Url == null ? "" : s1.Url.ToString(),
        //                   PictureLink = s1.PictureId != null && s1.PictureId != Guid.Empty ? s1.PictureId : Guid.Empty,
        //                   DisplaySequence = o.DisplaySequence

        //               };
        //    var result = await hubs.ToListAsync();
        //    foreach (var hub in result)
        //    {
        //        if (hub.PictureLink != null && hub.PictureLink != Guid.Empty)
        //        {

        //            hub.Picture = await _binaryObjectManager.GetOthersPictureUrlAsync((Guid)hub.PictureLink, ".png");

        //        }

        //    }

        //    output.Hubs = result.OrderBy(e => e.DisplaySequence).ToList();
        //    return output;
        //}

        [AbpAllowAnonymous]
        public async Task<PagedResultDto<TopHubPublicViewDto>> GetAllHubsList(PublicHubsListInput input)
        {



            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            if (input.Filter != null)
            {
                input.Filter = input.Filter[0] == '"' && input.Filter[input.Filter.Length - 1] == '"' ? "*" + input.Filter + "*" : '"' + "*" + input.Filter + "*" + '"';
            }
            SqlParameter filter = new SqlParameter("@Filter", input.Filter == null ? "\"\"" : input.Filter);
            sqlParameters.Add(filter);

            SqlParameter stateFilter = new SqlParameter("@StateFilter", input.StateName == null ? "\"\"" : input.StateName);
            sqlParameters.Add(stateFilter);

            SqlParameter countyFilter = new SqlParameter("@CountyFilter", input.CountyName == null ? "\"\"" : input.CountyName);
            sqlParameters.Add(countyFilter);

            SqlParameter zipCodeFilter = new SqlParameter("@ZipCodeFilter", input.ZipCode == null ? "\"\"" : input.ZipCode);
            sqlParameters.Add(zipCodeFilter);

            SqlParameter cityFilter = new SqlParameter("@CityFilter", input.CityName == null ? "\"\"" : input.CityName);
            sqlParameters.Add(cityFilter);

            SqlParameter skipCount = new SqlParameter("@SkipCount", input.SkipCount);
            sqlParameters.Add(skipCount);

            SqlParameter maxResultCount = new SqlParameter("@MaxResultCount", input.MaxResultCount);
            sqlParameters.Add(maxResultCount);

            SqlParameter storeIdFilter = new SqlParameter("@StoreIdFilter", input.StoreIdFilter == null ? (object)DBNull.Value : input.StoreIdFilter);
            sqlParameters.Add(storeIdFilter);

            SqlParameter countryIdFilter = new SqlParameter("@CountryIdFilter", input.CountryIdFilter == null ? (object)DBNull.Value : input.CountryIdFilter);
            sqlParameters.Add(countryIdFilter);


            GetHubListForPublicDirectoryBySp result = new GetHubListForPublicDirectoryBySp();

            try
            {
                result = await _storedProcedureRepository.ExecuteStoredProcedure<GetHubListForPublicDirectoryBySp>("usp_GetAllHubsForPublicHubDirectory", CommandType.StoredProcedure, sqlParameters.ToArray());

                if (result != null && result.Hubs != null)
                {
                    foreach (var item in result.Hubs)
                    {
                        if (item.PictureLink != null && item.PictureLink != Guid.Empty)
                        {
                            item.Picture = await _binaryObjectManager.GetStorePictureUrlAsync((Guid)item.PictureLink, ".png");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }




            return new PagedResultDto<TopHubPublicViewDto>(
                result.TotalCount,
                result.Hubs
            );
        }
        //[AbpAllowAnonymous]
        //public async Task<GetHubForViewDto> GetHubDetails(string url)
        //{
        //    var hub = await _hubRepository.FirstOrDefaultAsync(e => e.Url.Equals(url));

        //    var output = new GetHubForViewDto { Hub = ObjectMapper.Map<HubDto>(hub) };

        //    if (output.Hub.CityId != null)
        //    {
        //        var _lookupCity = await _cityRepository.FirstOrDefaultAsync((long)output.Hub.CityId);
        //        output.CityName = _lookupCity?.Name?.ToString();
        //    }

        //    if (output.Hub.StateId != null)
        //    {
        //        var _lookupState = await _stateRepository.FirstOrDefaultAsync((long)output.Hub.StateId);
        //        output.StateName = _lookupState?.Name?.ToString();
        //    }

        //    if (output.Hub.CountryId != null)
        //    {
        //        var _lookupCountry = await _countryRepository.FirstOrDefaultAsync((long)output.Hub.CountryId);
        //        output.CountryName = _lookupCountry?.Name?.ToString();
        //    }

        //    if (output.Hub.PictureId != Guid.Empty)
        //    {
        //        output.Picture = await _binaryObjectManager.GetOthersPictureUrlAsync(output.Hub.PictureId, ".png");

        //    }
        //    return output;
        //}

        //[AbpAllowAnonymous]
        //public async Task<List<TopProductCategoryPublicViewDto>> GetAllProductCategoriesByHub(long hubId)
        //{
        //    var productcategoryIdsIds = _hubsProductCategoryRepository.GetAll()
        //                                .Where(e => e.HubId == hubId)
        //                                .OrderBy(e => e.DisplaySequence)
        //                                .Select(e => e.ProductCategoryId);

        //    if (productcategoryIdsIds == null)
        //    {
        //        return null;
        //    }

        //    var filteredProductCategories = _productCategoryRepository.GetAll()
        //                .Include(e => e.MediaLibraryFk)
        //                .WhereIf(productcategoryIdsIds != null, e => productcategoryIdsIds.Contains(e.Id));

        //    var pagedAndFilteredCategories = filteredProductCategories;

        //    var categories = from o in pagedAndFilteredCategories

        //                     join o1 in _lookup_mediaLibraryRepository.GetAll() on o.MediaLibraryId equals o1.Id into j1
        //                     from s1 in j1.DefaultIfEmpty()

        //                     select new TopProductCategoryPublicViewDto()
        //                     {
        //                         Id = o.Id,
        //                         Name = o == null || o.Name == null ? "" : o.Name.ToString(),
        //                         Url = o == null || o.Url == null ? "" : o.Url.ToString(),
        //                         PictureLink = s1.BinaryObjectId != null && s1.BinaryObjectId != Guid.Empty ? s1.BinaryObjectId : Guid.Empty

        //                     };
        //    var results = await categories.ToListAsync();
        //    foreach (var category in results)
        //    {
        //        if (category.PictureLink != null && category.PictureLink != Guid.Empty)
        //        {

        //            category.Picture = await _binaryObjectManager.GetProductPictureUrlAsync((Guid)category.PictureLink, ".png");

        //        }

        //    }

        //    return results;
        //}
        //[AbpAllowAnonymous]
        //public async Task<PagedResultDto<GetPublicProductForViewDto>> GetAllProductsByHub(PublicProductListByHubInput input)
        //{
        //    var filteredProducts = _hubsProductRepository.GetAll()
        //                .Include(e => e.ProductFk)
        //                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ProductFk.Name.Contains(input.Filter))
        //                .Where(e => e.HubId == input.HubId).OrderBy(e => e.DisplaySequence);

        //    var pagedAndFilteredProducts = filteredProducts.PageBy(input);

        //    var products = from o in pagedAndFilteredProducts
        //                   join o1 in _productRepository.GetAll() on o.ProductId equals o1.Id into j1
        //                   from s1 in j1.DefaultIfEmpty()

        //                   join o2 in _productCategoryRepository.GetAll() on s1.ProductCategoryId equals o2.Id into j2
        //                   from s2 in j2.DefaultIfEmpty()

        //                   join o3 in _lookup_mediaLibraryRepository.GetAll() on s1.MediaLibraryId equals o3.Id into j3
        //                   from s3 in j3.DefaultIfEmpty()

        //                   join o4 in _measurementUnitRepository.GetAll() on s1.MeasurementUnitId equals o4.Id into j4
        //                   from s4 in j4.DefaultIfEmpty()

        //                   join o5 in _currencyRepository.GetAll() on s1.CurrencyId equals o5.Id into j5
        //                   from s5 in j5.DefaultIfEmpty()

        //                   join o6 in _ratingLikeRepository.GetAll() on s1.RatingLikeId equals o6.Id into j6
        //                   from s6 in j6.DefaultIfEmpty()

        //                   select new GetPublicProductForViewDto()
        //                   {
        //                       Product = new ProductDto
        //                       {
        //                           Name = s1.Name,
        //                           ShortDescription = s1.ShortDescription,
        //                           Description = s1.Description,
        //                           Sku = s1.Sku,
        //                           Url = s1.Url,
        //                           SeoTitle = s1.SeoTitle,
        //                           MetaKeywords = s1.MetaKeywords,
        //                           MetaDescription = s1.MetaDescription,
        //                           RegularPrice = s1.RegularPrice,
        //                           SalesPrice = s1.SalesPrice,
        //                           PriceDiscountPercentage = s1.PriceDiscountPercentage,
        //                           CallForPrice = s1.CallForPrice,
        //                           UnitPrice = s1.UnitPrice,
        //                           MeasureAmount = s1.MeasureAmount,
        //                           IsTaxExempt = s1.IsTaxExempt,
        //                           StockQuantity = s1.StockQuantity,
        //                           IsDisplayStockQuantity = s1.IsDisplayStockQuantity,
        //                           IsPublished = s1.IsPublished,
        //                           IsPackageProduct = s1.IsPackageProduct,
        //                           InternalNotes = s1.InternalNotes,
        //                           Icon = s1.Icon,
        //                           Id = s1.Id,
        //                           IsWholeSaleProduct = s1.IsWholeSaleProduct,
        //                           MediaLibraryId = s1.MediaLibraryId,
        //                           ProductCategoryId = s1.ProductCategoryId,
        //                           RatingLikeId = s1.RatingLikeId
        //                       },
        //                       ProductCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
        //                       ProductCategoryUrl = s2 == null || s2.Url == null ? "" : s2.Url.ToString(),
        //                       MediaLibraryName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
        //                       MeasurementUnitName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
        //                       CurrencyName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
        //                       CurrencyIcon = s5 == null || s5.Icon == null ? "$" : s5.Icon.ToString(),
        //                       RatingLikeName = s6 == null || s6.Name == null ? "" : s6.Name.ToString(),
        //                       RatingScore = s6 == null || s6.Score == null ? 0 : s6.Score
        //                   };

        //    var results = products.ToList();

        //    foreach (var result in results)
        //    {
        //        if (result.Product.MediaLibraryId != null)
        //        {
        //            var media = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync(e => e.Id == result.Product.MediaLibraryId);
        //            if (media.BinaryObjectId != null)
        //            {
        //                result.Picture = await _binaryObjectManager.GetProductPictureUrlAsync(media.BinaryObjectId, ".png");
        //            }
        //        }

        //        result.HasVariant = await _productByVariantRepository.CountAsync(e => e.ProductId == result.Product.Id) > 0 ? true : false;
        //    }

        //    var totalCount = await filteredProducts.CountAsync();

        //    return new PagedResultDto<GetPublicProductForViewDto>(
        //        totalCount,
        //        results
        //    );
        //}
        //[AbpAllowAnonymous]
        //public async Task<PagedResultDto<PublicStoreListViewDto>> GetAllStoresByHub(PublicStoreListByHubInput input)
        //{
        //    var storeIds = new List<long?>();
        //    if (input.DeliveryTypeId != null)
        //    {
        //        storeIds = _storeDeliveryTypeMapRepository.GetAll().Where(e => e.DeliveryTypeId == input.DeliveryTypeId).Select(e => e.StoreId).ToList();
        //    }

        //    var filteredStores = _hubsStoreRepository.GetAll()
        //                        .Include(e => e.StoreFk)
        //                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.StoreFk.Name.Contains(input.Filter))
        //                        .WhereIf(input.DeliveryTypeId != null, e => storeIds.Contains(e.StoreId))
        //                        .Where(e => e.HubId == input.HubId && e.Published == true);

        //    var pagedAndFilteredStores = filteredStores.OrderBy("displaySequence asc").Take(6);
        //    //.PageBy(input);

        //    var stores = from o in pagedAndFilteredStores

        //                 select new PublicStoreListViewDto()
        //                 {
        //                     Id = o.StoreFk.Id,
        //                     Name = o == null || o.StoreFk.Name == null ? "" : o.StoreFk.Name.ToString(),
        //                     Url = o == null || o.StoreFk.StoreUrl == null ? "" : o.StoreFk.StoreUrl.ToString(),
        //                     StoreLogoLink = o.StoreFk.StoreLogoLink != null && o.StoreFk.StoreLogoLink != Guid.Empty ? o.StoreFk.StoreLogoLink : Guid.Empty,
        //                     DisplaySequence = o.DisplaySequence
        //                 };
        //    var result = await stores.ToListAsync();
        //    foreach (var store in result)
        //    {
        //        if (store.StoreLogoLink != null && store.StoreLogoLink != Guid.Empty)
        //        {
        //            store.Logo = await _binaryObjectManager.GetStorePictureUrlAsync((Guid)store.StoreLogoLink, ".png");
        //        }
        //        store.OpenHour = await GetStoreCurrentOpenStatus((long)store.Id);
        //        var tags = _storeTagRepository.GetAll().Include(e => e.MasterTagFk).Where(e => e.StoreId == store.Id).OrderBy(e => e.DisplaySequence).Take(3);
        //        foreach (var tag in tags)
        //        {
        //            if (tag.MasterTagFk?.Name != null)
        //            {
        //                store.StoreTags.Add(tag.MasterTagFk.Name.ToString());
        //            }
        //            else if (tag.CustomTag != null)
        //            {
        //                store.StoreTags.Add(tag.CustomTag.ToString());
        //            }
        //        }
        //    }

        //    var totalCount = await filteredStores.CountAsync();

        //    return new PagedResultDto<PublicStoreListViewDto>(
        //        totalCount,
        //        result
        //    );
        //}
        //[AbpAllowAnonymous]
        //public async Task<List<TopBrandManufacturerPublicViewDto>> GetBrandManufacturersByHub(long hubId)
        //{
        //    var brandManufacturerIds = _hubsBrandRepository.GetAll().Where(e => e.HubId == hubId).OrderBy(e => e.DisplaySequence).Select(e => e.BrandManufacturerId);

        //    if (brandManufacturerIds == null)
        //    {
        //        return null;
        //    }
        //    var filteredBrandManufacturers = _brandManufacturerRepository.GetAll()
        //                .WhereIf(brandManufacturerIds != null, e => brandManufacturerIds.Contains(e.Id));

        //    var pagedAndFilteredBrandManufacturers = filteredBrandManufacturers;

        //    var topBrands = from o in pagedAndFilteredBrandManufacturers

        //                    select new TopBrandManufacturerPublicViewDto()
        //                    {
        //                        Id = o.Id,
        //                        Name = o == null || o.Name == null ? "" : o.Name.ToString(),
        //                        LogoBinaryObjectId = o.Logo != null && o.Logo != Guid.Empty ? o.Logo : Guid.Empty

        //                    };
        //    var result = await topBrands.ToListAsync();
        //    foreach (var item in result)
        //    {
        //        if (item.LogoBinaryObjectId != null && item.LogoBinaryObjectId != Guid.Empty)
        //        {
        //            item.Logo = await _binaryObjectManager.GetOthersPictureUrlAsync(item.LogoBinaryObjectId, ".png");

        //        }

        //    }

        //    return result;
        //}


        [AbpAllowAnonymous]
        public async Task<List<HubPublicViewForDropdownDto>> GetAllHubForDropdown(string hubFilter, string cityFilter, string zipCodeFilter)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (hubFilter != null)
            {
                hubFilter = hubFilter[0] == '"' && hubFilter[hubFilter.Length - 1] == '"' ? "*" + hubFilter + "*" : '"' + "*" + hubFilter + "*" + '"';
            }

            SqlParameter filter = new SqlParameter("@Filter", hubFilter == null ? "\"\"" : hubFilter);
            parameters.Add(filter);

            if (cityFilter != null)
            {
                cityFilter = cityFilter[0] == '"' && cityFilter[cityFilter.Length - 1] == '"' ? "*" + cityFilter + "*" : '"' + "*" + cityFilter + "*" + '"';
            }

            SqlParameter city = new SqlParameter("@CityFilter", cityFilter == null ? "\"\"" : cityFilter);
            parameters.Add(city);

            if (zipCodeFilter != null)
            {
                zipCodeFilter = zipCodeFilter[0] == '"' && zipCodeFilter[zipCodeFilter.Length - 1] == '"' ? "*" + zipCodeFilter + "*" : '"' + "*" + zipCodeFilter + "*" + '"';
            }

            SqlParameter zipCode = new SqlParameter("@ZipCodeFilter", zipCodeFilter == null ? "\"\"" : zipCodeFilter);
            parameters.Add(zipCode);

            var result = await _storedProcedureRepository.ExecuteStoredProcedure<GetAllHubsForDropdownViewBySp>("usp_GetAllHubsForDropdown", CommandType.StoredProcedure, parameters.ToArray());
            foreach (var item in result.Hubs)
            {
                if (item.PictureId != null && item.PictureId != Guid.Empty)
                {
                    item.Picture = await _binaryObjectManager.GetStorePictureUrlAsync((Guid)item.PictureId, ".png");
                }
            }
            return result.Hubs;
        }


        //[AbpAllowAnonymous]
        //public async Task<List<HubPublicViewForDropdownDto>> GetAllHubForDropdownByCity(string cityFilter)
        //{
        //    List<SqlParameter> parameters = new List<SqlParameter>();

        //    if (cityFilter != null)
        //    {
        //        cityFilter = cityFilter[0] == '"' && cityFilter[cityFilter.Length - 1] == '"' ? "*" + cityFilter + "*" : '"' + "*" + cityFilter + "*" + '"';
        //    }

        //    SqlParameter city = new SqlParameter("@CityFilter", cityFilter == null ? "\"\"" : cityFilter);
        //    parameters.Add(city);



        //    var result = await _storedProcedureRepository.ExecuteStoredProcedure<GetAllHubsForDropdownViewBySp>("usp_GetAllHubsForDropdown", CommandType.StoredProcedure, parameters.ToArray());
        //    foreach (var item in result.Hubs)
        //    {
        //        if (item.PictureId != null)
        //        {
        //            item.Picture = await _binaryObjectManager.GetStorePictureUrlAsync((Guid)item.PictureId, ".png");
        //        }
        //    }
        //    return result.Hubs;
        //}
        //[AbpAllowAnonymous]
        //public async Task<List<HubPublicViewForDropdownDto>> GetAllHubForDropdownByZipCode(string zipCodeFilter)
        //{
        //    List<SqlParameter> parameters = new List<SqlParameter>();

        //    if (zipCodeFilter != null)
        //    {
        //        zipCodeFilter = zipCodeFilter[0] == '"' && zipCodeFilter[zipCodeFilter.Length - 1] == '"' ? "*" + zipCodeFilter + "*" : '"' + "*" + zipCodeFilter + "*" + '"';
        //    }

        //    SqlParameter zipCode = new SqlParameter("@ZipCodeFilter", zipCodeFilter == null ? "\"\"" : zipCodeFilter);
        //    parameters.Add(zipCode);

        //    var result = await _storedProcedureRepository.ExecuteStoredProcedure<GetAllHubsForDropdownViewBySp>("usp_GetAllHubsForDropdown", CommandType.StoredProcedure, parameters.ToArray());
        //    foreach (var item in result.Hubs)
        //    {
        //        if (item.PictureId != null)
        //        {
        //            item.Picture = await _binaryObjectManager.GetStorePictureUrlAsync((Guid)item.PictureId, ".png");
        //        }
        //    }
        //    return result.Hubs;
        //}
        //[AbpAllowAnonymous]
        //public async Task<List<HubPublicViewForDropdownDto>> GetTopHubsForDropdown()
        //{
        //    var query = _hubRepository.GetAll().Where(e => e.Live == true).Take(5);

        //    var hubList = await query.ToListAsync();

        //    var lookupTableDtoList = new List<HubPublicViewForDropdownDto>();
        //    foreach (var hub in hubList)
        //    {
        //        lookupTableDtoList.Add(new HubPublicViewForDropdownDto
        //        {
        //            Id = hub.Id,
        //            DisplayName = hub.Name?.ToString(),
        //            Url = hub.Url?.ToString(),
        //            Picture = hub.PictureId != Guid.Empty ? await _binaryObjectManager.GetOthersPictureUrlAsync(hub.PictureId, ".png") : null
        //        });
        //    }

        //    return lookupTableDtoList;
        //}
        //[AbpAllowAnonymous]
        //public async Task<List<RelatedProductCategoryPublicViewDto>> GetAllRelatedProductCategory(long productCategoryId)
        //{
        //    var category = await _productCategoryRepository.FirstOrDefaultAsync(e => e.Id == productCategoryId);

        //    long[] ids = null;

        //    if (category.HasParentCategory)
        //    {
        //        ids = _productCategoryRepository.GetAll().Where(e => e.ParentCategoryId == category.ParentCategoryId && e.Id != productCategoryId).Select(e => e.Id).ToArray();
        //    }
        //    else
        //    {
        //        ids = _productCategoryRepository.GetAll().Where(e => e.ParentCategoryId == productCategoryId).Select(e => e.Id).ToArray();
        //    }

        //    if (ids == null)
        //    {
        //        return null;
        //    }

        //    var results = await _productCategoryRepository.GetAll()
        //        .Where(e => ids.Contains(e.Id))
        //        .Select(productCategory => new RelatedProductCategoryPublicViewDto
        //        {
        //            Id = productCategory.Id,
        //            Name = productCategory == null || productCategory.Name == null ? "" : productCategory.Name.ToString(),
        //            Url = productCategory == null || productCategory.Url == null ? "" : productCategory.Url.ToString(),
        //            MediaLibraryId = productCategory == null || productCategory.MediaLibraryId == null ? null : productCategory.MediaLibraryId
        //        }).ToListAsync();
        //    foreach (var result in results)
        //    {
        //        if (result.MediaLibraryId != null)
        //        {
        //            var media = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync(e => e.Id == result.MediaLibraryId);
        //            if (media.BinaryObjectId != null && media.BinaryObjectId != Guid.Empty)
        //            {
        //                result.Picture = await _binaryObjectManager.GetProductPictureUrlAsync(media.BinaryObjectId, ".png");
        //            }
        //        }
        //    }

        //    return results;
        //}
        //[AbpAllowAnonymous]
        //public async Task<GetSearchedStoresBySpForView> GetStoresBySearchBySp(string filter, int count)
        //{
        //    List<SqlParameter> parameters = PrepareSearchParameterForGetStoresBySearchBySp(filter, count);
        //    var result = await _storedProcedureRepository.ExecuteStoredProcedure<GetSearchedStoresBySpForView>("usp_GetStoresByPublicSearch", CommandType.StoredProcedure, parameters.ToArray());
        //    foreach (var item in result.Stores)
        //    {
        //        if (item.PictureId != null)
        //        {
        //            item.Picture = await _binaryObjectManager.GetStorePictureUrlAsync((Guid)item.PictureId, ".png");
        //        }
        //    }
        //    return result;
        //}

        //private static List<SqlParameter> PrepareSearchParameterForGetStoresBySearchBySp(string filterByName, int countItem)
        //{
        //    List<SqlParameter> sqlParameters = new List<SqlParameter>();
        //    if (filterByName != null)
        //    {
        //        filterByName = filterByName[0] == '"' && filterByName[filterByName.Length - 1] == '"' ? "*" + filterByName + "*" : '"' + "*" + filterByName + "*" + '"';
        //    }
        //    SqlParameter filter = new SqlParameter("@Filter", filterByName == null ? "\"\"" : filterByName);
        //    sqlParameters.Add(filter);

        //    SqlParameter count = new SqlParameter("@ReturnItemNumber", countItem);
        //    sqlParameters.Add(count);

        //    return sqlParameters;
        //}
        //[AbpAllowAnonymous]
        //public async Task<GetVariantsByOrderProduct> GetOrderProductVariants(long productId)
        //{
        //    var output = new GetVariantsByOrderProduct();


        //    var product = await _productRepository.FirstOrDefaultAsync(e => e.Id == productId);

        //    if (product != null)
        //    {
        //        output.ProductId = product.Id;
        //        output.ProductName = product.Name;
        //        if (product.MediaLibraryId != null)
        //        {
        //            var media = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync(e => e.Id == product.MediaLibraryId);
        //            output.ProductPicture = await _binaryObjectManager.GetProductPictureUrlAsync(media.BinaryObjectId, ".png");
        //        }
        //    }
        //    var productVariants = _productByVariantRepository.GetAll().Where(e => e.ProductId == productId)
        //        .Include(e => e.VariantCategoryFk)
        //        .Include(e => e.VariantTypeFk);

        //    var variantCategoryIds = productVariants.Select(e => e.VariantCategoryFk.Id).Distinct();
        //    foreach (var categoryId in variantCategoryIds)
        //    {
        //        var variantCategory = new OrderProductVariantCategory();
        //        var category = await _variantCategoryRepository.FirstOrDefaultAsync(e => e.Id == categoryId);
        //        variantCategory.Id = category.Id;
        //        variantCategory.DisplayName = category.Name;
        //        var variantTypeIds = productVariants.Where(e => e.VariantTypeFk.VariantCategoryId == categoryId).Select(e => e.VariantTypeFk.Id);
        //        foreach (var typeId in variantTypeIds)
        //        {
        //            var variantType = new OrderProductVariantList();
        //            var type = await _variantTypeRepository.FirstOrDefaultAsync(e => e.Id == typeId);
        //            variantType.Id = type.Id;
        //            variantType.DisplayName = type.Name;
        //            variantType.VariantCategoryId = categoryId;
        //            var price = _productByVariantRepository.GetAll().Where(e => e.VariantTypeId == typeId && e.ProductId == productId).Select(e => e.Price).FirstOrDefault();
        //            if (price != null)
        //            {
        //                variantType.Price = price;
        //            }
        //            else
        //            {
        //                variantType.Price = 0;
        //            }

        //            //variantType.Selected = _orderProductVariantRepository.GetAll().Where(e => e.OrderProductInfoId == orderProductInfo.Id && e.VariantTypeId == typeId).FirstOrDefault() == null ? false : true;
        //            variantCategory.OrderProductVariantList.Add(variantType);
        //        }
        //        output.OrderProductVariantCategories.Add(variantCategory);
        //    }

        //    return output;
        //}
        //[AbpAllowAnonymous]
        //public async Task<List<OrderProductVariantCategory>> GetVariantWithCategoriesForCart(OrderProductVariantList[] inputs)
        //{
        //    var output = new List<OrderProductVariantCategory>();
        //    var variantCategoryIds = inputs.Select(e => e.VariantCategoryId).Distinct();
        //    foreach (var categoryId in variantCategoryIds)
        //    {
        //        var variantCategory = new OrderProductVariantCategory();
        //        var category = await _variantCategoryRepository.FirstOrDefaultAsync(e => e.Id == categoryId);
        //        variantCategory.Id = category.Id;
        //        variantCategory.DisplayName = category.Name;
        //        var variantTypeIds = inputs.Where(e => e.VariantCategoryId == categoryId).Select(e => e.Id);
        //        foreach (var typeId in variantTypeIds)
        //        {
        //            var variantType = new OrderProductVariantList();
        //            var type = await _variantTypeRepository.FirstOrDefaultAsync(e => e.Id == typeId);
        //            variantType.Id = type.Id;
        //            variantType.DisplayName = type.Name;
        //            variantType.VariantCategoryId = categoryId;
        //            var price = inputs.Where(e => e.Id == typeId && e.VariantCategoryId == categoryId).Select(e => e.Price).FirstOrDefault();
        //            if (price != null)
        //            {
        //                variantType.Price = price;
        //            }
        //            else
        //            {
        //                variantType.Price = 0;
        //            }
        //            //variantType.Selected = _orderProductVariantRepository.GetAll().Where(e => e.OrderProductInfoId == orderProductInfo.Id && e.VariantTypeId == typeId).FirstOrDefault() == null ? false : true;
        //            variantCategory.OrderProductVariantList.Add(variantType);
        //        }
        //        output.Add(variantCategory);
        //    }
        //    return output;
        //}
        //[AbpAllowAnonymous]
        //public async Task<GetStoreHourCurrentStatusForPublicViewDto> GetStoreCurrentOpenStatus(long storeId)
        //{
        //    var output = new GetStoreHourCurrentStatusForPublicViewDto();

        //    var storeOpenHour = await _storeHourRepository.FirstOrDefaultAsync(e => e.StoreId == storeId);

        //    if (storeOpenHour != null)
        //    {
        //        if (storeOpenHour.NowOpenOrClosed || storeOpenHour.IsOpen24Hours)
        //        {
        //            output.IsOpen = true;
        //            return output;
        //        }
        //        var weekday = DateTime.Today.DayOfWeek;
        //        output.CurrentTime = DateTime.Now.ToString("h:mm tt");

        //        if (weekday.ToString() == "Monday")
        //        {
        //            output.StartTime = storeOpenHour.MondayStartTime;
        //            output.EndTime = storeOpenHour.MondayEndTime;
        //        }
        //        else if (weekday.ToString() == "Tuesday")
        //        {
        //            output.StartTime = storeOpenHour.TuesdayStartTime;
        //            output.EndTime = storeOpenHour.TuesdayEndTime;
        //        }
        //        else if (weekday.ToString() == "Wednesday")
        //        {
        //            output.StartTime = storeOpenHour.WednesdayStartTime;
        //            output.EndTime = storeOpenHour.WednesdayEndTime;
        //        }
        //        else if (weekday.ToString() == "Thursday")
        //        {
        //            output.StartTime = storeOpenHour.ThursdayStartTime;
        //            output.EndTime = storeOpenHour.ThursdayEndTime;
        //        }
        //        else if (weekday.ToString() == "Friday")
        //        {
        //            output.StartTime = storeOpenHour.FridayStartTime;
        //            output.EndTime = storeOpenHour.FridayEndTime;
        //        }
        //        else if (weekday.ToString() == "Saturday")
        //        {
        //            output.StartTime = storeOpenHour.SaturdayStartTime;
        //            output.EndTime = storeOpenHour.SaturdayEndTime;
        //        }
        //        else if (weekday.ToString() == "Sunday")
        //        {
        //            output.StartTime = storeOpenHour.SundayStartTime;
        //            output.EndTime = storeOpenHour.SundayEndTime;
        //        }
        //    }

        //    return output;

        //}

        //[AbpAllowAnonymous]
        //public async Task<List<GetPublicStoreCategoriesForViewDto>> GetCategoryWiseProductsByHub(long? hubId, int skipCount)
        //{
        //    List<SqlParameter> sqlParameters = new List<SqlParameter>();

        //    SqlParameter sCount = new SqlParameter("@SkipCount", skipCount);
        //    sqlParameters.Add(sCount);

        //    SqlParameter maxResultCount = new SqlParameter("@MaxResultCount", 2);
        //    sqlParameters.Add(maxResultCount);

        //    SqlParameter hubIdFilter = new SqlParameter("@HubIdFilter", hubId);
        //    sqlParameters.Add(hubIdFilter);

        //    var results = await _storedProcedureRepository.ExecuteStoredProcedure<GetPublicCategoryWiseProductsForViewDto>("usp_GetCategoryWiseProductsByHub", CommandType.StoredProcedure, sqlParameters.ToArray());



        //    foreach (var item in results.Categories)
        //    {
        //        var category = await _productCategoryRepository.FirstOrDefaultAsync(e => e.Id == item.Id);
        //        if (category.MediaLibraryId != null)
        //        {
        //            var media = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync(e => e.Id == category.MediaLibraryId);
        //            if (media.BinaryObjectId != null)
        //            {
        //                item.Picture = await _binaryObjectManager.GetProductPictureUrlAsync(media.BinaryObjectId, ".png");
        //            }
        //        }
        //        if (item.Products != null && item.Products.Count > 0)
        //        {
        //            foreach (var result in item.Products)
        //            {
        //                var store = await _storeProductMapRepository.GetAll().Include(e => e.StoreFk).Where(e => e.ProductId == result.Product.Id).FirstOrDefaultAsync();
        //                if (store != null)
        //                {
        //                    result.StoreId = store.StoreFk.Id;
        //                    result.StoreName = store.StoreFk.Name;
        //                    result.StoreUrl = store.StoreFk.StoreUrl;
        //                    result.StoreLogo = store.StoreFk.StoreLogoLink != Guid.Empty ? await _binaryObjectManager.GetStorePictureUrlAsync(store.StoreFk.StoreLogoLink, ".png") : null;
        //                }

        //                if (result.Product.MediaLibraryId != null)
        //                {
        //                    var media = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync(e => e.Id == result.Product.MediaLibraryId);
        //                    if (media.BinaryObjectId != null)
        //                    {
        //                        result.Picture = await _binaryObjectManager.GetProductPictureUrlAsync(media.BinaryObjectId, ".png");
        //                    }
        //                }

        //                result.HasVariant = await _productByVariantRepository.CountAsync(e => e.ProductId == result.Product.Id) > 0 ? true : false;
        //                result.MembershipPrice = _membershipAndProductMapRepository.GetAll().Where(e => e.ProductId == result.Product.Id && e.MembershipTypeId == (long)MembershipTypeEnum.Snack_Pass).Select(e => e.Price).FirstOrDefault();
        //                result.MembershipName = _membershipAndProductMapRepository.GetAll().Include(e => e.MembershipTypeFk).Where(e => e.ProductId == result.Product.Id && e.MembershipTypeId == (long)MembershipTypeEnum.Snack_Pass).Select(e => e.MembershipTypeFk.Name).FirstOrDefault();
        //            }
        //        }
        //    }


        //    return results.Categories;
        //}


        [AbpAllowAnonymous]
        public async Task<GetNearestHubsForViewDto> GetNearestHubsByUserLocation(GeUserLatLongInpuDto input)
        {
            List<SqlParameter> parameters = PrepareSearchParameterForGetNearestHubsByUserLocation(input);
            var results = await _storedProcedureRepository.ExecuteStoredProcedure<GetNearestHubsForViewDto>("usp_GetTopNearbyHubsByUserLocation", CommandType.StoredProcedure, parameters.ToArray());
            foreach (var result in results.NearestHubs)
            {
                if (result.PictureId != null && result.PictureId != Guid.Empty)
                {
                    result.Picture = await _binaryObjectManager.GetOthersPictureUrlAsync((Guid)result.PictureId, ".png");

                }
            }
            return results;
        }


        private static List<SqlParameter> PrepareSearchParameterForGetNearestHubsByUserLocation(GeUserLatLongInpuDto input)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();


            SqlParameter latitudeFilter = new SqlParameter("@Latitude", input.Latitude == null ? (object)DBNull.Value : input.Latitude);
            sqlParameters.Add(latitudeFilter);

            SqlParameter longitudeFilter = new SqlParameter("@Longitude", input.Longitude == null ? (object)DBNull.Value : input.Longitude);
            sqlParameters.Add(longitudeFilter);

            return sqlParameters;
        }

        //[AbpAllowAnonymous]
        //public async Task<PagedResultDto<GetPublicProductForViewDto>> GetAllProductsByHubAndProductCategory(string hubUrl, string categoryUrl, string filter)
        //{
        //    var hubId = _hubRepository.GetAll().Where(e => e.Url == hubUrl).Select(e => e.Id).FirstOrDefault();
        //    var categoryId = _productCategoryRepository.GetAll().Where(e => e.Url == categoryUrl).Select(e => e.Id).FirstOrDefault();

        //    var filteredProducts = _hubsProductRepository.GetAll()
        //                .Include(e => e.ProductFk)
        //                .WhereIf(!string.IsNullOrWhiteSpace(filter), e => false || e.ProductFk.Name.Contains(filter))
        //                .Where(e => e.HubId == hubId).Where(e => e.ProductFk.ProductCategoryId == categoryId).OrderBy(e => e.DisplaySequence);

        //    var pagedAndFilteredProducts = filteredProducts;

        //    var products = from o in pagedAndFilteredProducts
        //                   join o1 in _productRepository.GetAll() on o.ProductId equals o1.Id into j1
        //                   from s1 in j1.DefaultIfEmpty()

        //                   join o2 in _productCategoryRepository.GetAll() on s1.ProductCategoryId equals o2.Id into j2
        //                   from s2 in j2.DefaultIfEmpty()

        //                   join o3 in _lookup_mediaLibraryRepository.GetAll() on s1.MediaLibraryId equals o3.Id into j3
        //                   from s3 in j3.DefaultIfEmpty()

        //                   join o4 in _measurementUnitRepository.GetAll() on s1.MeasurementUnitId equals o4.Id into j4
        //                   from s4 in j4.DefaultIfEmpty()

        //                   join o5 in _currencyRepository.GetAll() on s1.CurrencyId equals o5.Id into j5
        //                   from s5 in j5.DefaultIfEmpty()

        //                   join o6 in _ratingLikeRepository.GetAll() on s1.RatingLikeId equals o6.Id into j6
        //                   from s6 in j6.DefaultIfEmpty()

        //                   select new GetPublicProductForViewDto()
        //                   {
        //                       Product = new ProductDto
        //                       {
        //                           Name = s1.Name,
        //                           ShortDescription = s1.ShortDescription,
        //                           Description = s1.Description,
        //                           Sku = s1.Sku,
        //                           Url = s1.Url,
        //                           SeoTitle = s1.SeoTitle,
        //                           MetaKeywords = s1.MetaKeywords,
        //                           MetaDescription = s1.MetaDescription,
        //                           RegularPrice = s1.RegularPrice,
        //                           SalesPrice = s1.SalesPrice,
        //                           PriceDiscountPercentage = s1.PriceDiscountPercentage,
        //                           CallForPrice = s1.CallForPrice,
        //                           UnitPrice = s1.UnitPrice,
        //                           MeasureAmount = s1.MeasureAmount,
        //                           IsTaxExempt = s1.IsTaxExempt,
        //                           StockQuantity = s1.StockQuantity,
        //                           IsDisplayStockQuantity = s1.IsDisplayStockQuantity,
        //                           IsPublished = s1.IsPublished,
        //                           IsPackageProduct = s1.IsPackageProduct,
        //                           InternalNotes = s1.InternalNotes,
        //                           Icon = s1.Icon,
        //                           Id = s1.Id,
        //                           IsWholeSaleProduct = s1.IsWholeSaleProduct,
        //                           MediaLibraryId = s1.MediaLibraryId,
        //                           ProductCategoryId = s1.ProductCategoryId,
        //                           RatingLikeId = s1.RatingLikeId
        //                       },
        //                       ProductCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
        //                       ProductCategoryUrl = s2 == null || s2.Url == null ? "" : s2.Url.ToString(),
        //                       MediaLibraryName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
        //                       MeasurementUnitName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
        //                       CurrencyName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
        //                       CurrencyIcon = s5 == null || s5.Icon == null ? "$" : s5.Icon.ToString(),
        //                       RatingLikeName = s6 == null || s6.Name == null ? "" : s6.Name.ToString(),
        //                       RatingScore = s6 == null || s6.Score == null ? 0 : s6.Score
        //                   };

        //    var results = products.ToList();

        //    foreach (var result in results)
        //    {

        //        var tags = _productTagRepository.GetAll().Include(e => e.MasterTagFk)
        //                            .Where(e => e.ProductId == result.Product.Id).OrderBy(e => e.DisplaySequenceNumber).Take(3);

        //        foreach (var tag in tags)
        //        {
        //            if (tag.MasterTagFk != null && tag.MasterTagFk.Name != null)
        //            {
        //                result.ProductTags.Add(tag.MasterTagFk.Name);
        //            }
        //            else if (tag.CustomTag != null)
        //            {
        //                result.ProductTags.Add(tag.CustomTag);
        //            }
        //        }

        //        var store = await _storeProductMapRepository.GetAll().Include(e => e.StoreFk).Where(e => e.ProductId == result.Product.Id).FirstOrDefaultAsync();
        //        if (store != null)
        //        {
        //            result.StoreId = store.StoreFk.Id;
        //            result.StoreName = store.StoreFk.Name;
        //            result.StoreLogo = store.StoreFk.StoreLogoLink != Guid.Empty ? await _binaryObjectManager.GetStorePictureUrlAsync(store.StoreFk.StoreLogoLink, ".png") : null;
        //        }

        //        if (result.Product.MediaLibraryId != null)
        //        {
        //            var media = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync(e => e.Id == result.Product.MediaLibraryId);
        //            if (media.BinaryObjectId != null)
        //            {
        //                result.Picture = await _binaryObjectManager.GetProductPictureUrlAsync(media.BinaryObjectId, ".png");
        //            }
        //        }
        //        result.HasVariant = await _productByVariantRepository.CountAsync(e => e.ProductId == result.Product.Id) > 0 ? true : false;
        //        result.MembershipPrice = _membershipAndProductMapRepository.GetAll().Where(e => e.ProductId == result.Product.Id && e.MembershipTypeId == (long)MembershipTypeEnum.Snack_Pass).Select(e => e.Price).FirstOrDefault();
        //        result.MembershipName = _membershipAndProductMapRepository.GetAll().Include(e => e.MembershipTypeFk).Where(e => e.ProductId == result.Product.Id && e.MembershipTypeId == (long)MembershipTypeEnum.Snack_Pass).Select(e => e.MembershipTypeFk.Name).FirstOrDefault();
        //    }

        //    var totalCount = await filteredProducts.CountAsync();

        //    return new PagedResultDto<GetPublicProductForViewDto>(
        //        totalCount,
        //        results
        //    );
        //}
        //[AbpAllowAnonymous]
        //public async Task<PagedResultDto<GetPublicProductForViewDto>> GetAllProductsByStoreAndProductCategory(string storeUrl, string categoryUrl, string filter)
        //{
        //    var storeId = _storeRepository.GetAll().Where(e => e.StoreUrl == storeUrl).Select(e => e.Id).FirstOrDefault();
        //    var categoryId = _productCategoryRepository.GetAll().Where(e => e.Url == categoryUrl).Select(e => e.Id).FirstOrDefault();

        //    var filteredProducts = _storeProductMapRepository.GetAll()
        //                .Include(e => e.ProductFk)
        //                .WhereIf(!string.IsNullOrWhiteSpace(filter), e => false || e.ProductFk.Name.Contains(filter))
        //                .Where(e => e.StoreId == storeId).Where(e => e.ProductFk.ProductCategoryId == categoryId).OrderBy(e => e.DisplaySequence);

        //    var pagedAndFilteredProducts = filteredProducts;

        //    var products = from o in pagedAndFilteredProducts
        //                   join o1 in _productRepository.GetAll() on o.ProductId equals o1.Id into j1
        //                   from s1 in j1.DefaultIfEmpty()

        //                   join o2 in _productCategoryRepository.GetAll() on s1.ProductCategoryId equals o2.Id into j2
        //                   from s2 in j2.DefaultIfEmpty()

        //                   join o3 in _lookup_mediaLibraryRepository.GetAll() on s1.MediaLibraryId equals o3.Id into j3
        //                   from s3 in j3.DefaultIfEmpty()

        //                   join o4 in _measurementUnitRepository.GetAll() on s1.MeasurementUnitId equals o4.Id into j4
        //                   from s4 in j4.DefaultIfEmpty()

        //                   join o5 in _currencyRepository.GetAll() on s1.CurrencyId equals o5.Id into j5
        //                   from s5 in j5.DefaultIfEmpty()

        //                   join o6 in _ratingLikeRepository.GetAll() on s1.RatingLikeId equals o6.Id into j6
        //                   from s6 in j6.DefaultIfEmpty()

        //                   select new GetPublicProductForViewDto()
        //                   {
        //                       Product = new ProductDto
        //                       {
        //                           Name = s1.Name,
        //                           ShortDescription = s1.ShortDescription,
        //                           Description = s1.Description,
        //                           Sku = s1.Sku,
        //                           Url = s1.Url,
        //                           SeoTitle = s1.SeoTitle,
        //                           MetaKeywords = s1.MetaKeywords,
        //                           MetaDescription = s1.MetaDescription,
        //                           RegularPrice = s1.RegularPrice,
        //                           SalesPrice = s1.SalesPrice,
        //                           PriceDiscountPercentage = s1.PriceDiscountPercentage,
        //                           CallForPrice = s1.CallForPrice,
        //                           UnitPrice = s1.UnitPrice,
        //                           MeasureAmount = s1.MeasureAmount,
        //                           IsTaxExempt = s1.IsTaxExempt,
        //                           StockQuantity = s1.StockQuantity,
        //                           IsDisplayStockQuantity = s1.IsDisplayStockQuantity,
        //                           IsPublished = s1.IsPublished,
        //                           IsPackageProduct = s1.IsPackageProduct,
        //                           InternalNotes = s1.InternalNotes,
        //                           Icon = s1.Icon,
        //                           Id = s1.Id,
        //                           IsWholeSaleProduct = s1.IsWholeSaleProduct,
        //                           MediaLibraryId = s1.MediaLibraryId,
        //                           ProductCategoryId = s1.ProductCategoryId,
        //                           RatingLikeId = s1.RatingLikeId
        //                       },
        //                       ProductCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
        //                       ProductCategoryUrl = s2 == null || s2.Url == null ? "" : s2.Url.ToString(),
        //                       MediaLibraryName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
        //                       MeasurementUnitName = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
        //                       CurrencyName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
        //                       CurrencyIcon = s5 == null || s5.Icon == null ? "$" : s5.Icon.ToString(),
        //                       RatingLikeName = s6 == null || s6.Name == null ? "" : s6.Name.ToString(),
        //                       RatingScore = s6 == null || s6.Score == null ? 0 : s6.Score,
        //                       StoreId = storeId
        //                   };

        //    var results = products.ToList();

        //    foreach (var result in results)
        //    {
        //        if (result.Product.MediaLibraryId != null)
        //        {
        //            var media = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync(e => e.Id == result.Product.MediaLibraryId);
        //            if (media.BinaryObjectId != null)
        //            {
        //                result.Picture = await _binaryObjectManager.GetProductPictureUrlAsync(media.BinaryObjectId, ".png");
        //            }
        //        }

        //        result.HasVariant = await _productByVariantRepository.CountAsync(e => e.ProductId == result.Product.Id) > 0 ? true : false;
        //        result.MembershipPrice = _membershipAndProductMapRepository.GetAll().Where(e => e.ProductId == result.Product.Id && e.MembershipTypeId == (long)MembershipTypeEnum.Snack_Pass).Select(e => e.Price).FirstOrDefault();
        //        result.MembershipName = _membershipAndProductMapRepository.GetAll().Include(e => e.MembershipTypeFk).Where(e => e.ProductId == result.Product.Id && e.MembershipTypeId == (long)MembershipTypeEnum.Snack_Pass).Select(e => e.MembershipTypeFk.Name).FirstOrDefault();
        //    }

        //    var totalCount = await filteredProducts.CountAsync();

        //    return new PagedResultDto<GetPublicProductForViewDto>(
        //        totalCount,
        //        results
        //    );
        //}
        //[AbpAllowAnonymous]
        //public async Task<GetRandomProductByHub> GetRandomProductByHub(long hubId)
        //{
        //    var output = new GetRandomProductByHub();

        //    Random r = new Random();
        //    var products = _hubsProductRepository.GetAll().Include(e => e.ProductFk).Where(e => e.HubId == hubId && e.ProductId != null).ToArray();
        //    int randomIndex = r.Next(0, products.Length);
        //    var randomItem = products[randomIndex];

        //    output.ProductId = randomItem.ProductFk.Id;
        //    output.ProductName = randomItem.ProductFk.Name;
        //    output.Url = randomItem.ProductFk.Url;

        //    if (randomItem.ProductFk.MediaLibraryId != null)
        //    {
        //        var media = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)randomItem.ProductFk.MediaLibraryId);
        //        if (media.BinaryObjectId != Guid.Empty)
        //        {
        //            output.Picture = await _binaryObjectManager.GetProductPictureUrlAsync(media.BinaryObjectId, ".png");
        //        }

        //    }

        //    return output;
        //}
        //[AbpAllowAnonymous]
        //public async Task<string> GetTermsOfUse()
        //{
        //    var content = await _contentRepository.FirstOrDefaultAsync(e => e.Id == Convert.ToInt32(_appConfiguration["LegalDocs:Terms"]) && e.IsPublished == true);

        //    if (content != null)
        //    {
        //        return content.Body;
        //    }

        //    return null;
        //}
        //[AbpAllowAnonymous]
        //public async Task<string> GetPrivacyPolicy()
        //{
        //    var content = await _contentRepository.FirstOrDefaultAsync(e => e.Id == Convert.ToInt32(_appConfiguration["LegalDocs:Privacy_Policy"]) && e.IsPublished == true);

        //    if (content != null)
        //    {
        //        return content.Body;
        //    }

        //    return null;
        //}

        [AbpAllowAnonymous]
        public async Task<string> GetAboutUs()
        {
            //var aboutUsId = Convert.ToInt32(_appConfiguration["LegalDocs:About_Us"]);

            var content = await _contentRepository.FirstOrDefaultAsync(e => e.Id == 0 /*aboutUsId*/ && e.Published == true);

            if (content != null)
            {
                return content.Body;
            }

            return null;
        }

        //[AbpAllowAnonymous]
        //public async Task<TaxRate> GetTaxRateByZipCode(string zipCode, long stateId)
        //{
        //    var taxRate = await _taxRateRepository.FirstOrDefaultAsync(e => e.StateId == stateId && e.ZipCode == zipCode);
        //    if (taxRate != null)
        //    {
        //        return taxRate;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //[AbpAllowAnonymous]
        //public async Task<List<PublicStoreListViewDto>> GetAllHubWiseStores(GetAllHubWiseStoreInput input)
        //{
        //    var storeIds = new List<long?>();
        //    if (input.DeliveryTypeId != null)
        //    {
        //        storeIds = _storeDeliveryTypeMapRepository.GetAll().Where(e => e.DeliveryTypeId == input.DeliveryTypeId).Select(e => e.StoreId).ToList();
        //    }
        //    var filteredStores = _hubsStoreRepository.GetAll()
        //                        .Include(e => e.StoreFk)
        //                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.StoreFk.Name.Contains(input.Filter))
        //                        .WhereIf(input.DeliveryTypeId != null, e => storeIds.Contains(e.StoreId))
        //                        .Where(e => e.HubId == input.HubId && e.Published == true);

        //    var pagedAndFilteredStores = filteredStores.OrderBy("displaySequence asc").Skip(input.SkipCount).Take(input.MaxResultCount);

        //    var stores = from o in pagedAndFilteredStores

        //                 select new PublicStoreListViewDto()
        //                 {
        //                     Id = o.StoreFk.Id,
        //                     Name = o == null || o.StoreFk.Name == null ? "" : o.StoreFk.Name.ToString(),
        //                     Url = o == null || o.StoreFk.StoreUrl == null ? "" : o.StoreFk.StoreUrl.ToString(),
        //                     StoreLogoLink = o.StoreFk.StoreLogoLink != null && o.StoreFk.StoreLogoLink != Guid.Empty ? o.StoreFk.StoreLogoLink : Guid.Empty,
        //                     DisplaySequence = o.DisplaySequence
        //                 };
        //    var result = await stores.ToListAsync();
        //    foreach (var store in result)
        //    {
        //        if (store.StoreLogoLink != null && store.StoreLogoLink != Guid.Empty)
        //        {
        //            store.Logo = await _binaryObjectManager.GetStorePictureUrlAsync((Guid)store.StoreLogoLink, ".png");
        //        }
        //        store.OpenHour = await GetStoreCurrentOpenStatus((long)store.Id);
        //        var tags = _storeTagRepository.GetAll().Include(e => e.MasterTagFk).Where(e => e.StoreId == store.Id).OrderBy(e => e.DisplaySequence).Take(3);
        //        foreach (var tag in tags)
        //        {
        //            if (tag.MasterTagFk?.Name != null)
        //            {
        //                store.StoreTags.Add(tag.MasterTagFk.Name.ToString());
        //            }
        //            else if (tag.CustomTag != null)
        //            {
        //                store.StoreTags.Add(tag.CustomTag.ToString());
        //            }
        //        }
        //    }

        //    return result;
        //}
        //[AbpAllowAnonymous]
        //public async Task<List<OrderDeliveryInfoDeliveryTypeLookupTableDto>> GetAllDeliveryTypeForTableDropdown(long? storeId)
        //{
        //    var deliveryTypeIds = new List<int?>();
        //    if (storeId != null)
        //    {
        //        deliveryTypeIds = _storeDeliveryTypeMapRepository.GetAll().Where(e => e.StoreId == storeId).Select(e => e.DeliveryTypeId).ToList();
        //    }
        //    var results = await _deliveryTypeRepository.GetAll().WhereIf(storeId != null, e => deliveryTypeIds.Contains(e.Id))
        //        .Select(deliveryType => new OrderDeliveryInfoDeliveryTypeLookupTableDto
        //        {
        //            Id = deliveryType.Id,
        //            DisplayName = deliveryType == null || deliveryType.Name == null ? "" : deliveryType.Name.ToString(),
        //            PictureId = deliveryType.PictureId
        //        }).ToListAsync();

        //    foreach (var item in results)
        //    {
        //        if (item.PictureId != null && item.PictureId != Guid.Empty)
        //        {
        //            item.Picture = await _binaryObjectManager.GetOthersPictureUrlAsync((Guid)item.PictureId, ".png");
        //        }
        //    }

        //    return results;
        //}

        //[AbpAuthorize]
        //public async Task<GetSquarePaymentCredentialsForViewDto> GetSquarePaymentCredentials()
        //{
        //    var output = new GetSquarePaymentCredentialsForViewDto();
        //    output.ApplicationId = _appConfiguration["Payment:Square:ApplicationId"];
        //    output.Environment = _appConfiguration["Payment:Square:Environment"];
        //    output.AccessToken = _appConfiguration["Payment:Square:AccessToken"];
        //    output.LocationId = _appConfiguration["Payment:Square:LocationId"];
        //    return output;
        //}

        //[AbpAuthorize]
        //public async Task<GetStripePaymentCredentialsForViewDto> GetStripePaymentCredentials()
        //{
        //    var output = new GetStripePaymentCredentialsForViewDto();
        //    output.BaseUrl = _appConfiguration["Payment:Stripe:BaseUrl"];
        //    output.SecretKey = _appConfiguration["Payment:Stripe:SecretKey"];
        //    output.PublishableKey = _appConfiguration["Payment:Stripe:PublishableKey"];
        //    output.WebhookSecret = _appConfiguration["Payment:Stripe:WebhookSecret"];
        //    output.PaymentMethodTypes = _appConfiguration.GetSection("Payment:Stripe:PaymentMethodTypes").Get<List<string>>();
        //    return output;
        //}

        //[AbpAuthorize]
        //public async Task CreatePayment(string nonceNumber, long totalAmount, long orderId)
        //{
        //    var checkOutDetails = await GetCheckOutDetails(orderId);
        //    string squareOrderId = await UpdateOrderToSquarePortal(checkOutDetails);

        //    if (squareOrderId != null)
        //    {
        //        SquareClient client;

        //        Square.Environment environment = _appConfiguration["Payment:Square:Environment"] == "sandbox" ? Square.Environment.Sandbox : Square.Environment.Production;

        //        client = new SquareClient.Builder()
        //            .Environment(environment)
        //            .AccessToken(_appConfiguration["Payment:Square:AccessToken"])
        //            .Build();

        //        string uuid = NewIdempotencyKey();



        //        var tipsAmount = checkOutDetails.GratuityAmount != null ? (Math.Round((decimal)checkOutDetails.GratuityAmount, 2)) * 100 : 0;

        //        totalAmount = (long)(checkOutDetails.TotalAmount * 100) - (long)tipsAmount;

        //        var amountMoney = new Money.Builder()
        //                                  .Amount(totalAmount)
        //                                  .Currency("USD")
        //                                  .Build();

        //        var tipMoney = new Money.Builder()
        //                              .Amount((long)tipsAmount)
        //                              .Currency("USD")
        //                              .Build();


        //        var body = new CreatePaymentRequest.Builder(
        //                    sourceId: nonceNumber,
        //                    idempotencyKey: uuid,
        //                    amountMoney: amountMoney)
        //                    .OrderId(squareOrderId)
        //                  .Note("Order Payment")
        //                  .TipMoney(tipMoney)
        //                  .Build();


        //        try
        //        {
        //            var result = await client.PaymentsApi.CreatePaymentAsync(body: body);
        //        }
        //        catch (ApiException e)
        //        {

        //        }
        //    }

        //}

        //private static string NewIdempotencyKey()
        //{
        //    return Guid.NewGuid().ToString();
        //}

        //private async Task<string> UpdateOrderToSquarePortal(GetOrderDetailsForPublicViewDto input)
        //{
        //    Square.Environment environment = _appConfiguration["Payment:Square:Environment"] == "sandbox" ? Square.Environment.Sandbox : Square.Environment.Production;

        //    var client = new SquareClient.Builder()
        //        .Environment(environment)
        //        .AccessToken(_appConfiguration["Payment:Square:AccessToken"])
        //        .Build();

        //    var lineItems = new List<OrderLineItem>();

        //    foreach (var item in input.Products)
        //    {
        //        var amount = (long)(item.UnitPrice * 100);
        //        var basePriceMoney = new Money.Builder()
        //                                  .Amount(amount)
        //                                  .Currency("USD")
        //                                  .Build();
        //        var orderLineItem = new OrderLineItem.Builder(quantity: item.Quantity.ToString())
        //                                                                          .Name(item.ProducName)
        //                                                                          .ItemType("ITEM")
        //                                                                          .BasePriceMoney(basePriceMoney)
        //                                                                          .Build();
        //        lineItems.Add(orderLineItem);
        //    }
        //    var taxes = new List<OrderLineItemTax>();
        //    var createdOrder = await _orderRepository.FirstOrDefaultAsync(e => e.Id == input.OrderId);
        //    if (createdOrder.TaxRateId != null)
        //    {
        //        var taxRate = await _taxRateRepository.FirstOrDefaultAsync(e => e.Id == createdOrder.TaxRateId);
        //        var taxPercentage = taxRate.CombinedRate * 100;

        //        var orderLineItemTax = new OrderLineItemTax.Builder()
        //                                              .Name(taxRate.Name)
        //                                              .Type("ADDITIVE")
        //                                              .Percentage(taxPercentage.ToString())
        //                                              .Scope("ORDER")
        //                                              .Build();


        //        taxes.Add(orderLineItemTax);
        //    }

        //    var serviceCharge = input.ServiceCharge != null ? (Math.Round((decimal)input.ServiceCharge, 2)) * 100 : 0;
        //    var serviceChargeAmountMoney = new Money.Builder()
        //                                              .Amount((long)serviceCharge)
        //                                              .Currency("USD")
        //                                              .Build();

        //    var orderServiceCharge = new OrderServiceCharge.Builder()
        //      .AmountMoney(serviceChargeAmountMoney)
        //      .Name("Service Charge")
        //      .CalculationPhase("TOTAL_PHASE")
        //      .Build();


        //    var deliveryCharge = input.DeliveryCharge != null ? (Math.Round((decimal)input.DeliveryCharge, 2)) * 100 : 0;
        //    var deliveryChargeAmountMoney = new Money.Builder()
        //                                              .Amount((long)deliveryCharge)
        //                                              .Currency("USD")
        //                                              .Build();

        //    var orderDeliveryCharge = new OrderServiceCharge.Builder()
        //      .AmountMoney(deliveryChargeAmountMoney)
        //      .Name("Delivery Charge")
        //      .CalculationPhase("TOTAL_PHASE")
        //      .Build();



        //    var serviceCharges = new List<OrderServiceCharge>();
        //    serviceCharges.Add(orderServiceCharge);
        //    serviceCharges.Add(orderDeliveryCharge);



        //    var recipient = new OrderFulfillmentRecipient.Builder()
        //      .DisplayName(input.Name)
        //      .EmailAddress(input.Email)
        //      .PhoneNumber(input.Phone)
        //      .Build();



        //    var shipmentName = input.IsDeliveryOrPickup ? "SHIPMENT" : "PICKUP";
        //    var orderState = "PROPOSED";
        //    var fulfillments = new List<OrderFulfillment>();
        //    if (input.IsDeliveryOrPickup)
        //    {
        //        var shipmentDetails = new OrderFulfillmentShipmentDetails.Builder()
        //                                                                 .Recipient(recipient)
        //                                                                 .Build();
        //        var orderFulfillment = new OrderFulfillment.Builder()
        //      .Type(shipmentName)
        //      .State(orderState)
        //      .ShipmentDetails(shipmentDetails)
        //      .Build();
        //        fulfillments.Add(orderFulfillment);
        //    }
        //    else
        //    {
        //        var pickupDetails = new OrderFulfillmentPickupDetails.Builder()
        //                                                              .Recipient(recipient)
        //                                                              .ScheduleType("ASAP")
        //                                                              .PickupAt(input.CreationTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo))
        //                                                              .Build();
        //        var orderFulfillment = new OrderFulfillment.Builder()
        //      .Type(shipmentName)
        //      .State(orderState)
        //      .PickupDetails(pickupDetails)
        //      .Build();
        //        fulfillments.Add(orderFulfillment);
        //    }


        //    var order = new Order.Builder(locationId: _appConfiguration["Payment:Square:LocationId"])
        //      .CustomerId(createdOrder.ContactId.ToString())
        //      .ReferenceId(createdOrder.Id.ToString())
        //      .LineItems(lineItems)
        //      .Fulfillments(fulfillments)
        //      .Taxes(taxes)
        //      .ServiceCharges(serviceCharges)
        //      .Build();

        //    var body = new CreateOrderRequest.Builder()
        //      .Order(order)
        //      .IdempotencyKey(NewIdempotencyKey())
        //      .Build();

        //    try
        //    {
        //        var result = await client.OrdersApi.CreateOrderAsync(body: body);
        //        return result.Order.Id;
        //    }
        //    catch (ApiException e)
        //    {
        //        //Console.WriteLine("Failed to make the request");
        //        //Console.WriteLine($"Response Code: {e.ResponseCode}");
        //        //Console.WriteLine($"Exception: {e.Message}");
        //        return null;
        //    }
        //}

        //[AbpAuthorize]
        //public async Task PayByWalletAmount(PayByWalletInputDto input)
        //{
        //    var customerWallet = await _customerWalletRepository.FirstOrDefaultAsync(e => e.ContactId == input.ContactId);
        //    var order = await _orderRepository.FirstOrDefaultAsync(e => e.Id == input.OrderId);
        //    var orderPayment = await _orderPaymentInfoRepository.FirstOrDefaultAsync(e => e.OrderId == input.OrderId);

        //    if (input.TotalAmount != null && Math.Round((decimal)input.TotalAmount, 2) < order.TotalAmount)
        //    {
        //        throw new UserFriendlyException("Paid amount doesn't match with order total amount");
        //    }

        //    orderPayment.PaidDate = input.PaidDate;
        //    orderPayment.PaidTime = input.PaidTime;
        //    orderPayment.PaymentTypeId = input.PaymentTypeId;
        //    await _orderPaymentInfoRepository.UpdateAsync(orderPayment);

        //    order.PaymentCompleted = true;
        //    await _orderRepository.UpdateAsync(order);

        //    if (customerWallet != null)
        //    {
        //        customerWallet.BalanceAmount = customerWallet.BalanceAmount - (double)input.TotalAmount;
        //        customerWallet.BalanceDate = DateTime.Now;
        //        await _customerWalletRepository.UpdateAsync(customerWallet);
        //        await UpdateWalletTransactionHistory(customerWallet.Id, input.TotalAmount, input.PaidTime, customerWallet.BalanceAmount, input.OrderId);
        //    }

        //}

        //private async Task UpdateWalletTransactionHistory(long customerWalletId, decimal? totalAmount, string paidTime, double? currentBalance, long orderId)
        //{
        //    var lastTransaction = _walletTransactionRepository.GetAll().Where(e => e.CustomerWalletId == customerWalletId).OrderByDescending(e => e.Id).FirstOrDefault();

        //    if (lastTransaction != null)
        //    {
        //        var item = new WalletTransaction();
        //        item.BalanceBeforeTransaction = lastTransaction.CurrentBalanceAfterTransaction;
        //        item.AddOrDeduct = false;
        //        item.Amount = (double)totalAmount;
        //        item.TransactionDate = DateTime.Now;
        //        item.TransactionTime = paidTime;
        //        item.CurrentBalanceAfterTransaction = currentBalance;
        //        item.CustomerWalletId = customerWalletId;
        //        item.OrderId = orderId;
        //        item.WalletTransactionTypeId = (long)WalletTransactionTypeEnum.FundsOut;
        //        await _walletTransactionRepository.InsertAsync(item);
        //    }
        //    else
        //    {
        //        var item = new WalletTransaction();
        //        item.BalanceBeforeTransaction = 0;
        //        item.AddOrDeduct = false;
        //        item.Amount = (double)totalAmount;
        //        item.TransactionDate = DateTime.Now;
        //        item.TransactionTime = paidTime;
        //        item.CurrentBalanceAfterTransaction = currentBalance;
        //        item.CustomerWalletId = customerWalletId;
        //        item.OrderId = orderId;
        //        item.WalletTransactionTypeId = (long)WalletTransactionTypeEnum.FundsOut;
        //        await _walletTransactionRepository.InsertAsync(item);
        //    }
        //}

        //public async Task<List<GetReservationTimeSlotForPublicViewDto>> GetStoreTimeSlotsByStore(long storeId,DateTime date)
        //{
        //    var output = new List<GetReservationTimeSlotForPublicViewDto>();
        //    var timeSlots = _reservationTimeSlotRepository.GetAll().Where(e => e.StoreId == storeId);

        //    foreach(var item in timeSlots)
        //    {
        //        var isAvailable = await _reservationRepository.FirstOrDefaultAsync(e => e.StoreId == storeId && e.Date == date && e.ReservationTimeSlotId == item.Id) != null ? false : true;

        //        output.Add(new GetReservationTimeSlotForPublicViewDto
        //        {
        //            TimeSlotName=item.Name,
        //            TimeSlotId=item.Id,
        //            IsAvailable=isAvailable
        //        });
        //    }
        //    return output;
        //}
    }
}