using SoftGrid.CMS.Dtos;
using SoftGrid.CMS;
using SoftGrid.DiscountManagement.Dtos;
using SoftGrid.DiscountManagement;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.OrderManagement;
using SoftGrid.SalesLeadManagement.Dtos;
using SoftGrid.SalesLeadManagement;
using SoftGrid.TaskManagement.Dtos;
using SoftGrid.TaskManagement;
using SoftGrid.JobManagement.Dtos;
using SoftGrid.JobManagement;
using SoftGrid.Shop.Dtos;
using SoftGrid.Shop;
using SoftGrid.CRM.Dtos;
using SoftGrid.CRM;
using SoftGrid.Territory.Dtos;
using SoftGrid.Territory;
using SoftGrid.LookupData.Dtos;
using SoftGrid.LookupData;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityProperties;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using IdentityServer4.Extensions;
using SoftGrid.Auditing.Dto;
using SoftGrid.Authorization.Accounts.Dto;
using SoftGrid.Authorization.Delegation;
using SoftGrid.Authorization.Permissions.Dto;
using SoftGrid.Authorization.Roles;
using SoftGrid.Authorization.Roles.Dto;
using SoftGrid.Authorization.Users;
using SoftGrid.Authorization.Users.Delegation.Dto;
using SoftGrid.Authorization.Users.Dto;
using SoftGrid.Authorization.Users.Importing.Dto;
using SoftGrid.Authorization.Users.Profile.Dto;
using SoftGrid.Chat;
using SoftGrid.Chat.Dto;
using SoftGrid.DynamicEntityProperties.Dto;
using SoftGrid.Editions;
using SoftGrid.Editions.Dto;
using SoftGrid.Friendships;
using SoftGrid.Friendships.Cache;
using SoftGrid.Friendships.Dto;
using SoftGrid.Localization.Dto;
using SoftGrid.MultiTenancy;
using SoftGrid.MultiTenancy.Dto;
using SoftGrid.MultiTenancy.HostDashboard.Dto;
using SoftGrid.MultiTenancy.Payments;
using SoftGrid.MultiTenancy.Payments.Dto;
using SoftGrid.Notifications.Dto;
using SoftGrid.Organizations.Dto;
using SoftGrid.Sessions.Dto;
using SoftGrid.WebHooks.Dto;

