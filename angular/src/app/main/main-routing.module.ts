import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MasterDataIndexComponent } from './lookupData/master-data-index/master-data-index.component';
import { MyHubListComponent } from './territory/hubs/my-hub-list/my-hub-list.component';
import { MyStoresComponent } from './shop/stores/my-stores/my-stores.component';
import { components } from '@metronic/app/kt';
import { MyEmployeesComponent } from './crm/employees/my-employees/my-employees.component';
import { ProductLibrariesComponent } from './shop/products/product-libraries/product-libraries.component';
import { TaskLibrariesComponent } from './taskManagement/task-libraries/task-libraries.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    
                    {
                        path: 'crm/businessDocuments',
                        loadChildren: () => import('./crm/businessDocuments/businessDocument.module').then(m => m.BusinessDocumentModule),
                        data: { permission: 'Pages.BusinessDocuments' }
                    },
                
                    
                    {
                        path: 'taskManagement/storeTaskMaps',
                        loadChildren: () => import('./taskManagement/storeTaskMaps/storeTaskMap.module').then(m => m.StoreTaskMapModule),
                        data: { permission: 'Pages.StoreTaskMaps' }
                    },
                
                    
                    {
                        path: 'cms/contents',
                        loadChildren: () => import('./cms/contents/content.module').then(m => m.ContentModule),
                        data: { permission: 'Pages.Contents' }
                    },
                
                    
                    {
                        path: 'shop/productTeams',
                        loadChildren: () => import('./shop/productTeams/productTeam.module').then(m => m.ProductTeamModule),
                        data: { permission: 'Pages.ProductTeams' }
                    },
                
                    
                    {
                        path: 'shop/shoppingCarts',
                        loadChildren: () => import('./shop/shoppingCarts/shoppingCart.module').then(m => m.ShoppingCartModule),
                        data: { permission: 'Pages.ShoppingCarts' }
                    },
                
                    
                    {
                        path: 'shop/wishLists',
                        loadChildren: () => import('./shop/wishLists/wishList.module').then(m => m.WishListModule),
                        data: { permission: 'Pages.WishLists' }
                    },
                
                    
                    {
                        path: 'discountManagement/discountCodeUserHistories',
                        loadChildren: () => import('./discountManagement/discountCodeUserHistories/discountCodeUserHistory.module').then(m => m.DiscountCodeUserHistoryModule),
                        data: { permission: 'Pages.DiscountCodeUserHistories' }
                    },
                
                    
                    {
                        path: 'discountManagement/discountCodeMaps',
                        loadChildren: () => import('./discountManagement/discountCodeMaps/discountCodeMap.module').then(m => m.DiscountCodeMapModule),
                        data: { permission: 'Pages.DiscountCodeMaps' }
                    },
                
                    
                    {
                        path: 'discountManagement/discountCodeByCustomers',
                        loadChildren: () => import('./discountManagement/discountCodeByCustomers/discountCodeByCustomer.module').then(m => m.DiscountCodeByCustomerModule),
                        data: { permission: 'Pages.DiscountCodeByCustomers' }
                    },
                
                    
                    {
                        path: 'discountManagement/discountCodeGenerators',
                        loadChildren: () => import('./discountManagement/discountCodeGenerators/discountCodeGenerator.module').then(m => m.DiscountCodeGeneratorModule),
                        data: { permission: 'Pages.DiscountCodeGenerators' }
                    },
                
                    
                    {
                        path: 'discountManagement/customerWallets',
                        loadChildren: () => import('./discountManagement/customerWallets/customerWallet.module').then(m => m.CustomerWalletModule),
                        data: { permission: 'Pages.CustomerWallets' }
                    },
                
                    
                    {
                        path: 'orderManagement/orderTeams',
                        loadChildren: () => import('./orderManagement/orderTeams/orderTeam.module').then(m => m.OrderTeamModule),
                        data: { permission: 'Pages.OrderTeams' }
                    },
                
                    
                    {
                        path: 'orderManagement/orderProductVariants',
                        loadChildren: () => import('./orderManagement/orderProductVariants/orderProductVariant.module').then(m => m.OrderProductVariantModule),
                        data: { permission: 'Pages.OrderProductVariants' }
                    },
                
                    
                    {
                        path: 'orderManagement/orderProductInfos',
                        loadChildren: () => import('./orderManagement/orderProductInfos/orderProductInfo.module').then(m => m.OrderProductInfoModule),
                        data: { permission: 'Pages.OrderProductInfos' }
                    },
                
                    
                    {
                        path: 'lookupData/paymentTypes',
                        loadChildren: () => import('./lookupData/paymentTypes/paymentType.module').then(m => m.PaymentTypeModule),
                        data: { permission: 'Pages.PaymentTypes' }
                    },
                
                    
                    {
                        path: 'orderManagement/orderPaymentInfos',
                        loadChildren: () => import('./orderManagement/orderPaymentInfos/orderPaymentInfo.module').then(m => m.OrderPaymentInfoModule),
                        data: { permission: 'Pages.OrderPaymentInfos' }
                    },
                
                    
                    {
                        path: 'orderManagement/orderfulfillmentTeams',
                        loadChildren: () => import('./orderManagement/orderfulfillmentTeams/orderfulfillmentTeam.module').then(m => m.OrderfulfillmentTeamModule),
                        data: { permission: 'Pages.OrderfulfillmentTeams' }
                    },
                
                    
                    {
                        path: 'orderManagement/orderFulfillmentStatuses',
                        loadChildren: () => import('./orderManagement/orderFulfillmentStatuses/orderFulfillmentStatus.module').then(m => m.OrderFulfillmentStatusModule),
                        data: { permission: 'Pages.OrderFulfillmentStatuses' }
                    },
                
                    
                    {
                        path: 'orderManagement/orderDeliveryInfos',
                        loadChildren: () => import('./orderManagement/orderDeliveryInfos/orderDeliveryInfo.module').then(m => m.OrderDeliveryInfoModule),
                        data: { permission: 'Pages.OrderDeliveryInfos' }
                    },
                
                    
                    {
                        path: 'orderManagement/orders',
                        loadChildren: () => import('./orderManagement/orders/order.module').then(m => m.OrderModule),
                        data: { permission: 'Pages.Orders' }
                    },
                
                    
                    {
                        path: 'orderManagement/orderStatuses',
                        loadChildren: () => import('./orderManagement/orderStatuses/orderStatus.module').then(m => m.OrderStatusModule),
                        data: { permission: 'Pages.OrderStatuses' }
                    },
                
                    
                    {
                        path: 'orderManagement/orderSalesChannels',
                        loadChildren: () => import('./orderManagement/orderSalesChannels/orderSalesChannel.module').then(m => m.OrderSalesChannelModule),
                        data: { permission: 'Pages.OrderSalesChannels' }
                    },
                
                    
                    {
                        path: 'shop/productWholeSalePrices',
                        loadChildren: () => import('./shop/productWholeSalePrices/productWholeSalePrice.module').then(m => m.ProductWholeSalePriceModule),
                        data: { permission: 'Pages.ProductWholeSalePrices' }
                    },
                
                    
                    {
                        path: 'lookupData/productWholeSaleQuantityTypes',
                        loadChildren: () => import('./lookupData/productWholeSaleQuantityTypes/productWholeSaleQuantityType.module').then(m => m.ProductWholeSaleQuantityTypeModule),
                        data: { permission: 'Pages.ProductWholeSaleQuantityTypes' }
                    },
                
                    
                    {
                        path: 'shop/productFlashSaleProductMaps',
                        loadChildren: () => import('./shop/productFlashSaleProductMaps/productFlashSaleProductMap.module').then(m => m.ProductFlashSaleProductMapModule),
                        data: { permission: 'Pages.ProductFlashSaleProductMaps' }
                    },
                
                    
                    {
                        path: 'shop/productAndGiftCardMaps',
                        loadChildren: () => import('./shop/productAndGiftCardMaps/productAndGiftCardMap.module').then(m => m.ProductAndGiftCardMapModule),
                        data: { permission: 'Pages.ProductAndGiftCardMaps' }
                    },
                
                    
                    {
                        path: 'shop/productCategoryVariantMaps',
                        loadChildren: () => import('./shop/productCategoryVariantMaps/productCategoryVariantMap.module').then(m => m.ProductCategoryVariantMapModule),
                        data: { permission: 'Pages.ProductCategoryVariantMaps' }
                    },
                
                    
                    {
                        path: 'shop/productByVariants',
                        loadChildren: () => import('./shop/productByVariants/productByVariant.module').then(m => m.ProductByVariantModule),
                        data: { permission: 'Pages.ProductByVariants' }
                    },
                
                    
                    {
                        path: 'shop/productOwnerPublicContactInfos',
                        loadChildren: () => import('./shop/productOwnerPublicContactInfos/productOwnerPublicContactInfo.module').then(m => m.ProductOwnerPublicContactInfoModule),
                        data: { permission: 'Pages.ProductOwnerPublicContactInfos' }
                    },
                
                    
                    {
                        path: 'shop/productReviewFeedbacks',
                        loadChildren: () => import('./shop/productReviewFeedbacks/productReviewFeedback.module').then(m => m.ProductReviewFeedbackModule),
                        data: { permission: 'Pages.ProductReviewFeedbacks' }
                    },
                
                    
                    {
                        path: 'shop/productReviews',
                        loadChildren: () => import('./shop/productReviews/productReview.module').then(m => m.ProductReviewModule),
                        data: { permission: 'Pages.ProductReviews' }
                    },
                
                    
                    {
                        path: 'shop/productReturnInfos',
                        loadChildren: () => import('./shop/productReturnInfos/productReturnInfo.module').then(m => m.ProductReturnInfoModule),
                        data: { permission: 'Pages.ProductReturnInfos' }
                    },
                
                    
                    {
                        path: 'lookupData/returnStatuses',
                        loadChildren: () => import('./lookupData/returnStatuses/returnStatus.module').then(m => m.ReturnStatusModule),
                        data: { permission: 'Pages.ReturnStatuses' }
                    },
                
                    
                    {
                        path: 'lookupData/returnTypes',
                        loadChildren: () => import('./lookupData/returnTypes/returnType.module').then(m => m.ReturnTypeModule),
                        data: { permission: 'Pages.ReturnTypes' }
                    },
                
                    
                    {
                        path: 'shop/productUpsellRelatedProducts',
                        loadChildren: () => import('./shop/productUpsellRelatedProducts/productUpsellRelatedProduct.module').then(m => m.ProductUpsellRelatedProductModule),
                        data: { permission: 'Pages.ProductUpsellRelatedProducts' }
                    },
                
                    
                    {
                        path: 'shop/productCrossSellProducts',
                        loadChildren: () => import('./shop/productCrossSellProducts/productCrossSellProduct.module').then(m => m.ProductCrossSellProductModule),
                        data: { permission: 'Pages.ProductCrossSellProducts' }
                    },
                
                    
                    {
                        path: 'shop/productAccountTeams',
                        loadChildren: () => import('./shop/productAccountTeams/productAccountTeam.module').then(m => m.ProductAccountTeamModule),
                        data: { permission: 'Pages.ProductAccountTeams' }
                    },
                
                    
                    {
                        path: 'lookupData/subscriptionTypes',
                        loadChildren: () => import('./lookupData/subscriptionTypes/subscriptionType.module').then(m => m.SubscriptionTypeModule),
                        data: { permission: 'Pages.SubscriptionTypes' }
                    },
                
                    
                    {
                        path: 'shop/productSubscriptionMaps',
                        loadChildren: () => import('./shop/productSubscriptionMaps/productSubscriptionMap.module').then(m => m.ProductSubscriptionMapModule),
                        data: { permission: 'Pages.ProductSubscriptionMaps' }
                    },
                
                    
                    {
                        path: 'lookupData/socialMedias',
                        loadChildren: () => import('./lookupData/socialMedias/socialMedia.module').then(m => m.SocialMediaModule),
                        data: { permission: 'Pages.SocialMedias' }
                    },
                
                    
                    {
                        path: 'shop/productCustomerStats',
                        loadChildren: () => import('./shop/productCustomerStats/productCustomerStat.module').then(m => m.ProductCustomerStatModule),
                        data: { permission: 'Pages.ProductCustomerStats' }
                    },
                
                    
                    {
                        path: 'shop/productFaqs',
                        loadChildren: () => import('./shop/productFaqs/productFaq.module').then(m => m.ProductFaqModule),
                        data: { permission: 'Pages.ProductFaqs' }
                    },
                
                    
                    {
                        path: 'shop/productCustomerQueries',
                        loadChildren: () => import('./shop/productCustomerQueries/productCustomerQuery.module').then(m => m.ProductCustomerQueryModule),
                        data: { permission: 'Pages.ProductCustomerQueries' }
                    },
                
                    
                    {
                        path: 'shop/productPackages',
                        loadChildren: () => import('./shop/productPackages/productPackage.module').then(m => m.ProductPackageModule),
                        data: { permission: 'Pages.ProductPackages' }
                    },
                
                    
                    {
                        path: 'shop/productNotes',
                        loadChildren: () => import('./shop/productNotes/productNote.module').then(m => m.ProductNoteModule),
                        data: { permission: 'Pages.ProductNotes' }
                    },
                
                    
                    {
                        path: 'shop/productMedias',
                        loadChildren: () => import('./shop/productMedias/productMedia.module').then(m => m.ProductMediaModule),
                        data: { permission: 'Pages.ProductMedias' }
                    },
                
                    
                    {
                        path: 'shop/productCategoryTeams',
                        loadChildren: () => import('./shop/productCategoryTeams/productCategoryTeam.module').then(m => m.ProductCategoryTeamModule),
                        data: { permission: 'Pages.ProductCategoryTeams' }
                    },
                
                    
                    {
                        path: 'shop/productCategoryMaps',
                        loadChildren: () => import('./shop/productCategoryMaps/productCategoryMap.module').then(m => m.ProductCategoryMapModule),
                        data: { permission: 'Pages.ProductCategoryMaps' }
                    },
                
                    
                    {
                        path: 'shop/productVariants',
                        loadChildren: () => import('./shop/productVariants/productVariant.module').then(m => m.ProductVariantModule),
                        data: { permission: 'Pages.ProductVariants' }
                    },
                
                    
                    {
                        path: 'shop/productVariantCategories',
                        loadChildren: () => import('./shop/productVariantCategories/productVariantCategory.module').then(m => m.ProductVariantCategoryModule),
                        data: { permission: 'Pages.ProductVariantCategories' }
                    },
                
                    
                    {
                        path: 'territory/hubNavigationMenus',
                        loadChildren: () => import('./territory/hubNavigationMenus/hubNavigationMenu.module').then(m => m.HubNavigationMenuModule),
                        data: { permission: 'Pages.HubNavigationMenus' }
                    },
                
                    
                    {
                        path: 'territory/masterNavigationMenus',
                        loadChildren: () => import('./territory/masterNavigationMenus/masterNavigationMenu.module').then(m => m.MasterNavigationMenuModule),
                        data: { permission: 'Pages.MasterNavigationMenus' }
                    },
                
                    
                    {
                        path: 'territory/hubAccountTeams',
                        loadChildren: () => import('./territory/hubAccountTeams/hubAccountTeam.module').then(m => m.HubAccountTeamModule),
                        data: { permission: 'Pages.HubAccountTeams' }
                    },
                
                    
                    {
                        path: 'territory/hubSalesProjections',
                        loadChildren: () => import('./territory/hubSalesProjections/hubSalesProjection.module').then(m => m.HubSalesProjectionModule),
                        data: { permission: 'Pages.HubSalesProjections' }
                    },
                
                    
                    {
                        path: 'territory/hubZipCodeMaps',
                        loadChildren: () => import('./territory/hubZipCodeMaps/hubZipCodeMap.module').then(m => m.HubZipCodeMapModule),
                        data: { permission: 'Pages.HubZipCodeMaps' }
                    },
                
                    
                    {
                        path: 'territory/hubContacts',
                        loadChildren: () => import('./territory/hubContacts/hubContact.module').then(m => m.HubContactModule),
                        data: { permission: 'Pages.HubContacts' }
                    },
                
                    
                    {
                        path: 'territory/hubBusinesses',
                        loadChildren: () => import('./territory/hubBusinesses/hubBusiness.module').then(m => m.HubBusinessModule),
                        data: { permission: 'Pages.HubBusinesses' }
                    },
                
                    
                    {
                        path: 'territory/hubStores',
                        loadChildren: () => import('./territory/hubStores/hubStore.module').then(m => m.HubStoreModule),
                        data: { permission: 'Pages.HubStores' }
                    },
                
                    
                    {
                        path: 'territory/hubProducts',
                        loadChildren: () => import('./territory/hubProducts/hubProduct.module').then(m => m.HubProductModule),
                        data: { permission: 'Pages.HubProducts' }
                    },
                
                    
                    {
                        path: 'territory/hubProductCategories',
                        loadChildren: () => import('./territory/hubProductCategories/hubProductCategory.module').then(m => m.HubProductCategoryModule),
                        data: { permission: 'Pages.HubProductCategories' }
                    },
                
                    
                    {
                        path: 'salesLeadManagement/leadReferralRewards',
                        loadChildren: () => import('./salesLeadManagement/leadReferralRewards/leadReferralReward.module').then(m => m.LeadReferralRewardModule),
                        data: { permission: 'Pages.LeadReferralRewards' }
                    },
                
                    
                    {
                        path: 'salesLeadManagement/leadContacts',
                        loadChildren: () => import('./salesLeadManagement/leadContacts/leadContact.module').then(m => m.LeadContactModule),
                        data: { permission: 'Pages.LeadContacts' }
                    },
                
                    
                    {
                        path: 'salesLeadManagement/leadNotes',
                        loadChildren: () => import('./salesLeadManagement/leadNotes/leadNote.module').then(m => m.LeadNoteModule),
                        data: { permission: 'Pages.LeadNotes' }
                    },
                
                    
                    {
                        path: 'salesLeadManagement/leadTasks',
                        loadChildren: () => import('./salesLeadManagement/leadTasks/leadTask.module').then(m => m.LeadTaskModule),
                        data: { permission: 'Pages.LeadTasks' }
                    },
                
                    
                    {
                        path: 'salesLeadManagement/leadTags',
                        loadChildren: () => import('./salesLeadManagement/leadTags/leadTag.module').then(m => m.LeadTagModule),
                        data: { permission: 'Pages.LeadTags' }
                    },
                
                    
                    {
                        path: 'salesLeadManagement/leadSalesTeams',
                        loadChildren: () => import('./salesLeadManagement/leadSalesTeams/leadSalesTeam.module').then(m => m.LeadSalesTeamModule),
                        data: { permission: 'Pages.LeadSalesTeams' }
                    },
                
                    
                    {
                        path: 'salesLeadManagement/leadPipelineStatuses',
                        loadChildren: () => import('./salesLeadManagement/leadPipelineStatuses/leadPipelineStatus.module').then(m => m.LeadPipelineStatusModule),
                        data: { permission: 'Pages.LeadPipelineStatuses' }
                    },
                
                    
                    {
                        path: 'salesLeadManagement/leads',
                        loadChildren: () => import('./salesLeadManagement/leads/lead.module').then(m => m.LeadModule),
                        data: { permission: 'Pages.Leads' }
                    },
                
                    
                    {
                        path: 'salesLeadManagement/leadPipelineStages',
                        loadChildren: () => import('./salesLeadManagement/leadPipelineStages/leadPipelineStage.module').then(m => m.LeadPipelineStageModule),
                        data: { permission: 'Pages.LeadPipelineStages' }
                    },
                
                    
                    {
                        path: 'salesLeadManagement/leadSources',
                        loadChildren: () => import('./salesLeadManagement/leadSources/leadSource.module').then(m => m.LeadSourceModule),
                        data: { permission: 'Pages.LeadSources' }
                    },
                
                    
                    {
                        path: 'shop/storeZipCodeMaps',
                        loadChildren: () => import('./shop/storeZipCodeMaps/storeZipCodeMap.module').then(m => m.StoreZipCodeMapModule),
                        data: { permission: 'Pages.StoreZipCodeMaps' }
                    },
                
                    
                    {
                        path: 'shop/storeReviewFeedbacks',
                        loadChildren: () => import('./shop/storeReviewFeedbacks/storeReviewFeedback.module').then(m => m.StoreReviewFeedbackModule),
                        data: { permission: 'Pages.StoreReviewFeedbacks' }
                    },
                
                    
                    {
                        path: 'shop/storeReviews',
                        loadChildren: () => import('./shop/storeReviews/storeReview.module').then(m => m.StoreReviewModule),
                        data: { permission: 'Pages.StoreReviews' }
                    },
                
                    
                    {
                        path: 'shop/storeRelevantStores',
                        loadChildren: () => import('./shop/storeRelevantStores/storeRelevantStore.module').then(m => m.StoreRelevantStoreModule),
                        data: { permission: 'Pages.StoreRelevantStores' }
                    },
                
                    
                    {
                        path: 'shop/storeMarketplaceCommissionSettings',
                        loadChildren: () => import('./shop/storeMarketplaceCommissionSettings/storeMarketplaceCommissionSetting.module').then(m => m.StoreMarketplaceCommissionSettingModule),
                        data: { permission: 'Pages.StoreMarketplaceCommissionSettings' }
                    },
                
                    
                    {
                        path: 'shop/marketplaceCommissionTypes',
                        loadChildren: () => import('./shop/marketplaceCommissionTypes/marketplaceCommissionType.module').then(m => m.MarketplaceCommissionTypeModule),
                        data: { permission: 'Pages.MarketplaceCommissionTypes' }
                    },
                
                    
                    {
                        path: 'shop/storeSalesAlerts',
                        loadChildren: () => import('./shop/storeSalesAlerts/storeSalesAlert.module').then(m => m.StoreSalesAlertModule),
                        data: { permission: 'Pages.StoreSalesAlerts' }
                    },
                
                    
                    {
                        path: 'shop/storeTaxes',
                        loadChildren: () => import('./shop/storeTaxes/storeTax.module').then(m => m.StoreTaxModule),
                        data: { permission: 'Pages.StoreTaxes' }
                    },
                
                    
                    {
                        path: 'shop/storeBankAccounts',
                        loadChildren: () => import('./shop/storeBankAccounts/storeBankAccount.module').then(m => m.StoreBankAccountModule),
                        data: { permission: 'Pages.StoreBankAccounts' }
                    },
                
                    
                    {
                        path: 'shop/storeNotes',
                        loadChildren: () => import('./shop/storeNotes/storeNote.module').then(m => m.StoreNoteModule),
                        data: { permission: 'Pages.StoreNotes' }
                    },
                
                    
                    {
                        path: 'shop/storeMedias',
                        loadChildren: () => import('./shop/storeMedias/storeMedia.module').then(m => m.StoreMediaModule),
                        data: { permission: 'Pages.StoreMedias' }
                    },
                
                    
                    {
                        path: 'shop/storeLocations',
                        loadChildren: () => import('./shop/storeLocations/storeLocation.module').then(m => m.StoreLocationModule),
                        data: { permission: 'Pages.StoreLocations' }
                    },
                
                    
                    {
                        path: 'shop/storeBusinessHours',
                        loadChildren: () => import('./shop/storeBusinessHours/storeBusinessHour.module').then(m => m.StoreBusinessHourModule),
                        data: { permission: 'Pages.StoreBusinessHours' }
                    },
                
                    
                    {
                        path: 'shop/storeContactMaps',
                        loadChildren: () => import('./shop/storeContactMaps/storeContactMap.module').then(m => m.StoreContactMapModule),
                        data: { permission: 'Pages.StoreContactMaps' }
                    },
                
                    
                    {
                        path: 'shop/storeBusinessCustomerMaps',
                        loadChildren: () => import('./shop/storeBusinessCustomerMaps/storeBusinessCustomerMap.module').then(m => m.StoreBusinessCustomerMapModule),
                        data: { permission: 'Pages.StoreBusinessCustomerMaps' }
                    },
                
                    
                    {
                        path: 'shop/storeOwnerTeams',
                        loadChildren: () => import('./shop/storeOwnerTeams/storeOwnerTeam.module').then(m => m.StoreOwnerTeamModule),
                        data: { permission: 'Pages.StoreOwnerTeams' }
                    },
                
                    
                    {
                        path: 'shop/storeAccountTeams',
                        loadChildren: () => import('./shop/storeAccountTeams/storeAccountTeam.module').then(m => m.StoreAccountTeamModule),
                        data: { permission: 'Pages.StoreAccountTeams' }
                    },
                
                    
                    {
                        path: 'shop/storeProductMaps',
                        loadChildren: () => import('./shop/storeProductMaps/storeProductMap.module').then(m => m.StoreProductMapModule),
                        data: { permission: 'Pages.StoreProductMaps' }
                    },
                
                    
                    {
                        path: 'shop/storeProductCategoryMaps',
                        loadChildren: () => import('./shop/storeProductCategoryMaps/storeProductCategoryMap.module').then(m => m.StoreProductCategoryMapModule),
                        data: { permission: 'Pages.StoreProductCategoryMaps' }
                    },
                
                    
                    {
                        path: 'crm/employeeTags',
                        loadChildren: () => import('./crm/employeeTags/employeeTag.module').then(m => m.EmployeeTagModule),
                        data: { permission: 'Pages.EmployeeTags' }
                    },
                
                    
                    {
                        path: 'crm/businessTaskMaps',
                        loadChildren: () => import('./crm/businessTaskMaps/businessTaskMap.module').then(m => m.BusinessTaskMapModule),
                        data: { permission: 'Pages.BusinessTaskMaps' }
                    },
                
                    
                    {
                        path: 'crm/businessProductMaps',
                        loadChildren: () => import('./crm/businessProductMaps/businessProductMap.module').then(m => m.BusinessProductMapModule),
                        data: { permission: 'Pages.BusinessProductMaps' }
                    },
                
                    
                    {
                        path: 'crm/businessStoreMaps',
                        loadChildren: () => import('./crm/businessStoreMaps/businessStoreMap.module').then(m => m.BusinessStoreMapModule),
                        data: { permission: 'Pages.BusinessStoreMaps' }
                    },
                
                    
                    {
                        path: 'crm/businessJobMaps',
                        loadChildren: () => import('./crm/businessJobMaps/businessJobMap.module').then(m => m.BusinessJobMapModule),
                        data: { permission: 'Pages.BusinessJobMaps' }
                    },
                
                    
                    {
                        path: 'crm/businessNotes',
                        loadChildren: () => import('./crm/businessNotes/businessNote.module').then(m => m.BusinessNoteModule),
                        data: { permission: 'Pages.BusinessNotes' }
                    },
                
                    
                    {
                        path: 'crm/businessUsers',
                        loadChildren: () => import('./crm/businessUsers/businessUser.module').then(m => m.BusinessUserModule),
                        data: { permission: 'Pages.BusinessUsers' }
                    },
                
                    
                    {
                        path: 'crm/businessAccountTeams',
                        loadChildren: () => import('./crm/businessAccountTeams/businessAccountTeam.module').then(m => m.BusinessAccountTeamModule),
                        data: { permission: 'Pages.BusinessAccountTeams' }
                    },
                
                    
                    {
                        path: 'crm/businessContactMaps',
                        loadChildren: () => import('./crm/businessContactMaps/businessContactMap.module').then(m => m.BusinessContactMapModule),
                        data: { permission: 'Pages.BusinessContactMaps' }
                    },
                
                    
                    {
                        path: 'taskManagement/taskTags',
                        loadChildren: () => import('./taskManagement/taskTags/taskTag.module').then(m => m.TaskTagModule),
                        data: { permission: 'Pages.TaskTags' }
                    },
                
                    
                    {
                        path: 'taskManagement/taskEvents',
                        loadChildren: () => import('./taskManagement/taskEvents/taskEvent.module').then(m => m.TaskEventModule),
                        data: { permission: 'Pages.TaskEvents' }
                    },

                    {
                        path: 'taskManagement/taskLibrary',
                        data: { permission: 'Pages.TaskEvents' },
                        component: TaskLibrariesComponent,
                    },
                
                    
                    {
                        path: 'taskManagement/taskStatuses',
                        loadChildren: () => import('./taskManagement/taskStatuses/taskStatus.module').then(m => m.TaskStatusModule),
                        data: { permission: 'Pages.TaskStatuses' }
                    },
                
                    
                    {
                        path: 'crm/employees',
                        loadChildren: () => import('./crm/employees/employee.module').then(m => m.EmployeeModule),
                        data: { permission: 'Pages.Employees' }
                    },

                    {
                        path: 'crm/myEmployees',
                        data: { permission: 'Pages.Employees' },
                        component: MyEmployeesComponent,
                    },
                
                    
                    {
                        path: 'jobManagement/jobTags',
                        loadChildren: () => import('./jobManagement/jobTags/jobTag.module').then(m => m.JobTagModule),
                        data: { permission: 'Pages.JobTags' }
                    },
                
                    
                    {
                        path: 'jobManagement/jobs',
                        loadChildren: () => import('./jobManagement/jobs/job.module').then(m => m.JobModule),
                        data: { permission: 'Pages.Jobs' }
                    },
                
                    
                    {
                        path: 'jobManagement/jobStatusTypes',
                        loadChildren: () => import('./jobManagement/jobStatusTypes/jobStatusType.module').then(m => m.JobStatusTypeModule),
                        data: { permission: 'Pages.JobStatusTypes' }
                    },
                
                    
                    {
                        path: 'shop/storeTags',
                        loadChildren: () => import('./shop/storeTags/storeTag.module').then(m => m.StoreTagModule),
                        data: { permission: 'Pages.StoreTags' }
                    },
                
                    
                    {
                        path: 'shop/productTags',
                        loadChildren: () => import('./shop/productTags/productTag.module').then(m => m.ProductTagModule),
                        data: { permission: 'Pages.ProductTags' }
                    },
                
                    
                    {
                        path: 'crm/businessTags',
                        loadChildren: () => import('./crm/businessTags/businessTag.module').then(m => m.BusinessTagModule),
                        data: { permission: 'Pages.BusinessTags' }
                    },
                
                    
                    {
                        path: 'crm/contactTags',
                        loadChildren: () => import('./crm/contactTags/contactTag.module').then(m => m.ContactTagModule),
                        data: { permission: 'Pages.ContactTags' }
                    },
                
                    
                    {
                        path: 'shop/stores',
                        loadChildren: () => import('./shop/stores/store.module').then(m => m.StoreModule),
                        data: { permission: 'Pages.Stores' }
                    },

                    {
                        path: 'shop/myStores',
                        data: { permission: 'Pages.Stores' },
                        component: MyStoresComponent,
                    },
                
                    
                    {
                        path: 'shop/products',
                        loadChildren: () => import('./shop/products/product.module').then(m => m.ProductModule),
                        data: { permission: 'Pages.Products' }
                    },
                    
                    {
                        path: 'shop/productLibraries',
                        data: { permission: 'Pages.Products' },
                        component: ProductLibrariesComponent,
                    },
                    
                    {
                        path: 'shop/productCategories',
                        loadChildren: () => import('./shop/productCategories/productCategory.module').then(m => m.ProductCategoryModule),
                        data: { permission: 'Pages.ProductCategories' }
                    },
                
                    
                    {
                        path: 'lookupData/mediaLibraries',
                        loadChildren: () => import('./lookupData/mediaLibraries/mediaLibrary.module').then(m => m.MediaLibraryModule),
                        data: { permission: 'Pages.MediaLibraries' }
                    },
                
                    
                    {
                        path: 'crm/businesses',
                        loadChildren: () => import('./crm/businesses/business.module').then(m => m.BusinessModule),
                        data: { permission: 'Pages.Businesses' }
                    },
                
                    
                    {
                        path: 'crm/contacts',
                        loadChildren: () => import('./crm/contacts/contact.module').then(m => m.ContactModule),
                        data: { permission: 'Pages.Contacts' }
                    },
                
                    
                    {
                        path: 'territory/hubs',
                        loadChildren: () => import('./territory/hubs/hub.module').then(m => m.HubModule),
                        data: { permission: 'Pages.Hubs' }
                    },

                    {
                        path: 'territory/myHubs',
                        data: { permission: 'Pages.Hubs' },
                        component: MyHubListComponent,
                    },

                    
                
                    
                    {
                        path: 'lookupData/hubTypes',
                        loadChildren: () => import('./lookupData/hubTypes/hubType.module').then(m => m.HubTypeModule),
                        data: { permission: 'Pages.HubTypes' }
                    },
                
                    
                    {
                        path: 'lookupData/membershipTypes',
                        loadChildren: () => import('./lookupData/membershipTypes/membershipType.module').then(m => m.MembershipTypeModule),
                        data: { permission: 'Pages.MembershipTypes' }
                    },
                
                    
                    {
                        path: 'lookupData/contractTypes',
                        loadChildren: () => import('./lookupData/contractTypes/contractType.module').then(m => m.ContractTypeModule),
                        data: { permission: 'Pages.ContractTypes' }
                    },
                
                    
                    {
                        path: 'lookupData/documentTypes',
                        loadChildren: () => import('./lookupData/documentTypes/documentType.module').then(m => m.DocumentTypeModule),
                        data: { permission: 'Pages.DocumentTypes' }
                    },
                
                    
                    {
                        path: 'lookupData/smsTemplates',
                        loadChildren: () => import('./lookupData/smsTemplates/smsTemplate.module').then(m => m.SmsTemplateModule),
                        data: { permission: 'Pages.SmsTemplates' }
                    },
                
                    
                    {
                        path: 'lookupData/emailTemplates',
                        loadChildren: () => import('./lookupData/emailTemplates/emailTemplate.module').then(m => m.EmailTemplateModule),
                        data: { permission: 'Pages.EmailTemplates' }
                    },
                
                    
                    {
                        path: 'lookupData/connectChannels',
                        loadChildren: () => import('./lookupData/connectChannels/connectChannel.module').then(m => m.ConnectChannelModule),
                        data: { permission: 'Pages.ConnectChannels' }
                    },
                
                    
                    {
                        path: 'lookupData/zipCodes',
                        loadChildren: () => import('./lookupData/zipCodes/zipCode.module').then(m => m.ZipCodeModule),
                        data: { permission: 'Pages.ZipCodes' }
                    },
                
                    
                    {
                        path: 'lookupData/ratingLikes',
                        loadChildren: () => import('./lookupData/ratingLikes/ratingLike.module').then(m => m.RatingLikeModule),
                        data: { permission: 'Pages.RatingLikes' }
                    },
                
                    
                    {
                        path: 'lookupData/measurementUnits',
                        loadChildren: () => import('./lookupData/measurementUnits/measurementUnit.module').then(m => m.MeasurementUnitModule),
                        data: { permission: 'Pages.MeasurementUnits' }
                    },
                
                    
                    {
                        path: 'lookupData/masterTags',
                        loadChildren: () => import('./lookupData/masterTags/masterTag.module').then(m => m.MasterTagModule),
                        data: { permission: 'Pages.MasterTags' }
                    },
                
                    
                    {
                        path: 'lookupData/masterTagCategories',
                        loadChildren: () => import('./lookupData/masterTagCategories/masterTagCategory.module').then(m => m.MasterTagCategoryModule),
                        data: { permission: 'Pages.MasterTagCategories' }
                    },
                
                    
                    {
                        path: 'lookupData/cities',
                        loadChildren: () => import('./lookupData/cities/city.module').then(m => m.CityModule),
                        data: { permission: 'Pages.Cities' }
                    },
                
                    
                    {
                        path: 'lookupData/counties',
                        loadChildren: () => import('./lookupData/counties/county.module').then(m => m.CountyModule),
                        data: { permission: 'Pages.Counties' }
                    },
                
                    
                    {
                        path: 'lookupData/states',
                        loadChildren: () => import('./lookupData/states/state.module').then(m => m.StateModule),
                        data: { permission: 'Pages.States' }
                    },
                
                    
                    {
                        path: 'lookupData/countries',
                        loadChildren: () => import('./lookupData/countries/country.module').then(m => m.CountryModule),
                        data: { permission: 'Pages.Countries' }
                    },
                
                    
                    {
                        path: 'lookupData/currencies',
                        loadChildren: () => import('./lookupData/currencies/currency.module').then(m => m.CurrencyModule),
                        data: { permission: 'Pages.Currencies' }
                    },
                
                    {
                        path: 'dashboard',
                        loadChildren: () => import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
                        data: { permission: 'Pages.Tenant.Dashboard' },
                    },
                    {
                        path: 'lookupData/masterDataIndex',
                        component: MasterDataIndexComponent,
                    },
                    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
                    { path: '**', redirectTo: 'dashboard' },
                ],
            },
        ]),
    ],
    exports: [RouterModule],
})
export class MainRoutingModule {}
