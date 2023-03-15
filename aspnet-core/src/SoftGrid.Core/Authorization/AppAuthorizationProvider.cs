using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace SoftGrid.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var stores = pages.CreateChildPermission(AppPermissions.Pages_Stores, L("Stores"));
            stores.CreateChildPermission(AppPermissions.Pages_Stores_Create, L("CreateNewStore"));
            stores.CreateChildPermission(AppPermissions.Pages_Stores_Edit, L("EditStore"));
            stores.CreateChildPermission(AppPermissions.Pages_Stores_Delete, L("DeleteStore"));

            var products = pages.CreateChildPermission(AppPermissions.Pages_Products, L("Products"));
            products.CreateChildPermission(AppPermissions.Pages_Products_Create, L("CreateNewProduct"));
            products.CreateChildPermission(AppPermissions.Pages_Products_Edit, L("EditProduct"));
            products.CreateChildPermission(AppPermissions.Pages_Products_Delete, L("DeleteProduct"));

            var productCategories = pages.CreateChildPermission(AppPermissions.Pages_ProductCategories, L("ProductCategories"));
            productCategories.CreateChildPermission(AppPermissions.Pages_ProductCategories_Create, L("CreateNewProductCategory"));
            productCategories.CreateChildPermission(AppPermissions.Pages_ProductCategories_Edit, L("EditProductCategory"));
            productCategories.CreateChildPermission(AppPermissions.Pages_ProductCategories_Delete, L("DeleteProductCategory"));

            var mediaLibraries = pages.CreateChildPermission(AppPermissions.Pages_MediaLibraries, L("MediaLibraries"));
            mediaLibraries.CreateChildPermission(AppPermissions.Pages_MediaLibraries_Create, L("CreateNewMediaLibrary"));
            mediaLibraries.CreateChildPermission(AppPermissions.Pages_MediaLibraries_Edit, L("EditMediaLibrary"));
            mediaLibraries.CreateChildPermission(AppPermissions.Pages_MediaLibraries_Delete, L("DeleteMediaLibrary"));

            var businesses = pages.CreateChildPermission(AppPermissions.Pages_Businesses, L("Businesses"));
            businesses.CreateChildPermission(AppPermissions.Pages_Businesses_Create, L("CreateNewBusiness"));
            businesses.CreateChildPermission(AppPermissions.Pages_Businesses_Edit, L("EditBusiness"));
            businesses.CreateChildPermission(AppPermissions.Pages_Businesses_Delete, L("DeleteBusiness"));

            var contacts = pages.CreateChildPermission(AppPermissions.Pages_Contacts, L("Contacts"));
            contacts.CreateChildPermission(AppPermissions.Pages_Contacts_Create, L("CreateNewContact"));
            contacts.CreateChildPermission(AppPermissions.Pages_Contacts_Edit, L("EditContact"));
            contacts.CreateChildPermission(AppPermissions.Pages_Contacts_Delete, L("DeleteContact"));

            var hubs = pages.CreateChildPermission(AppPermissions.Pages_Hubs, L("Hubs"));
            hubs.CreateChildPermission(AppPermissions.Pages_Hubs_Create, L("CreateNewHub"));
            hubs.CreateChildPermission(AppPermissions.Pages_Hubs_Edit, L("EditHub"));
            hubs.CreateChildPermission(AppPermissions.Pages_Hubs_Delete, L("DeleteHub"));

            var hubTypes = pages.CreateChildPermission(AppPermissions.Pages_HubTypes, L("HubTypes"));
            hubTypes.CreateChildPermission(AppPermissions.Pages_HubTypes_Create, L("CreateNewHubType"));
            hubTypes.CreateChildPermission(AppPermissions.Pages_HubTypes_Edit, L("EditHubType"));
            hubTypes.CreateChildPermission(AppPermissions.Pages_HubTypes_Delete, L("DeleteHubType"));

            var membershipTypes = pages.CreateChildPermission(AppPermissions.Pages_MembershipTypes, L("MembershipTypes"));
            membershipTypes.CreateChildPermission(AppPermissions.Pages_MembershipTypes_Create, L("CreateNewMembershipType"));
            membershipTypes.CreateChildPermission(AppPermissions.Pages_MembershipTypes_Edit, L("EditMembershipType"));
            membershipTypes.CreateChildPermission(AppPermissions.Pages_MembershipTypes_Delete, L("DeleteMembershipType"));

            var contractTypes = pages.CreateChildPermission(AppPermissions.Pages_ContractTypes, L("ContractTypes"));
            contractTypes.CreateChildPermission(AppPermissions.Pages_ContractTypes_Create, L("CreateNewContractType"));
            contractTypes.CreateChildPermission(AppPermissions.Pages_ContractTypes_Edit, L("EditContractType"));
            contractTypes.CreateChildPermission(AppPermissions.Pages_ContractTypes_Delete, L("DeleteContractType"));

            var documentTypes = pages.CreateChildPermission(AppPermissions.Pages_DocumentTypes, L("DocumentTypes"));
            documentTypes.CreateChildPermission(AppPermissions.Pages_DocumentTypes_Create, L("CreateNewDocumentType"));
            documentTypes.CreateChildPermission(AppPermissions.Pages_DocumentTypes_Edit, L("EditDocumentType"));
            documentTypes.CreateChildPermission(AppPermissions.Pages_DocumentTypes_Delete, L("DeleteDocumentType"));

            var smsTemplates = pages.CreateChildPermission(AppPermissions.Pages_SmsTemplates, L("SmsTemplates"));
            smsTemplates.CreateChildPermission(AppPermissions.Pages_SmsTemplates_Create, L("CreateNewSmsTemplate"));
            smsTemplates.CreateChildPermission(AppPermissions.Pages_SmsTemplates_Edit, L("EditSmsTemplate"));
            smsTemplates.CreateChildPermission(AppPermissions.Pages_SmsTemplates_Delete, L("DeleteSmsTemplate"));

            var emailTemplates = pages.CreateChildPermission(AppPermissions.Pages_EmailTemplates, L("EmailTemplates"));
            emailTemplates.CreateChildPermission(AppPermissions.Pages_EmailTemplates_Create, L("CreateNewEmailTemplate"));
            emailTemplates.CreateChildPermission(AppPermissions.Pages_EmailTemplates_Edit, L("EditEmailTemplate"));
            emailTemplates.CreateChildPermission(AppPermissions.Pages_EmailTemplates_Delete, L("DeleteEmailTemplate"));

            var connectChannels = pages.CreateChildPermission(AppPermissions.Pages_ConnectChannels, L("ConnectChannels"));
            connectChannels.CreateChildPermission(AppPermissions.Pages_ConnectChannels_Create, L("CreateNewConnectChannel"));
            connectChannels.CreateChildPermission(AppPermissions.Pages_ConnectChannels_Edit, L("EditConnectChannel"));
            connectChannels.CreateChildPermission(AppPermissions.Pages_ConnectChannels_Delete, L("DeleteConnectChannel"));

            var zipCodes = pages.CreateChildPermission(AppPermissions.Pages_ZipCodes, L("ZipCodes"));
            zipCodes.CreateChildPermission(AppPermissions.Pages_ZipCodes_Create, L("CreateNewZipCode"));
            zipCodes.CreateChildPermission(AppPermissions.Pages_ZipCodes_Edit, L("EditZipCode"));
            zipCodes.CreateChildPermission(AppPermissions.Pages_ZipCodes_Delete, L("DeleteZipCode"));

            var ratingLikes = pages.CreateChildPermission(AppPermissions.Pages_RatingLikes, L("RatingLikes"));
            ratingLikes.CreateChildPermission(AppPermissions.Pages_RatingLikes_Create, L("CreateNewRatingLike"));
            ratingLikes.CreateChildPermission(AppPermissions.Pages_RatingLikes_Edit, L("EditRatingLike"));
            ratingLikes.CreateChildPermission(AppPermissions.Pages_RatingLikes_Delete, L("DeleteRatingLike"));

            var measurementUnits = pages.CreateChildPermission(AppPermissions.Pages_MeasurementUnits, L("MeasurementUnits"));
            measurementUnits.CreateChildPermission(AppPermissions.Pages_MeasurementUnits_Create, L("CreateNewMeasurementUnit"));
            measurementUnits.CreateChildPermission(AppPermissions.Pages_MeasurementUnits_Edit, L("EditMeasurementUnit"));
            measurementUnits.CreateChildPermission(AppPermissions.Pages_MeasurementUnits_Delete, L("DeleteMeasurementUnit"));

            var masterTags = pages.CreateChildPermission(AppPermissions.Pages_MasterTags, L("MasterTags"));
            masterTags.CreateChildPermission(AppPermissions.Pages_MasterTags_Create, L("CreateNewMasterTag"));
            masterTags.CreateChildPermission(AppPermissions.Pages_MasterTags_Edit, L("EditMasterTag"));
            masterTags.CreateChildPermission(AppPermissions.Pages_MasterTags_Delete, L("DeleteMasterTag"));

            var masterTagCategories = pages.CreateChildPermission(AppPermissions.Pages_MasterTagCategories, L("MasterTagCategories"));
            masterTagCategories.CreateChildPermission(AppPermissions.Pages_MasterTagCategories_Create, L("CreateNewMasterTagCategory"));
            masterTagCategories.CreateChildPermission(AppPermissions.Pages_MasterTagCategories_Edit, L("EditMasterTagCategory"));
            masterTagCategories.CreateChildPermission(AppPermissions.Pages_MasterTagCategories_Delete, L("DeleteMasterTagCategory"));

            var cities = pages.CreateChildPermission(AppPermissions.Pages_Cities, L("Cities"));
            cities.CreateChildPermission(AppPermissions.Pages_Cities_Create, L("CreateNewCity"));
            cities.CreateChildPermission(AppPermissions.Pages_Cities_Edit, L("EditCity"));
            cities.CreateChildPermission(AppPermissions.Pages_Cities_Delete, L("DeleteCity"));

            var counties = pages.CreateChildPermission(AppPermissions.Pages_Counties, L("Counties"));
            counties.CreateChildPermission(AppPermissions.Pages_Counties_Create, L("CreateNewCounty"));
            counties.CreateChildPermission(AppPermissions.Pages_Counties_Edit, L("EditCounty"));
            counties.CreateChildPermission(AppPermissions.Pages_Counties_Delete, L("DeleteCounty"));

            var states = pages.CreateChildPermission(AppPermissions.Pages_States, L("States"));
            states.CreateChildPermission(AppPermissions.Pages_States_Create, L("CreateNewState"));
            states.CreateChildPermission(AppPermissions.Pages_States_Edit, L("EditState"));
            states.CreateChildPermission(AppPermissions.Pages_States_Delete, L("DeleteState"));

            var countries = pages.CreateChildPermission(AppPermissions.Pages_Countries, L("Countries"));
            countries.CreateChildPermission(AppPermissions.Pages_Countries_Create, L("CreateNewCountry"));
            countries.CreateChildPermission(AppPermissions.Pages_Countries_Edit, L("EditCountry"));
            countries.CreateChildPermission(AppPermissions.Pages_Countries_Delete, L("DeleteCountry"));

            var currencies = pages.CreateChildPermission(AppPermissions.Pages_Currencies, L("Currencies"));
            currencies.CreateChildPermission(AppPermissions.Pages_Currencies_Create, L("CreateNewCurrency"));
            currencies.CreateChildPermission(AppPermissions.Pages_Currencies_Edit, L("EditCurrency"));
            currencies.CreateChildPermission(AppPermissions.Pages_Currencies_Delete, L("DeleteCurrency"));

            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangeProfilePicture, L("UpdateUsersProfilePicture"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeDefaultLanguage, L("ChangeDefaultLanguage"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicProperties = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties, L("DynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Create, L("CreatingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Edit, L("EditingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Delete, L("DeletingDynamicProperties"));

            var dynamicPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue, L("DynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Create, L("CreatingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Edit, L("EditingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Delete, L("DeletingDynamicPropertyValue"));

            var dynamicEntityProperties = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties, L("DynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Create, L("CreatingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Edit, L("EditingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Delete, L("DeletingDynamicEntityProperties"));

            var dynamicEntityPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue, L("EntityDynamicPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Create, L("CreatingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Edit, L("EditingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Delete, L("DeletingDynamicEntityPropertyValue"));

            var massNotification = administration.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification, L("MassNotifications"));
            massNotification.CreateChildPermission(AppPermissions.Pages_Administration_MassNotification_Create, L("MassNotificationCreate"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);

            var maintenance = administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            maintenance.CreateChildPermission(AppPermissions.Pages_Administration_NewVersion_Create, L("SendNewVersionNotification"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SoftGridConsts.LocalizationSourceName);
        }
    }
}