namespace SoftGrid
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditJobTaskMapDto, JobTaskMap>().ReverseMap();
            configuration.CreateMap<JobTaskMapDto, JobTaskMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductTaskMapDto, ProductTaskMap>().ReverseMap();
            configuration.CreateMap<ProductTaskMapDto, ProductTaskMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditContactTaskMapDto, ContactTaskMap>().ReverseMap();
            configuration.CreateMap<ContactTaskMapDto, ContactTaskMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditJobMasterTagSettingDto, JobMasterTagSetting>().ReverseMap();
            configuration.CreateMap<JobMasterTagSettingDto, JobMasterTagSetting>().ReverseMap();
            configuration.CreateMap<CreateOrEditBusinessMasterTagSettingDto, BusinessMasterTagSetting>().ReverseMap();
            configuration.CreateMap<BusinessMasterTagSettingDto, BusinessMasterTagSetting>().ReverseMap();
            configuration.CreateMap<CreateOrEditContactMasterTagSettingDto, ContactMasterTagSetting>().ReverseMap();
            configuration.CreateMap<ContactMasterTagSettingDto, ContactMasterTagSetting>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductMasterTagSettingDto, ProductMasterTagSetting>().ReverseMap();
            configuration.CreateMap<ProductMasterTagSettingDto, ProductMasterTagSetting>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreMasterTagSettingDto, StoreMasterTagSetting>().ReverseMap();
            configuration.CreateMap<StoreMasterTagSettingDto, StoreMasterTagSetting>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreTagSettingCategoryDto, StoreTagSettingCategory>().ReverseMap();
            configuration.CreateMap<StoreTagSettingCategoryDto, StoreTagSettingCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditTaskDocumentDto, TaskDocument>().ReverseMap();
            configuration.CreateMap<TaskDocumentDto, TaskDocument>().ReverseMap();
            configuration.CreateMap<CreateOrEditJobDocumentDto, JobDocument>().ReverseMap();
            configuration.CreateMap<JobDocumentDto, JobDocument>().ReverseMap();
            configuration.CreateMap<CreateOrEditHubDocumentDto, HubDocument>().ReverseMap();
            configuration.CreateMap<HubDocumentDto, HubDocument>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductDocumentDto, ProductDocument>().ReverseMap();
            configuration.CreateMap<ProductDocumentDto, ProductDocument>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreDocumentDto, StoreDocument>().ReverseMap();
            configuration.CreateMap<StoreDocumentDto, StoreDocument>().ReverseMap();
            configuration.CreateMap<CreateOrEditEmployeeDocumentDto, EmployeeDocument>().ReverseMap();
            configuration.CreateMap<EmployeeDocumentDto, EmployeeDocument>().ReverseMap();
            configuration.CreateMap<CreateOrEditBusinessDocumentDto, BusinessDocument>().ReverseMap();
            configuration.CreateMap<BusinessDocumentDto, BusinessDocument>().ReverseMap();
            configuration.CreateMap<CreateOrEditContactDocumentDto, ContactDocument>().ReverseMap();
            configuration.CreateMap<ContactDocumentDto, ContactDocument>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreTaskMapDto, StoreTaskMap>().ReverseMap();
            configuration.CreateMap<StoreTaskMapDto, StoreTaskMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditContentDto, Content>().ReverseMap();
            configuration.CreateMap<ContentDto, Content>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductTeamDto, ProductTeam>().ReverseMap();
            configuration.CreateMap<ProductTeamDto, ProductTeam>().ReverseMap();
            configuration.CreateMap<CreateOrEditShoppingCartDto, ShoppingCart>().ReverseMap();
            configuration.CreateMap<ShoppingCartDto, ShoppingCart>().ReverseMap();
            configuration.CreateMap<CreateOrEditWishListDto, WishList>().ReverseMap();
            configuration.CreateMap<WishListDto, WishList>().ReverseMap();
            configuration.CreateMap<CreateOrEditDiscountCodeUserHistoryDto, DiscountCodeUserHistory>().ReverseMap();
            configuration.CreateMap<DiscountCodeUserHistoryDto, DiscountCodeUserHistory>().ReverseMap();
            configuration.CreateMap<CreateOrEditDiscountCodeMapDto, DiscountCodeMap>().ReverseMap();
            configuration.CreateMap<DiscountCodeMapDto, DiscountCodeMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditDiscountCodeByCustomerDto, DiscountCodeByCustomer>().ReverseMap();
            configuration.CreateMap<DiscountCodeByCustomerDto, DiscountCodeByCustomer>().ReverseMap();
            configuration.CreateMap<CreateOrEditDiscountCodeGeneratorDto, DiscountCodeGenerator>().ReverseMap();
            configuration.CreateMap<DiscountCodeGeneratorDto, DiscountCodeGenerator>().ReverseMap();
            configuration.CreateMap<CreateOrEditCustomerWalletDto, CustomerWallet>().ReverseMap();
            configuration.CreateMap<CustomerWalletDto, CustomerWallet>().ReverseMap();
            configuration.CreateMap<CreateOrEditOrderTeamDto, OrderTeam>().ReverseMap();
            configuration.CreateMap<OrderTeamDto, OrderTeam>().ReverseMap();
            configuration.CreateMap<CreateOrEditOrderProductVariantDto, OrderProductVariant>().ReverseMap();
            configuration.CreateMap<OrderProductVariantDto, OrderProductVariant>().ReverseMap();
            configuration.CreateMap<CreateOrEditOrderProductInfoDto, OrderProductInfo>().ReverseMap();
            configuration.CreateMap<OrderProductInfoDto, OrderProductInfo>().ReverseMap();
            configuration.CreateMap<CreateOrEditPaymentTypeDto, PaymentType>().ReverseMap();
            configuration.CreateMap<PaymentTypeDto, PaymentType>().ReverseMap();
            configuration.CreateMap<CreateOrEditOrderPaymentInfoDto, OrderPaymentInfo>().ReverseMap();
            configuration.CreateMap<OrderPaymentInfoDto, OrderPaymentInfo>().ReverseMap();
            configuration.CreateMap<CreateOrEditOrderfulfillmentTeamDto, OrderfulfillmentTeam>().ReverseMap();
            configuration.CreateMap<OrderfulfillmentTeamDto, OrderfulfillmentTeam>().ReverseMap();
            configuration.CreateMap<CreateOrEditOrderFulfillmentStatusDto, OrderFulfillmentStatus>().ReverseMap();
            configuration.CreateMap<OrderFulfillmentStatusDto, OrderFulfillmentStatus>().ReverseMap();
            configuration.CreateMap<CreateOrEditOrderDeliveryInfoDto, OrderDeliveryInfo>().ReverseMap();
            configuration.CreateMap<OrderDeliveryInfoDto, OrderDeliveryInfo>().ReverseMap();
            configuration.CreateMap<CreateOrEditOrderDto, Order>().ReverseMap();
            configuration.CreateMap<OrderDto, Order>().ReverseMap();
            configuration.CreateMap<CreateOrEditOrderStatusDto, OrderStatus>().ReverseMap();
            configuration.CreateMap<OrderStatusDto, OrderStatus>().ReverseMap();
            configuration.CreateMap<CreateOrEditOrderSalesChannelDto, OrderSalesChannel>().ReverseMap();
            configuration.CreateMap<OrderSalesChannelDto, OrderSalesChannel>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductWholeSalePriceDto, ProductWholeSalePrice>().ReverseMap();
            configuration.CreateMap<ProductWholeSalePriceDto, ProductWholeSalePrice>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductWholeSaleQuantityTypeDto, ProductWholeSaleQuantityType>().ReverseMap();
            configuration.CreateMap<ProductWholeSaleQuantityTypeDto, ProductWholeSaleQuantityType>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductFlashSaleProductMapDto, ProductFlashSaleProductMap>().ReverseMap();
            configuration.CreateMap<ProductFlashSaleProductMapDto, ProductFlashSaleProductMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductAndGiftCardMapDto, ProductAndGiftCardMap>().ReverseMap();
            configuration.CreateMap<ProductAndGiftCardMapDto, ProductAndGiftCardMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductCategoryVariantMapDto, ProductCategoryVariantMap>().ReverseMap();
            configuration.CreateMap<ProductCategoryVariantMapDto, ProductCategoryVariantMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductByVariantDto, ProductByVariant>().ReverseMap();
            configuration.CreateMap<ProductByVariantDto, ProductByVariant>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductOwnerPublicContactInfoDto, ProductOwnerPublicContactInfo>().ReverseMap();
            configuration.CreateMap<ProductOwnerPublicContactInfoDto, ProductOwnerPublicContactInfo>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductReviewFeedbackDto, ProductReviewFeedback>().ReverseMap();
            configuration.CreateMap<ProductReviewFeedbackDto, ProductReviewFeedback>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductReviewDto, ProductReview>().ReverseMap();
            configuration.CreateMap<ProductReviewDto, ProductReview>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductReturnInfoDto, ProductReturnInfo>().ReverseMap();
            configuration.CreateMap<ProductReturnInfoDto, ProductReturnInfo>().ReverseMap();
            configuration.CreateMap<CreateOrEditReturnStatusDto, ReturnStatus>().ReverseMap();
            configuration.CreateMap<ReturnStatusDto, ReturnStatus>().ReverseMap();
            configuration.CreateMap<CreateOrEditReturnTypeDto, ReturnType>().ReverseMap();
            configuration.CreateMap<ReturnTypeDto, ReturnType>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductUpsellRelatedProductDto, ProductUpsellRelatedProduct>().ReverseMap();
            configuration.CreateMap<ProductUpsellRelatedProductDto, ProductUpsellRelatedProduct>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductCrossSellProductDto, ProductCrossSellProduct>().ReverseMap();
            configuration.CreateMap<ProductCrossSellProductDto, ProductCrossSellProduct>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductAccountTeamDto, ProductAccountTeam>().ReverseMap();
            configuration.CreateMap<ProductAccountTeamDto, ProductAccountTeam>().ReverseMap();
            configuration.CreateMap<CreateOrEditSubscriptionTypeDto, SubscriptionType>().ReverseMap();
            configuration.CreateMap<SubscriptionTypeDto, SubscriptionType>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductSubscriptionMapDto, ProductSubscriptionMap>().ReverseMap();
            configuration.CreateMap<ProductSubscriptionMapDto, ProductSubscriptionMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditSocialMediaDto, SocialMedia>().ReverseMap();
            configuration.CreateMap<SocialMediaDto, SocialMedia>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductCustomerStatDto, ProductCustomerStat>().ReverseMap();
            configuration.CreateMap<ProductCustomerStatDto, ProductCustomerStat>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductFaqDto, ProductFaq>().ReverseMap();
            configuration.CreateMap<ProductFaqDto, ProductFaq>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductCustomerQueryDto, ProductCustomerQuery>().ReverseMap();
            configuration.CreateMap<ProductCustomerQueryDto, ProductCustomerQuery>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductPackageDto, ProductPackage>().ReverseMap();
            configuration.CreateMap<ProductPackageDto, ProductPackage>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductNoteDto, ProductNote>().ReverseMap();
            configuration.CreateMap<ProductNoteDto, ProductNote>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductMediaDto, ProductMedia>().ReverseMap();
            configuration.CreateMap<ProductMediaDto, ProductMedia>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductCategoryTeamDto, ProductCategoryTeam>().ReverseMap();
            configuration.CreateMap<ProductCategoryTeamDto, ProductCategoryTeam>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductCategoryMapDto, ProductCategoryMap>().ReverseMap();
            configuration.CreateMap<ProductCategoryMapDto, ProductCategoryMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductVariantDto, ProductVariant>().ReverseMap();
            configuration.CreateMap<ProductVariantDto, ProductVariant>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductVariantCategoryDto, ProductVariantCategory>().ReverseMap();
            configuration.CreateMap<ProductVariantCategoryDto, ProductVariantCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditHubNavigationMenuDto, HubNavigationMenu>().ReverseMap();
            configuration.CreateMap<HubNavigationMenuDto, HubNavigationMenu>().ReverseMap();
            configuration.CreateMap<CreateOrEditMasterNavigationMenuDto, MasterNavigationMenu>().ReverseMap();
            configuration.CreateMap<MasterNavigationMenuDto, MasterNavigationMenu>().ReverseMap();
            configuration.CreateMap<CreateOrEditHubAccountTeamDto, HubAccountTeam>().ReverseMap();
            configuration.CreateMap<HubAccountTeamDto, HubAccountTeam>().ReverseMap();
            configuration.CreateMap<CreateOrEditHubSalesProjectionDto, HubSalesProjection>().ReverseMap();
            configuration.CreateMap<HubSalesProjectionDto, HubSalesProjection>().ReverseMap();
            configuration.CreateMap<CreateOrEditHubZipCodeMapDto, HubZipCodeMap>().ReverseMap();
            configuration.CreateMap<HubZipCodeMapDto, HubZipCodeMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditHubContactDto, HubContact>().ReverseMap();
            configuration.CreateMap<HubContactDto, HubContact>().ReverseMap();
            configuration.CreateMap<CreateOrEditHubBusinessDto, HubBusiness>().ReverseMap();
            configuration.CreateMap<HubBusinessDto, HubBusiness>().ReverseMap();
            configuration.CreateMap<CreateOrEditHubStoreDto, HubStore>().ReverseMap();
            configuration.CreateMap<HubStoreDto, HubStore>().ReverseMap();
            configuration.CreateMap<CreateOrEditHubProductDto, HubProduct>().ReverseMap();
            configuration.CreateMap<HubProductDto, HubProduct>().ReverseMap();
            configuration.CreateMap<CreateOrEditHubProductCategoryDto, HubProductCategory>().ReverseMap();
            configuration.CreateMap<HubProductCategoryDto, HubProductCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditLeadReferralRewardDto, LeadReferralReward>().ReverseMap();
            configuration.CreateMap<LeadReferralRewardDto, LeadReferralReward>().ReverseMap();
            configuration.CreateMap<CreateOrEditLeadContactDto, LeadContact>().ReverseMap();
            configuration.CreateMap<LeadContactDto, LeadContact>().ReverseMap();
            configuration.CreateMap<CreateOrEditLeadNoteDto, LeadNote>().ReverseMap();
            configuration.CreateMap<LeadNoteDto, LeadNote>().ReverseMap();
            configuration.CreateMap<CreateOrEditLeadTaskDto, LeadTask>().ReverseMap();
            configuration.CreateMap<LeadTaskDto, LeadTask>().ReverseMap();
            configuration.CreateMap<CreateOrEditLeadTagDto, LeadTag>().ReverseMap();
            configuration.CreateMap<LeadTagDto, LeadTag>().ReverseMap();
            configuration.CreateMap<CreateOrEditLeadSalesTeamDto, LeadSalesTeam>().ReverseMap();
            configuration.CreateMap<LeadSalesTeamDto, LeadSalesTeam>().ReverseMap();
            configuration.CreateMap<CreateOrEditLeadPipelineStatusDto, LeadPipelineStatus>().ReverseMap();
            configuration.CreateMap<LeadPipelineStatusDto, LeadPipelineStatus>().ReverseMap();
            configuration.CreateMap<CreateOrEditLeadDto, Lead>().ReverseMap();
            configuration.CreateMap<LeadDto, Lead>().ReverseMap();
            configuration.CreateMap<CreateOrEditLeadPipelineStageDto, LeadPipelineStage>().ReverseMap();
            configuration.CreateMap<LeadPipelineStageDto, LeadPipelineStage>().ReverseMap();
            configuration.CreateMap<CreateOrEditLeadSourceDto, LeadSource>().ReverseMap();
            configuration.CreateMap<LeadSourceDto, LeadSource>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreZipCodeMapDto, StoreZipCodeMap>().ReverseMap();
            configuration.CreateMap<StoreZipCodeMapDto, StoreZipCodeMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreReviewFeedbackDto, StoreReviewFeedback>().ReverseMap();
            configuration.CreateMap<StoreReviewFeedbackDto, StoreReviewFeedback>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreReviewDto, StoreReview>().ReverseMap();
            configuration.CreateMap<StoreReviewDto, StoreReview>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreRelevantStoreDto, StoreRelevantStore>().ReverseMap();
            configuration.CreateMap<StoreRelevantStoreDto, StoreRelevantStore>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreMarketplaceCommissionSettingDto, StoreMarketplaceCommissionSetting>().ReverseMap();
            configuration.CreateMap<StoreMarketplaceCommissionSettingDto, StoreMarketplaceCommissionSetting>().ReverseMap();
            configuration.CreateMap<CreateOrEditMarketplaceCommissionTypeDto, MarketplaceCommissionType>().ReverseMap();
            configuration.CreateMap<MarketplaceCommissionTypeDto, MarketplaceCommissionType>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreSalesAlertDto, StoreSalesAlert>().ReverseMap();
            configuration.CreateMap<StoreSalesAlertDto, StoreSalesAlert>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreTaxDto, StoreTax>().ReverseMap();
            configuration.CreateMap<StoreTaxDto, StoreTax>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreBankAccountDto, StoreBankAccount>().ReverseMap();
            configuration.CreateMap<StoreBankAccountDto, StoreBankAccount>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreNoteDto, StoreNote>().ReverseMap();
            configuration.CreateMap<StoreNoteDto, StoreNote>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreMediaDto, StoreMedia>().ReverseMap();
            configuration.CreateMap<StoreMediaDto, StoreMedia>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreLocationDto, StoreLocation>().ReverseMap();
            configuration.CreateMap<StoreLocationDto, StoreLocation>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreBusinessHourDto, StoreBusinessHour>().ReverseMap();
            configuration.CreateMap<StoreBusinessHourDto, StoreBusinessHour>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreContactMapDto, StoreContactMap>().ReverseMap();
            configuration.CreateMap<StoreContactMapDto, StoreContactMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreBusinessCustomerMapDto, StoreBusinessCustomerMap>().ReverseMap();
            configuration.CreateMap<StoreBusinessCustomerMapDto, StoreBusinessCustomerMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreOwnerTeamDto, StoreOwnerTeam>().ReverseMap();
            configuration.CreateMap<StoreOwnerTeamDto, StoreOwnerTeam>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreAccountTeamDto, StoreAccountTeam>().ReverseMap();
            configuration.CreateMap<StoreAccountTeamDto, StoreAccountTeam>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreProductMapDto, StoreProductMap>().ReverseMap();
            configuration.CreateMap<StoreProductMapDto, StoreProductMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreProductCategoryMapDto, StoreProductCategoryMap>().ReverseMap();
            configuration.CreateMap<StoreProductCategoryMapDto, StoreProductCategoryMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditEmployeeTagDto, EmployeeTag>().ReverseMap();
            configuration.CreateMap<EmployeeTagDto, EmployeeTag>().ReverseMap();
            configuration.CreateMap<CreateOrEditBusinessTaskMapDto, BusinessTaskMap>().ReverseMap();
            configuration.CreateMap<BusinessTaskMapDto, BusinessTaskMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditBusinessProductMapDto, BusinessProductMap>().ReverseMap();
            configuration.CreateMap<BusinessProductMapDto, BusinessProductMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditBusinessStoreMapDto, BusinessStoreMap>().ReverseMap();
            configuration.CreateMap<BusinessStoreMapDto, BusinessStoreMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditBusinessJobMapDto, BusinessJobMap>().ReverseMap();
            configuration.CreateMap<BusinessJobMapDto, BusinessJobMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditBusinessNoteDto, BusinessNote>().ReverseMap();
            configuration.CreateMap<BusinessNoteDto, BusinessNote>().ReverseMap();
            configuration.CreateMap<CreateOrEditBusinessUserDto, BusinessUser>().ReverseMap();
            configuration.CreateMap<BusinessUserDto, BusinessUser>().ReverseMap();
            configuration.CreateMap<CreateOrEditBusinessAccountTeamDto, BusinessAccountTeam>().ReverseMap();
            configuration.CreateMap<BusinessAccountTeamDto, BusinessAccountTeam>().ReverseMap();
            configuration.CreateMap<CreateOrEditBusinessContactMapDto, BusinessContactMap>().ReverseMap();
            configuration.CreateMap<BusinessContactMapDto, BusinessContactMap>().ReverseMap();
            configuration.CreateMap<CreateOrEditTaskTagDto, TaskTag>().ReverseMap();
            configuration.CreateMap<TaskTagDto, TaskTag>().ReverseMap();
            configuration.CreateMap<CreateOrEditTaskEventDto, TaskEvent>().ReverseMap();
            configuration.CreateMap<TaskEventDto, TaskEvent>().ReverseMap();
            configuration.CreateMap<CreateOrEditTaskStatusDto, TaskStatus>().ReverseMap();
            configuration.CreateMap<TaskStatusDto, TaskStatus>().ReverseMap();
            configuration.CreateMap<CreateOrEditEmployeeDto, Employee>().ReverseMap();
            configuration.CreateMap<EmployeeDto, Employee>().ReverseMap();
            configuration.CreateMap<CreateOrEditJobTagDto, JobTag>().ReverseMap();
            configuration.CreateMap<JobTagDto, JobTag>().ReverseMap();
            configuration.CreateMap<CreateOrEditJobDto, Job>().ReverseMap();
            configuration.CreateMap<JobDto, Job>().ReverseMap();
            configuration.CreateMap<CreateOrEditJobStatusTypeDto, JobStatusType>().ReverseMap();
            configuration.CreateMap<JobStatusTypeDto, JobStatusType>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreTagDto, StoreTag>().ReverseMap();
            configuration.CreateMap<StoreTagDto, StoreTag>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductTagDto, ProductTag>().ReverseMap();
            configuration.CreateMap<ProductTagDto, ProductTag>().ReverseMap();
            configuration.CreateMap<CreateOrEditBusinessTagDto, BusinessTag>().ReverseMap();
            configuration.CreateMap<BusinessTagDto, BusinessTag>().ReverseMap();
            configuration.CreateMap<CreateOrEditContactTagDto, ContactTag>().ReverseMap();
            configuration.CreateMap<ContactTagDto, ContactTag>().ReverseMap();
            configuration.CreateMap<CreateOrEditStoreDto, Store>().ReverseMap();
            configuration.CreateMap<StoreDto, Store>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductDto, Product>().ReverseMap();
            configuration.CreateMap<ProductDto, Product>().ReverseMap();
            configuration.CreateMap<CreateOrEditProductCategoryDto, ProductCategory>().ReverseMap();
            configuration.CreateMap<ProductCategoryDto, ProductCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditMediaLibraryDto, MediaLibrary>().ReverseMap();
            configuration.CreateMap<MediaLibraryDto, MediaLibrary>().ReverseMap();
            configuration.CreateMap<CreateOrEditBusinessDto, Business>().ReverseMap();
            configuration.CreateMap<BusinessDto, Business>().ReverseMap();
            configuration.CreateMap<CreateOrEditContactDto, Contact>().ReverseMap();
            configuration.CreateMap<ContactDto, Contact>().ReverseMap();
            configuration.CreateMap<CreateOrEditHubDto, Hub>().ReverseMap();
            configuration.CreateMap<HubDto, Hub>().ReverseMap();
            configuration.CreateMap<CreateOrEditHubTypeDto, HubType>().ReverseMap();
            configuration.CreateMap<HubTypeDto, HubType>().ReverseMap();
            configuration.CreateMap<CreateOrEditMembershipTypeDto, MembershipType>().ReverseMap();
            configuration.CreateMap<MembershipTypeDto, MembershipType>().ReverseMap();
            configuration.CreateMap<CreateOrEditContractTypeDto, ContractType>().ReverseMap();
            configuration.CreateMap<ContractTypeDto, ContractType>().ReverseMap();
            configuration.CreateMap<CreateOrEditDocumentTypeDto, DocumentType>().ReverseMap();
            configuration.CreateMap<DocumentTypeDto, DocumentType>().ReverseMap();
            configuration.CreateMap<CreateOrEditSmsTemplateDto, SmsTemplate>().ReverseMap();
            configuration.CreateMap<SmsTemplateDto, SmsTemplate>().ReverseMap();
            configuration.CreateMap<CreateOrEditEmailTemplateDto, EmailTemplate>().ReverseMap();
            configuration.CreateMap<EmailTemplateDto, EmailTemplate>().ReverseMap();
            configuration.CreateMap<CreateOrEditConnectChannelDto, ConnectChannel>().ReverseMap();
            configuration.CreateMap<ConnectChannelDto, ConnectChannel>().ReverseMap();
            configuration.CreateMap<CreateOrEditZipCodeDto, ZipCode>().ReverseMap();
            configuration.CreateMap<ZipCodeDto, ZipCode>().ReverseMap();
            configuration.CreateMap<CreateOrEditRatingLikeDto, RatingLike>().ReverseMap();
            configuration.CreateMap<RatingLikeDto, RatingLike>().ReverseMap();
            configuration.CreateMap<CreateOrEditMeasurementUnitDto, MeasurementUnit>().ReverseMap();
            configuration.CreateMap<MeasurementUnitDto, MeasurementUnit>().ReverseMap();
            configuration.CreateMap<CreateOrEditMasterTagDto, MasterTag>().ReverseMap();
            configuration.CreateMap<MasterTagDto, MasterTag>().ReverseMap();
            configuration.CreateMap<CreateOrEditMasterTagCategoryDto, MasterTagCategory>().ReverseMap();
            configuration.CreateMap<MasterTagCategoryDto, MasterTagCategory>().ReverseMap();
            configuration.CreateMap<CreateOrEditCityDto, City>().ReverseMap();
            configuration.CreateMap<CityDto, City>().ReverseMap();
            configuration.CreateMap<CreateOrEditCountyDto, County>().ReverseMap();
            configuration.CreateMap<CountyDto, County>().ReverseMap();
            configuration.CreateMap<CreateOrEditStateDto, State>().ReverseMap();
            configuration.CreateMap<StateDto, State>().ReverseMap();
            configuration.CreateMap<CreateOrEditCountryDto, Country>().ReverseMap();
            configuration.CreateMap<CountryDto, Country>().ReverseMap();
            configuration.CreateMap<CreateOrEditCurrencyDto, Currency>().ReverseMap();
            configuration.CreateMap<CurrencyDto, Currency>().ReverseMap();
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();

            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicProperty, DynamicPropertyDto>().ReverseMap();
            configuration.CreateMap<DynamicPropertyValue, DynamicPropertyValueDto>().ReverseMap();
            configuration.CreateMap<DynamicEntityProperty, DynamicEntityPropertyDto>()
                .ForMember(dto => dto.DynamicPropertyName,
                    options => options.MapFrom(entity => entity.DynamicProperty.DisplayName.IsNullOrEmpty() ? entity.DynamicProperty.PropertyName : entity.DynamicProperty.DisplayName));
            configuration.CreateMap<DynamicEntityPropertyDto, DynamicEntityProperty>();

            configuration.CreateMap<DynamicEntityPropertyValue, DynamicEntityPropertyValueDto>().ReverseMap();

            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}