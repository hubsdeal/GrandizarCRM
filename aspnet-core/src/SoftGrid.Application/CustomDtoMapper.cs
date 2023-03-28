﻿using SoftGrid.SalesLeadManagement.Dtos;
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