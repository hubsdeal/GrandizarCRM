import { PermissionCheckerService } from 'abp-ng2-module';
import { AppSessionService } from '@shared/common/session/app-session.service';

import { Injectable } from '@angular/core';
import { AppMenu } from './app-menu';
import { AppMenuItem } from './app-menu-item';

@Injectable()
export class AppNavigationService {
    constructor(
        private _permissionCheckerService: PermissionCheckerService,
        private _appSessionService: AppSessionService
    ) { }

    getMenu(): AppMenu {
        return new AppMenu('MainMenu', 'MainMenu', [
            // new AppMenuItem(
            //     'Dashboard',
            //     'Pages.Administration.Host.Dashboard',
            //     'flaticon-line-graph',
            //     '/app/admin/hostDashboard'
            // ),
            new AppMenuItem(
                'Dashboard',
                '',
                'flaticon-dashboard',
                '',
                [],
                [
                    new AppMenuItem(
                        'Admin Dashboard',
                        'Pages.Administration.Host.Dashboard',
                        'flaticon-line-graph',
                        '/app/admin/hostDashboard'
                    ),
                    new AppMenuItem('Master Task List', 'Pages.TaskEvents', 'flaticon2-list-2', '/app/main/taskManagement/taskEvents'),
                    new AppMenuItem('My Task List', 'Pages.TaskEvents', 'flaticon2-list-2', '/app/main/taskManagement/myTaskEvents'),
                    new AppMenuItem('Task Template', 'Pages.TaskEvents', 'flaticon2-list-2', '/app/main/taskManagement/taskLibrary'),
                ]
            ),

            new AppMenuItem(
                'Store Management',
                '',
                'fas fa-store-alt',
                '',
                [],
                [
                    new AppMenuItem('Master Store List', 'Pages.Stores', 'flaticon2-list-1', '/app/main/shop/stores'),
                    new AppMenuItem('My Store List', 'Pages.Stores', 'flaticon2-list-2', '/app/main/shop/myStores'),
                ]
            ),

            new AppMenuItem(
                'Product Catalog',
                '',
                'flaticon-open-box',
                '',
                [],
                [
                    new AppMenuItem('Master Product List', 'Pages.Products', 'flaticon2-list-1', '/app/main/shop/products'),
                    new AppMenuItem('My Product List', 'Pages.Products', 'flaticon2-list-2', '/app/main/shop/products'),
                    new AppMenuItem('Product Libraries', 'Pages.Products', 'flaticon2-delivery-package', '/app/main/shop/productLibraries'),
                    new AppMenuItem('ProductCategories', 'Pages.ProductCategories', 'flaticon-map', '/app/main/shop/productCategories'),
                    new AppMenuItem('MediaLibrary', 'Pages.MediaLibraries', 'fas fa-photo-video', '/app/main/lookupData/mediaLibraries'),
                ]
            ),

            new AppMenuItem(
                'Order Management',
                '',
                'fas fa-shopping-cart',
                '',
                [],
                [
                    new AppMenuItem('Master Order List', 'Pages.Orders', 'fas fa-list', '/app/main/orderManagement/orders'),
                    new AppMenuItem('My Order List', 'Pages.Orders', 'fas fa-list-alt', '/app/main/orderManagement/myOrders'),
                    new AppMenuItem('Abandoned Cart', 'Pages.Orders', 'fas fa-shopping-cart', '/app/main/orderManagement/abandonedCart'),
                ]
            ),

            new AppMenuItem(
                'Job Management',
                '',
                'fas fa-briefcase',
                '',
                [],
                [
                    new AppMenuItem('Master Job List', 'Pages.Jobs', 'fas fa-briefcase', '/app/main/jobManagement/jobs'),
                    new AppMenuItem('My Job List', 'Pages.Jobs', 'fas fa-briefcase', '/app/main/jobManagement/jobs'),
                ]
            ),

            new AppMenuItem(
                'Applicant & Gigs',
                '',
                'flaticon-profile-1',
                '',
                [],
                [
                    new AppMenuItem('Applicants', '', 'fas fa-user-tie', '/app/main/jobManagement/applicants'),
                ]
            ),

            new AppMenuItem(
                'Contact Management',
                '',
                'fas fa-users-cog',
                '',
                [],
                [
                    new AppMenuItem('Businesses', 'Pages.Businesses', 'fas fa-building', '/app/main/crm/businesses'),
                    new AppMenuItem('MyCustomerList', 'Pages.Contacts', 'fas fa-user-friends', '/app/main/crm/contacts'),
                    new AppMenuItem('MasterCustomerList', 'Pages.Contacts', 'fas fa-users', '/app/main/crm/contacts'),
                    new AppMenuItem('PremiumCustomerList', 'Pages.Contacts', 'flaticon-medal', '/app/main/crm/contacts'),
                    new AppMenuItem('ContactRepo', 'Pages.Contacts', 'flaticon-network', '/app/main/crm/contacts'),
                    new AppMenuItem('ReferredContactList', 'Pages.Contacts', 'flaticon-share', '/app/main/crm/contacts'),
                ]
            ),

            

            new AppMenuItem(
                'Employee & Consultants ',
                '',
                'fas fa-user-tie',
                '',
                [],
                [
                    new AppMenuItem('Master Employee List', 'Pages.Employees', 'fas fa-users', '/app/main/crm/employees'),
                    new AppMenuItem('My Employee List', 'Pages.Employees', 'fas fa-user-friends', '/app/main/crm/myEmployees'),
                ]
            ),

            new AppMenuItem(
                'Territory Management',
                '',
                'fas fa-map-marked-alt',
                '',
                [],
                [
                    new AppMenuItem('Master Hub List', 'Pages.Hubs', 'flaticon2-list-1', '/app/main/territory/hubs'),

                    new AppMenuItem('My Hub List', 'Pages.Hubs', 'flaticon2-list-2', '/app/main/territory/myHubs'),
                ]
            ),

            new AppMenuItem(
                'Connect Messaging',
                '',
                'fab fa-connectdevelop',
                '',
                [],
                [
                    new AppMenuItem('ConnectChannels', 'Pages.ConnectChannels', 'flaticon-more', '/app/main/lookupData/connectChannels'),

                    new AppMenuItem('EmailTemplates', 'Pages.EmailTemplates', 'flaticon-more', '/app/main/lookupData/emailTemplates'),

                    new AppMenuItem('SmsTemplates', 'Pages.SmsTemplates', 'flaticon-more', '/app/main/lookupData/smsTemplates'),

                ]
            ),
            new AppMenuItem(
                'CMS & Blogs',
                '',
                'flaticon2-document',
                '',
                [],
                [
                    new AppMenuItem('Site Default Content', 'Pages.Contents', 'flaticon-more', '/app/main/cms/siteDefaultContents'),
                    new AppMenuItem('Contents', 'Pages.Contents', 'flaticon-more', '/app/main/cms/contents'),
                ]
            ),
            new AppMenuItem('Dashboard', 'Pages.Tenant.Dashboard', 'flaticon-line-graph', '/app/main/dashboard'),
            //new AppMenuItem('Tenants', 'Pages.Tenants', 'flaticon-list-3', '/app/admin/tenants'),

            // new AppMenuItem('Currencies', 'Pages.Currencies', 'flaticon-more', '/app/main/lookupData/currencies'),

            // new AppMenuItem('Countries', 'Pages.Countries', 'flaticon-more', '/app/main/lookupData/countries'),

            // new AppMenuItem('States', 'Pages.States', 'flaticon-more', '/app/main/lookupData/states'),

            // new AppMenuItem('Counties', 'Pages.Counties', 'flaticon-more', '/app/main/lookupData/counties'),

            // new AppMenuItem('Cities', 'Pages.Cities', 'flaticon-more', '/app/main/lookupData/cities'),

            // new AppMenuItem('MasterTagCategories', 'Pages.MasterTagCategories', 'flaticon-more', '/app/main/lookupData/masterTagCategories'),

            // new AppMenuItem('MasterTags', 'Pages.MasterTags', 'flaticon-more', '/app/main/lookupData/masterTags'),

            // new AppMenuItem('MeasurementUnits', 'Pages.MeasurementUnits', 'flaticon-more', '/app/main/lookupData/measurementUnits'),

            // new AppMenuItem('RatingLikes', 'Pages.RatingLikes', 'flaticon-more', '/app/main/lookupData/ratingLikes'),

            // new AppMenuItem('ZipCodes', 'Pages.ZipCodes', 'flaticon-more', '/app/main/lookupData/zipCodes'),

            // new AppMenuItem('ConnectChannels', 'Pages.ConnectChannels', 'flaticon-more', '/app/main/lookupData/connectChannels'),

            // new AppMenuItem('EmailTemplates', 'Pages.EmailTemplates', 'flaticon-more', '/app/main/lookupData/emailTemplates'),

            // new AppMenuItem('SmsTemplates', 'Pages.SmsTemplates', 'flaticon-more', '/app/main/lookupData/smsTemplates'),

            // new AppMenuItem('DocumentTypes', 'Pages.DocumentTypes', 'flaticon-more', '/app/main/lookupData/documentTypes'),

            // new AppMenuItem('ContractTypes', 'Pages.ContractTypes', 'flaticon-more', '/app/main/lookupData/contractTypes'),

            // new AppMenuItem('MembershipTypes', 'Pages.MembershipTypes', 'flaticon-more', '/app/main/lookupData/membershipTypes'),

            // new AppMenuItem('HubTypes', 'Pages.HubTypes', 'flaticon-more', '/app/main/lookupData/hubTypes'),

            // new AppMenuItem('Hubs', 'Pages.Hubs', 'flaticon-more', '/app/main/territory/hubs'),

            // new AppMenuItem('Contacts', 'Pages.Contacts', 'flaticon-more', '/app/main/crm/contacts'),

            // new AppMenuItem('Businesses', 'Pages.Businesses', 'flaticon-more', '/app/main/crm/businesses'),

            // new AppMenuItem('MediaLibraries', 'Pages.MediaLibraries', 'flaticon-more', '/app/main/lookupData/mediaLibraries'),

            // new AppMenuItem('ProductCategories', 'Pages.ProductCategories', 'flaticon-more', '/app/main/shop/productCategories'),

            // new AppMenuItem('Products', 'Pages.Products', 'flaticon-more', '/app/main/shop/products'),

            // new AppMenuItem('Stores', 'Pages.Stores', 'flaticon-more', '/app/main/shop/stores'),

            // new AppMenuItem('ContactTags', 'Pages.ContactTags', 'flaticon-more', '/app/main/crm/contactTags'),

            // new AppMenuItem('BusinessTags', 'Pages.BusinessTags', 'flaticon-more', '/app/main/crm/businessTags'),

            // new AppMenuItem('ProductTags', 'Pages.ProductTags', 'flaticon-more', '/app/main/shop/productTags'),

            // new AppMenuItem('StoreTags', 'Pages.StoreTags', 'flaticon-more', '/app/main/shop/storeTags'),

            // new AppMenuItem('JobStatusTypes', 'Pages.JobStatusTypes', 'flaticon-more', '/app/main/jobManagement/jobStatusTypes'),

            // new AppMenuItem('Jobs', 'Pages.Jobs', 'flaticon-more', '/app/main/jobManagement/jobs'),

            // new AppMenuItem('JobTags', 'Pages.JobTags', 'flaticon-more', '/app/main/jobManagement/jobTags'),

            // new AppMenuItem('Employees', 'Pages.Employees', 'flaticon-more', '/app/main/crm/employees'),

            // new AppMenuItem('TaskStatuses', 'Pages.TaskStatuses', 'flaticon-more', '/app/main/taskManagement/taskStatuses'),

            // new AppMenuItem('TaskEvents', 'Pages.TaskEvents', 'flaticon-more', '/app/main/taskManagement/taskEvents'),

            // new AppMenuItem('TaskTags', 'Pages.TaskTags', 'flaticon-more', '/app/main/taskManagement/taskTags'),

            // new AppMenuItem('BusinessContactMaps', 'Pages.BusinessContactMaps', 'flaticon-more', '/app/main/crm/businessContactMaps'),

            // new AppMenuItem('BusinessAccountTeams', 'Pages.BusinessAccountTeams', 'flaticon-more', '/app/main/crm/businessAccountTeams'),

            // new AppMenuItem('BusinessUsers', 'Pages.BusinessUsers', 'flaticon-more', '/app/main/crm/businessUsers'),

            // new AppMenuItem('BusinessNotes', 'Pages.BusinessNotes', 'flaticon-more', '/app/main/crm/businessNotes'),

            // new AppMenuItem('BusinessJobMaps', 'Pages.BusinessJobMaps', 'flaticon-more', '/app/main/crm/businessJobMaps'),

            // new AppMenuItem('BusinessStoreMaps', 'Pages.BusinessStoreMaps', 'flaticon-more', '/app/main/crm/businessStoreMaps'),

            // new AppMenuItem('BusinessProductMaps', 'Pages.BusinessProductMaps', 'flaticon-more', '/app/main/crm/businessProductMaps'),

            // new AppMenuItem('BusinessTaskMaps', 'Pages.BusinessTaskMaps', 'flaticon-more', '/app/main/crm/businessTaskMaps'),

            // new AppMenuItem('EmployeeTags', 'Pages.EmployeeTags', 'flaticon-more', '/app/main/crm/employeeTags'),

            // new AppMenuItem('StoreProductCategoryMaps', 'Pages.StoreProductCategoryMaps', 'flaticon-more', '/app/main/shop/storeProductCategoryMaps'),

            // new AppMenuItem('StoreProductMaps', 'Pages.StoreProductMaps', 'flaticon-more', '/app/main/shop/storeProductMaps'),

            // new AppMenuItem('StoreAccountTeams', 'Pages.StoreAccountTeams', 'flaticon-more', '/app/main/shop/storeAccountTeams'),

            // new AppMenuItem('StoreOwnerTeams', 'Pages.StoreOwnerTeams', 'flaticon-more', '/app/main/shop/storeOwnerTeams'),

            // new AppMenuItem('StoreBusinessCustomerMaps', 'Pages.StoreBusinessCustomerMaps', 'flaticon-more', '/app/main/shop/storeBusinessCustomerMaps'),

            // new AppMenuItem('StoreContactMaps', 'Pages.StoreContactMaps', 'flaticon-more', '/app/main/shop/storeContactMaps'),

            // new AppMenuItem('StoreBusinessHours', 'Pages.StoreBusinessHours', 'flaticon-more', '/app/main/shop/storeBusinessHours'),

            // new AppMenuItem('StoreLocations', 'Pages.StoreLocations', 'flaticon-more', '/app/main/shop/storeLocations'),

            // new AppMenuItem('StoreMedias', 'Pages.StoreMedias', 'flaticon-more', '/app/main/shop/storeMedias'),

            // new AppMenuItem('StoreNotes', 'Pages.StoreNotes', 'flaticon-more', '/app/main/shop/storeNotes'),

            // new AppMenuItem('StoreBankAccounts', 'Pages.StoreBankAccounts', 'flaticon-more', '/app/main/shop/storeBankAccounts'),

            // new AppMenuItem('StoreTaxes', 'Pages.StoreTaxes', 'flaticon-more', '/app/main/shop/storeTaxes'),

            // new AppMenuItem('StoreSalesAlerts', 'Pages.StoreSalesAlerts', 'flaticon-more', '/app/main/shop/storeSalesAlerts'),

            // new AppMenuItem('MarketplaceCommissionTypes', 'Pages.MarketplaceCommissionTypes', 'flaticon-more', '/app/main/shop/marketplaceCommissionTypes'),

            // new AppMenuItem('StoreMarketplaceCommissionSettings', 'Pages.StoreMarketplaceCommissionSettings', 'flaticon-more', '/app/main/shop/storeMarketplaceCommissionSettings'),

            // new AppMenuItem('StoreRelevantStores', 'Pages.StoreRelevantStores', 'flaticon-more', '/app/main/shop/storeRelevantStores'),

            // new AppMenuItem('StoreReviews', 'Pages.StoreReviews', 'flaticon-more', '/app/main/shop/storeReviews'),

            // new AppMenuItem('StoreReviewFeedbacks', 'Pages.StoreReviewFeedbacks', 'flaticon-more', '/app/main/shop/storeReviewFeedbacks'),

            // new AppMenuItem('StoreZipCodeMaps', 'Pages.StoreZipCodeMaps', 'flaticon-more', '/app/main/shop/storeZipCodeMaps'),

            // new AppMenuItem('LeadSources', 'Pages.LeadSources', 'flaticon-more', '/app/main/salesLeadManagement/leadSources'),

            // new AppMenuItem('LeadPipelineStages', 'Pages.LeadPipelineStages', 'flaticon-more', '/app/main/salesLeadManagement/leadPipelineStages'),

            // new AppMenuItem('Leads', 'Pages.Leads', 'flaticon-more', '/app/main/salesLeadManagement/leads'),

            // new AppMenuItem('LeadPipelineStatuses', 'Pages.LeadPipelineStatuses', 'flaticon-more', '/app/main/salesLeadManagement/leadPipelineStatuses'),

            // new AppMenuItem('LeadSalesTeams', 'Pages.LeadSalesTeams', 'flaticon-more', '/app/main/salesLeadManagement/leadSalesTeams'),

            // new AppMenuItem('LeadTags', 'Pages.LeadTags', 'flaticon-more', '/app/main/salesLeadManagement/leadTags'),

            // new AppMenuItem('LeadTasks', 'Pages.LeadTasks', 'flaticon-more', '/app/main/salesLeadManagement/leadTasks'),

            // new AppMenuItem('LeadNotes', 'Pages.LeadNotes', 'flaticon-more', '/app/main/salesLeadManagement/leadNotes'),

            // new AppMenuItem('LeadContacts', 'Pages.LeadContacts', 'flaticon-more', '/app/main/salesLeadManagement/leadContacts'),

            // new AppMenuItem('LeadReferralRewards', 'Pages.LeadReferralRewards', 'flaticon-more', '/app/main/salesLeadManagement/leadReferralRewards'),

            // new AppMenuItem('HubProductCategories', 'Pages.HubProductCategories', 'flaticon-more', '/app/main/territory/hubProductCategories'),

            // new AppMenuItem('HubProducts', 'Pages.HubProducts', 'flaticon-more', '/app/main/territory/hubProducts'),

            // new AppMenuItem('HubStores', 'Pages.HubStores', 'flaticon-more', '/app/main/territory/hubStores'),

            // new AppMenuItem('HubBusinesses', 'Pages.HubBusinesses', 'flaticon-more', '/app/main/territory/hubBusinesses'),

            // new AppMenuItem('HubContacts', 'Pages.HubContacts', 'flaticon-more', '/app/main/territory/hubContacts'),

            // new AppMenuItem('HubZipCodeMaps', 'Pages.HubZipCodeMaps', 'flaticon-more', '/app/main/territory/hubZipCodeMaps'),

            // new AppMenuItem('HubSalesProjections', 'Pages.HubSalesProjections', 'flaticon-more', '/app/main/territory/hubSalesProjections'),

            // new AppMenuItem('HubAccountTeams', 'Pages.HubAccountTeams', 'flaticon-more', '/app/main/territory/hubAccountTeams'),

            // new AppMenuItem('MasterNavigationMenus', 'Pages.MasterNavigationMenus', 'flaticon-more', '/app/main/territory/masterNavigationMenus'),

            // new AppMenuItem('HubNavigationMenus', 'Pages.HubNavigationMenus', 'flaticon-more', '/app/main/territory/hubNavigationMenus'),

            // new AppMenuItem('ProductVariantCategories', 'Pages.ProductVariantCategories', 'flaticon-more', '/app/main/shop/productVariantCategories'),

            // new AppMenuItem('ProductVariants', 'Pages.ProductVariants', 'flaticon-more', '/app/main/shop/productVariants'),

            // new AppMenuItem('ProductCategoryMaps', 'Pages.ProductCategoryMaps', 'flaticon-more', '/app/main/shop/productCategoryMaps'),

            // new AppMenuItem('ProductCategoryTeams', 'Pages.ProductCategoryTeams', 'flaticon-more', '/app/main/shop/productCategoryTeams'),

            // new AppMenuItem('ProductMedias', 'Pages.ProductMedias', 'flaticon-more', '/app/main/shop/productMedias'),

            // new AppMenuItem('ProductNotes', 'Pages.ProductNotes', 'flaticon-more', '/app/main/shop/productNotes'),

            // new AppMenuItem('ProductPackages', 'Pages.ProductPackages', 'flaticon-more', '/app/main/shop/productPackages'),

            // new AppMenuItem('ProductCustomerQueries', 'Pages.ProductCustomerQueries', 'flaticon-more', '/app/main/shop/productCustomerQueries'),

            // new AppMenuItem('ProductFaqs', 'Pages.ProductFaqs', 'flaticon-more', '/app/main/shop/productFaqs'),

            // new AppMenuItem('ProductCustomerStats', 'Pages.ProductCustomerStats', 'flaticon-more', '/app/main/shop/productCustomerStats'),

            // new AppMenuItem('SocialMedias', 'Pages.SocialMedias', 'flaticon-more', '/app/main/lookupData/socialMedias'),

            // new AppMenuItem('ProductSubscriptionMaps', 'Pages.ProductSubscriptionMaps', 'flaticon-more', '/app/main/shop/productSubscriptionMaps'),

            // new AppMenuItem('SubscriptionTypes', 'Pages.SubscriptionTypes', 'flaticon-more', '/app/main/lookupData/subscriptionTypes'),

            // new AppMenuItem('ProductAccountTeams', 'Pages.ProductAccountTeams', 'flaticon-more', '/app/main/shop/productAccountTeams'),

            // new AppMenuItem('ProductCrossSellProducts', 'Pages.ProductCrossSellProducts', 'flaticon-more', '/app/main/shop/productCrossSellProducts'),

            // new AppMenuItem('ProductUpsellRelatedProducts', 'Pages.ProductUpsellRelatedProducts', 'flaticon-more', '/app/main/shop/productUpsellRelatedProducts'),

            // new AppMenuItem('ReturnTypes', 'Pages.ReturnTypes', 'flaticon-more', '/app/main/lookupData/returnTypes'),

            // new AppMenuItem('ReturnStatuses', 'Pages.ReturnStatuses', 'flaticon-more', '/app/main/lookupData/returnStatuses'),

            // new AppMenuItem('ProductReturnInfos', 'Pages.ProductReturnInfos', 'flaticon-more', '/app/main/shop/productReturnInfos'),

            // new AppMenuItem('ProductReviews', 'Pages.ProductReviews', 'flaticon-more', '/app/main/shop/productReviews'),

            // new AppMenuItem('ProductReviewFeedbacks', 'Pages.ProductReviewFeedbacks', 'flaticon-more', '/app/main/shop/productReviewFeedbacks'),

            // new AppMenuItem('ProductOwnerPublicContactInfos', 'Pages.ProductOwnerPublicContactInfos', 'flaticon-more', '/app/main/shop/productOwnerPublicContactInfos'),

            // new AppMenuItem('ProductByVariants', 'Pages.ProductByVariants', 'flaticon-more', '/app/main/shop/productByVariants'),

            // new AppMenuItem('ProductCategoryVariantMaps', 'Pages.ProductCategoryVariantMaps', 'flaticon-more', '/app/main/shop/productCategoryVariantMaps'),

            // new AppMenuItem('ProductAndGiftCardMaps', 'Pages.ProductAndGiftCardMaps', 'flaticon-more', '/app/main/shop/productAndGiftCardMaps'),

            // new AppMenuItem('ProductFlashSaleProductMaps', 'Pages.ProductFlashSaleProductMaps', 'flaticon-more', '/app/main/shop/productFlashSaleProductMaps'),

            // new AppMenuItem('ProductWholeSaleQuantityTypes', 'Pages.ProductWholeSaleQuantityTypes', 'flaticon-more', '/app/main/lookupData/productWholeSaleQuantityTypes'),

            // new AppMenuItem('ProductWholeSalePrices', 'Pages.ProductWholeSalePrices', 'flaticon-more', '/app/main/shop/productWholeSalePrices'),

            // new AppMenuItem('OrderSalesChannels', 'Pages.OrderSalesChannels', 'flaticon-more', '/app/main/orderManagement/orderSalesChannels'),

            // new AppMenuItem('OrderStatuses', 'Pages.OrderStatuses', 'flaticon-more', '/app/main/orderManagement/orderStatuses'),

            // new AppMenuItem('Orders', 'Pages.Orders', 'flaticon-more', '/app/main/orderManagement/orders'),

            // new AppMenuItem('OrderDeliveryInfos', 'Pages.OrderDeliveryInfos', 'flaticon-more', '/app/main/orderManagement/orderDeliveryInfos'),

            // new AppMenuItem('OrderFulfillmentStatuses', 'Pages.OrderFulfillmentStatuses', 'flaticon-more', '/app/main/orderManagement/orderFulfillmentStatuses'),

            // new AppMenuItem('OrderfulfillmentTeams', 'Pages.OrderfulfillmentTeams', 'flaticon-more', '/app/main/orderManagement/orderfulfillmentTeams'),

            // new AppMenuItem('OrderPaymentInfos', 'Pages.OrderPaymentInfos', 'flaticon-more', '/app/main/orderManagement/orderPaymentInfos'),

            // new AppMenuItem('PaymentTypes', 'Pages.PaymentTypes', 'flaticon-more', '/app/main/lookupData/paymentTypes'),

            // new AppMenuItem('OrderProductInfos', 'Pages.OrderProductInfos', 'flaticon-more', '/app/main/orderManagement/orderProductInfos'),

            // new AppMenuItem('OrderProductVariants', 'Pages.OrderProductVariants', 'flaticon-more', '/app/main/orderManagement/orderProductVariants'),

            // new AppMenuItem('OrderTeams', 'Pages.OrderTeams', 'flaticon-more', '/app/main/orderManagement/orderTeams'),

            // new AppMenuItem('CustomerWallets', 'Pages.CustomerWallets', 'flaticon-more', '/app/main/discountManagement/customerWallets'),

            // new AppMenuItem('DiscountCodeGenerators', 'Pages.DiscountCodeGenerators', 'flaticon-more', '/app/main/discountManagement/discountCodeGenerators'),

            // new AppMenuItem('DiscountCodeByCustomers', 'Pages.DiscountCodeByCustomers', 'flaticon-more', '/app/main/discountManagement/discountCodeByCustomers'),

            // new AppMenuItem('DiscountCodeMaps', 'Pages.DiscountCodeMaps', 'flaticon-more', '/app/main/discountManagement/discountCodeMaps'),

            // new AppMenuItem('DiscountCodeUserHistories', 'Pages.DiscountCodeUserHistories', 'flaticon-more', '/app/main/discountManagement/discountCodeUserHistories'),
           
            new AppMenuItem('WishLists', 'Pages.WishLists', 'flaticon-more', '/app/main/shop/wishLists'),
            
            new AppMenuItem('ShoppingCarts', 'Pages.ShoppingCarts', 'flaticon-more', '/app/main/shop/shoppingCarts'),
            
            new AppMenuItem('ProductTeams', 'Pages.ProductTeams', 'flaticon-more', '/app/main/shop/productTeams'),
            
            
            
            new AppMenuItem('StoreTaskMaps', 'Pages.StoreTaskMaps', 'flaticon-more', '/app/main/taskManagement/storeTaskMaps'),
            
            new AppMenuItem('BusinessDocuments', 'Pages.BusinessDocuments', 'flaticon-more', '/app/main/crm/businessDocuments'),
            
            new AppMenuItem('StoreTagSettingCategories', 'Pages.StoreTagSettingCategories', 'flaticon-more', '/app/main/shop/storeTagSettingCategories'),
            
            new AppMenuItem('StoreMasterTagSettings', 'Pages.StoreMasterTagSettings', 'flaticon-more', '/app/main/shop/storeMasterTagSettings'),
            
            new AppMenuItem('ProductMasterTagSettings', 'Pages.ProductMasterTagSettings', 'flaticon-more', '/app/main/shop/productMasterTagSettings'),
            
            new AppMenuItem('ContactMasterTagSettings', 'Pages.ContactMasterTagSettings', 'flaticon-more', '/app/main/crm/contactMasterTagSettings'),
            
            new AppMenuItem('BusinessMasterTagSettings', 'Pages.BusinessMasterTagSettings', 'flaticon-more', '/app/main/crm/businessMasterTagSettings'),
            
            new AppMenuItem('JobMasterTagSettings', 'Pages.JobMasterTagSettings', 'flaticon-more', '/app/main/jobManagement/jobMasterTagSettings'),
            
            new AppMenuItem('ContactTaskMaps', 'Pages.ContactTaskMaps', 'flaticon-more', '/app/main/crm/contactTaskMaps'),
            
            new AppMenuItem('ProductTaskMaps', 'Pages.ProductTaskMaps', 'flaticon-more', '/app/main/shop/productTaskMaps'),
            
            new AppMenuItem('JobTaskMaps', 'Pages.JobTaskMaps', 'flaticon-more', '/app/main/jobManagement/jobTaskMaps'),
            
            new AppMenuItem('TaskTeams', 'Pages.TaskTeams', 'flaticon-more', '/app/main/taskManagement/taskTeams'),
            
            new AppMenuItem('TaskWorkItems', 'Pages.TaskWorkItems', 'flaticon-more', '/app/main/taskManagement/taskWorkItems'),
             new AppMenuItem('Editions', 'Pages.Editions', 'flaticon-app', '/app/admin/editions'),
            new AppMenuItem(
                'Administration',
                '',
                'flaticon-interface-8',
                '',
                [],
                [
                    new AppMenuItem(
                        'OrganizationUnits',
                        'Pages.Administration.OrganizationUnits',
                        'flaticon-map',
                        '/app/admin/organization-units'
                    ),
                    new AppMenuItem('Roles', 'Pages.Administration.Roles', 'flaticon-suitcase', '/app/admin/roles'),
                    new AppMenuItem('Users', 'Pages.Administration.Users', 'flaticon-users', '/app/admin/users'),
                    new AppMenuItem('App-Site Map', 'Pages.Administration.Users', 'flaticon-map', '/app/main/lookupData/masterDataIndex'),
                    new AppMenuItem(
                        'Languages',
                        'Pages.Administration.Languages',
                        'flaticon-tabs',
                        '/app/admin/languages',
                        ['/app/admin/languages/{name}/texts']
                    ),
                    new AppMenuItem(
                        'AuditLogs',
                        'Pages.Administration.AuditLogs',
                        'flaticon-folder-1',
                        '/app/admin/auditLogs'
                    ),
                    new AppMenuItem(
                        'Maintenance',
                        'Pages.Administration.Host.Maintenance',
                        'flaticon-lock',
                        '/app/admin/maintenance'
                    ),
                    new AppMenuItem(
                        'Subscription',
                        'Pages.Administration.Tenant.SubscriptionManagement',
                        'flaticon-refresh',
                        '/app/admin/subscription-management'
                    ),
                    new AppMenuItem(
                        'VisualSettings',
                        'Pages.Administration.UiCustomization',
                        'flaticon-medical',
                        '/app/admin/ui-customization'
                    ),
                    new AppMenuItem(
                        'WebhookSubscriptions',
                        'Pages.Administration.WebhookSubscription',
                        'flaticon2-world',
                        '/app/admin/webhook-subscriptions'
                    ),
                    new AppMenuItem(
                        'DynamicProperties',
                        'Pages.Administration.DynamicProperties',
                        'flaticon-interface-8',
                        '/app/admin/dynamic-property'
                    ),
                    new AppMenuItem(
                        'Settings',
                        'Pages.Administration.Host.Settings',
                        'flaticon-settings',
                        '/app/admin/hostSettings'
                    ),
                    new AppMenuItem(
                        'Settings',
                        'Pages.Administration.Tenant.Settings',
                        'flaticon-settings',
                        '/app/admin/tenantSettings'
                    ),
                    new AppMenuItem(
                        'Notifications',
                        '',
                        'flaticon-alarm',
                        '',
                        [],
                        [
                            new AppMenuItem(
                                'Inbox',
                                '',
                                'flaticon-mail-1',
                                '/app/notifications'
                            ),
                            new AppMenuItem(
                                'MassNotifications',
                                'Pages.Administration.MassNotification',
                                'flaticon-paper-plane',
                                '/app/admin/mass-notifications'
                            )
                        ]
                    )
                ]
            ),
            new AppMenuItem(
                'DemoUiComponents',
                'Pages.DemoUiComponents',
                'flaticon-shapes',
                '/app/admin/demo-ui-components'
            ),
        ]);
    }

