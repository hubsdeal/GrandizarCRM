﻿import { AbpHttpInterceptor, RefreshTokenService, AbpHttpConfigurationService } from 'abp-ng2-module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import * as ApiServiceProxies from './service-proxies';
import { ZeroRefreshTokenService } from '@account/auth/zero-refresh-token.service';
import { ZeroTemplateHttpConfigurationService } from './zero-template-http-configuration.service';

@NgModule({
    providers: [
        ApiServiceProxies.ShoppingCartsServiceProxy,        
        ApiServiceProxies.WishListsServiceProxy,        
        ApiServiceProxies.DiscountCodeUserHistoriesServiceProxy,        
        ApiServiceProxies.DiscountCodeMapsServiceProxy,        
        ApiServiceProxies.DiscountCodeByCustomersServiceProxy,        
        ApiServiceProxies.DiscountCodeGeneratorsServiceProxy,        
        ApiServiceProxies.CustomerWalletsServiceProxy,        
        ApiServiceProxies.OrderTeamsServiceProxy,        
        ApiServiceProxies.OrderProductVariantsServiceProxy,        
        ApiServiceProxies.OrderProductInfosServiceProxy,        
        ApiServiceProxies.PaymentTypesServiceProxy,        
        ApiServiceProxies.OrderPaymentInfosServiceProxy,        
        ApiServiceProxies.OrderfulfillmentTeamsServiceProxy,        
        ApiServiceProxies.OrderFulfillmentStatusesServiceProxy,        
        ApiServiceProxies.OrderDeliveryInfosServiceProxy,        
        ApiServiceProxies.OrdersServiceProxy,        
        ApiServiceProxies.OrderStatusesServiceProxy,        
        ApiServiceProxies.OrderSalesChannelsServiceProxy,        
        ApiServiceProxies.ProductWholeSalePricesServiceProxy,        
        ApiServiceProxies.ProductWholeSaleQuantityTypesServiceProxy,        
        ApiServiceProxies.ProductFlashSaleProductMapsServiceProxy,        
        ApiServiceProxies.ProductAndGiftCardMapsServiceProxy,        
        ApiServiceProxies.ProductCategoryVariantMapsServiceProxy,        
        ApiServiceProxies.ProductByVariantsServiceProxy,        
        ApiServiceProxies.ProductOwnerPublicContactInfosServiceProxy,        
        ApiServiceProxies.ProductReviewFeedbacksServiceProxy,        
        ApiServiceProxies.ProductReviewsServiceProxy,        
        ApiServiceProxies.ProductReturnInfosServiceProxy,        
        ApiServiceProxies.ReturnStatusesServiceProxy,        
        ApiServiceProxies.ReturnTypesServiceProxy,        
        ApiServiceProxies.ProductUpsellRelatedProductsServiceProxy,        
        ApiServiceProxies.ProductCrossSellProductsServiceProxy,        
        ApiServiceProxies.ProductAccountTeamsServiceProxy,        
        ApiServiceProxies.SubscriptionTypesServiceProxy,        
        ApiServiceProxies.ProductSubscriptionMapsServiceProxy,        
        ApiServiceProxies.SocialMediasServiceProxy,        
        ApiServiceProxies.ProductCustomerStatsServiceProxy,        
        ApiServiceProxies.ProductFaqsServiceProxy,        
        ApiServiceProxies.ProductCustomerQueriesServiceProxy,        
        ApiServiceProxies.ProductPackagesServiceProxy,        
        ApiServiceProxies.ProductNotesServiceProxy,        
        ApiServiceProxies.ProductMediasServiceProxy,        
        ApiServiceProxies.ProductCategoryTeamsServiceProxy,        
        ApiServiceProxies.ProductCategoryMapsServiceProxy,        
        ApiServiceProxies.ProductVariantsServiceProxy,        
        ApiServiceProxies.ProductVariantCategoriesServiceProxy,        
        ApiServiceProxies.HubNavigationMenusServiceProxy,        
        ApiServiceProxies.MasterNavigationMenusServiceProxy,        
        ApiServiceProxies.HubAccountTeamsServiceProxy,        
        ApiServiceProxies.HubSalesProjectionsServiceProxy,        
        ApiServiceProxies.HubZipCodeMapsServiceProxy,        
        ApiServiceProxies.HubContactsServiceProxy,        
        ApiServiceProxies.HubBusinessesServiceProxy,        
        ApiServiceProxies.HubStoresServiceProxy,        
        ApiServiceProxies.HubProductsServiceProxy,        
        ApiServiceProxies.HubProductCategoriesServiceProxy,        
        ApiServiceProxies.LeadReferralRewardsServiceProxy,        
        ApiServiceProxies.LeadContactsServiceProxy,        
        ApiServiceProxies.LeadNotesServiceProxy,        
        ApiServiceProxies.LeadTasksServiceProxy,        
        ApiServiceProxies.LeadTagsServiceProxy,        
        ApiServiceProxies.LeadSalesTeamsServiceProxy,        
        ApiServiceProxies.LeadPipelineStatusesServiceProxy,        
        ApiServiceProxies.LeadsServiceProxy,        
        ApiServiceProxies.LeadPipelineStagesServiceProxy,        
        ApiServiceProxies.LeadSourcesServiceProxy,        
        ApiServiceProxies.StoreZipCodeMapsServiceProxy,        
        ApiServiceProxies.StoreReviewFeedbacksServiceProxy,        
        ApiServiceProxies.StoreReviewsServiceProxy,        
        ApiServiceProxies.StoreRelevantStoresServiceProxy,        
        ApiServiceProxies.StoreMarketplaceCommissionSettingsServiceProxy,        
        ApiServiceProxies.MarketplaceCommissionTypesServiceProxy,        
        ApiServiceProxies.StoreSalesAlertsServiceProxy,        
        ApiServiceProxies.StoreTaxesServiceProxy,        
        ApiServiceProxies.StoreBankAccountsServiceProxy,        
        ApiServiceProxies.StoreNotesServiceProxy,        
        ApiServiceProxies.StoreMediasServiceProxy,        
        ApiServiceProxies.StoreLocationsServiceProxy,        
        ApiServiceProxies.StoreBusinessHoursServiceProxy,        
        ApiServiceProxies.StoreContactMapsServiceProxy,        
        ApiServiceProxies.StoreBusinessCustomerMapsServiceProxy,        
        ApiServiceProxies.StoreOwnerTeamsServiceProxy,        
        ApiServiceProxies.StoreAccountTeamsServiceProxy,        
        ApiServiceProxies.StoreProductMapsServiceProxy,        
        ApiServiceProxies.StoreProductCategoryMapsServiceProxy,        
        ApiServiceProxies.EmployeeTagsServiceProxy,        
        ApiServiceProxies.BusinessTaskMapsServiceProxy,        
        ApiServiceProxies.BusinessProductMapsServiceProxy,        
        ApiServiceProxies.BusinessStoreMapsServiceProxy,        
        ApiServiceProxies.BusinessJobMapsServiceProxy,        
        ApiServiceProxies.BusinessNotesServiceProxy,        
        ApiServiceProxies.BusinessUsersServiceProxy,        
        ApiServiceProxies.BusinessAccountTeamsServiceProxy,        
        ApiServiceProxies.BusinessContactMapsServiceProxy,        
        ApiServiceProxies.TaskTagsServiceProxy,        
        ApiServiceProxies.TaskEventsServiceProxy,        
        ApiServiceProxies.TaskStatusesServiceProxy,        
        ApiServiceProxies.EmployeesServiceProxy,        
        ApiServiceProxies.JobTagsServiceProxy,        
        ApiServiceProxies.JobsServiceProxy,        
        ApiServiceProxies.JobStatusTypesServiceProxy,        
        ApiServiceProxies.StoreTagsServiceProxy,        
        ApiServiceProxies.ProductTagsServiceProxy,        
        ApiServiceProxies.BusinessTagsServiceProxy,        
        ApiServiceProxies.ContactTagsServiceProxy,        
        ApiServiceProxies.StoresServiceProxy,        
        ApiServiceProxies.ProductsServiceProxy,        
        ApiServiceProxies.ProductCategoriesServiceProxy,        
        ApiServiceProxies.MediaLibrariesServiceProxy,        
        ApiServiceProxies.BusinessesServiceProxy,        
        ApiServiceProxies.ContactsServiceProxy,        
        ApiServiceProxies.HubsServiceProxy,        
        ApiServiceProxies.HubTypesServiceProxy,        
        ApiServiceProxies.MembershipTypesServiceProxy,        
        ApiServiceProxies.ContractTypesServiceProxy,        
        ApiServiceProxies.DocumentTypesServiceProxy,        
        ApiServiceProxies.SmsTemplatesServiceProxy,        
        ApiServiceProxies.EmailTemplatesServiceProxy,        
        ApiServiceProxies.ConnectChannelsServiceProxy,        
        ApiServiceProxies.ZipCodesServiceProxy,        
        ApiServiceProxies.RatingLikesServiceProxy,        
        ApiServiceProxies.MeasurementUnitsServiceProxy,        
        ApiServiceProxies.MasterTagsServiceProxy,        
        ApiServiceProxies.MasterTagCategoriesServiceProxy,        
        ApiServiceProxies.CitiesServiceProxy,        
        ApiServiceProxies.CountiesServiceProxy,        
        ApiServiceProxies.StatesServiceProxy,        
        ApiServiceProxies.CountriesServiceProxy,        
        ApiServiceProxies.CurrenciesServiceProxy,        
        ApiServiceProxies.AuditLogServiceProxy,
        ApiServiceProxies.CachingServiceProxy,
        ApiServiceProxies.ChatServiceProxy,
        ApiServiceProxies.CommonLookupServiceProxy,
        ApiServiceProxies.EditionServiceProxy,
        ApiServiceProxies.FriendshipServiceProxy,
        ApiServiceProxies.HostSettingsServiceProxy,
        ApiServiceProxies.InstallServiceProxy,
        ApiServiceProxies.LanguageServiceProxy,
        ApiServiceProxies.NotificationServiceProxy,
        ApiServiceProxies.OrganizationUnitServiceProxy,
        ApiServiceProxies.PermissionServiceProxy,
        ApiServiceProxies.ProfileServiceProxy,
        ApiServiceProxies.RoleServiceProxy,
        ApiServiceProxies.SessionServiceProxy,
        ApiServiceProxies.TenantServiceProxy,
        ApiServiceProxies.TenantDashboardServiceProxy,
        ApiServiceProxies.TenantSettingsServiceProxy,
        ApiServiceProxies.TimingServiceProxy,
        ApiServiceProxies.UserServiceProxy,
        ApiServiceProxies.UserLinkServiceProxy,
        ApiServiceProxies.UserLoginServiceProxy,
        ApiServiceProxies.WebLogServiceProxy,
        ApiServiceProxies.AccountServiceProxy,
        ApiServiceProxies.TokenAuthServiceProxy,
        ApiServiceProxies.TenantRegistrationServiceProxy,
        ApiServiceProxies.HostDashboardServiceProxy,
        ApiServiceProxies.PaymentServiceProxy,
        ApiServiceProxies.DemoUiComponentsServiceProxy,
        ApiServiceProxies.InvoiceServiceProxy,
        ApiServiceProxies.SubscriptionServiceProxy,
        ApiServiceProxies.InstallServiceProxy,
        ApiServiceProxies.UiCustomizationSettingsServiceProxy,
        ApiServiceProxies.PayPalPaymentServiceProxy,
        ApiServiceProxies.StripePaymentServiceProxy,
        ApiServiceProxies.DashboardCustomizationServiceProxy,
        ApiServiceProxies.WebhookEventServiceProxy,
        ApiServiceProxies.WebhookSubscriptionServiceProxy,
        ApiServiceProxies.WebhookSendAttemptServiceProxy,
        ApiServiceProxies.UserDelegationServiceProxy,
        ApiServiceProxies.DynamicPropertyServiceProxy,
        ApiServiceProxies.DynamicEntityPropertyDefinitionServiceProxy,
        ApiServiceProxies.DynamicEntityPropertyServiceProxy,
        ApiServiceProxies.DynamicPropertyValueServiceProxy,
        ApiServiceProxies.DynamicEntityPropertyValueServiceProxy,
        ApiServiceProxies.TwitterServiceProxy,
        { provide: RefreshTokenService, useClass: ZeroRefreshTokenService },
        { provide: AbpHttpConfigurationService, useClass: ZeroTemplateHttpConfigurationService },
        { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true },
    ],
})
export class ServiceProxyModule {}
