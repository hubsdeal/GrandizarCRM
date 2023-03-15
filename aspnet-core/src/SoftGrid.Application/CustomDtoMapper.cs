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