    checkChildMenuItemPermission(menuItem): boolean {
        for (let i = 0; i < menuItem.items.length; i++) {
            let subMenuItem = menuItem.items[i];

            if (subMenuItem.permissionName === '' || subMenuItem.permissionName === null) {
                if (subMenuItem.route) {
                    return true;
                }
            } else if (this._permissionCheckerService.isGranted(subMenuItem.permissionName)) {
                return true;
            }

            if (subMenuItem.items && subMenuItem.items.length) {
                let isAnyChildItemActive = this.checkChildMenuItemPermission(subMenuItem);
                if (isAnyChildItemActive) {
                    return true;
                }
            }
        }
        return false;
    }

    showMenuItem(menuItem: AppMenuItem): boolean {
        if (
            menuItem.permissionName === 'Pages.Administration.Tenant.SubscriptionManagement' &&
            this._appSessionService.tenant &&
            !this._appSessionService.tenant.edition
        ) {
            return false;
        }

        let hideMenuItem = false;

        if (menuItem.requiresAuthentication && !this._appSessionService.user) {
            hideMenuItem = true;
        }

        if (menuItem.permissionName && !this._permissionCheckerService.isGranted(menuItem.permissionName)) {
            hideMenuItem = true;
        }

        if (this._appSessionService.tenant || !abp.multiTenancy.ignoreFeatureCheckForHostUsers) {
            if (menuItem.hasFeatureDependency() && !menuItem.featureDependencySatisfied()) {
                hideMenuItem = true;
            }
        }

        if (!hideMenuItem && menuItem.items && menuItem.items.length) {
            return this.checkChildMenuItemPermission(menuItem);
        }

        return !hideMenuItem;
    }

    /**
     * Returns all menu items recursively
     */
    getAllMenuItems(): AppMenuItem[] {
        let menu = this.getMenu();
        let allMenuItems: AppMenuItem[] = [];
        menu.items.forEach((menuItem) => {
            allMenuItems = allMenuItems.concat(this.getAllMenuItemsRecursive(menuItem));
        });

        return allMenuItems;
    }

    private getAllMenuItemsRecursive(menuItem: AppMenuItem): AppMenuItem[] {
        if (!menuItem.items) {
            return [menuItem];
        }

        let menuItems = [menuItem];
        menuItem.items.forEach((subMenu) => {
            menuItems = menuItems.concat(this.getAllMenuItemsRecursive(subMenu));
        });

        return menuItems;
    }
}
