import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MasterDataIndexComponent } from './lookupData/master-data-index/master-data-index.component';
import { MyHubListComponent } from './territory/hubs/my-hub-list/my-hub-list.component';
import { MyStoresComponent } from './shop/stores/my-stores/my-stores.component';
import { components } from '@metronic/app/kt';
import { MyEmployeesComponent } from './crm/employees/my-employees/my-employees.component';
import { ProductLibrariesComponent } from './shop/products/product-libraries/product-libraries.component';
import { TaskLibrariesComponent } from './taskManagement/task-libraries/task-libraries.component';
import { MyTaskEventsComponent } from './taskManagement/taskEvents/my-task-events/my-task-events.component';
import { MyOrdersComponent } from './orderManagement/orders/my-orders/my-orders.component';
import { AbandonedCartComponent } from './orderManagement/orders/abandoned-cart/abandoned-cart.component';
import { SiteDefaultContentComponent } from './cms/contents/site-default-content/site-default-content.component';
import { MyProductsComponent } from './shop/products/my-products/my-products.component';
import { TaskEventsLibraryComponent } from './taskManagement/taskEvents/task-events-library/task-events-library.component';
import { TimesheetsComponent } from './taskManagement/timesheets/timesheets.component';
import { ProjectsComponent } from './jobManagement/jobs/projects/projects.component';
import { ProjectDashboardComponent } from './jobManagement/jobs/projects/project-dashboard/project-dashboard.component';
import { OrderDeliveryCaptainListComponent } from './crm/employees/order-delivery-captain-list/order-delivery-captain-list.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [

                    {
                        path: 'widgetManagement/storeThemeMaps',
                        loadChildren: () => import('./widgetManagement/storeThemeMaps/storeThemeMap.module').then(m => m.StoreThemeMapModule),
                        data: { permission: 'Pages.StoreThemeMaps' }
                    },


                    {
                        path: 'widgetManagement/storeMasterThemes',
                        loadChildren: () => import('./widgetManagement/storeMasterThemes/storeMasterTheme.module').then(m => m.StoreMasterThemeModule),
                        data: { permission: 'Pages.StoreMasterThemes' }
                    },


                    {
                        path: 'widgetManagement/storeWidgetContentMaps',
                        loadChildren: () => import('./widgetManagement/storeWidgetContentMaps/storeWidgetContentMap.module').then(m => m.StoreWidgetContentMapModule),
                        data: { permission: 'Pages.StoreWidgetContentMaps' }
                    },


                    {
                        path: 'widgetManagement/storeWidgetProductMaps',
                        loadChildren: () => import('./widgetManagement/storeWidgetProductMaps/storeWidgetProductMap.module').then(m => m.StoreWidgetProductMapModule),
                        data: { permission: 'Pages.StoreWidgetProductMaps' }
                    },


                    {
                        path: 'widgetManagement/storeWidgetMaps',
                        loadChildren: () => import('./widgetManagement/storeWidgetMaps/storeWidgetMap.module').then(m => m.StoreWidgetMapModule),
                        data: { permission: 'Pages.StoreWidgetMaps' }
                    },


                    {
                        path: 'widgetManagement/hubWidgetContentMaps',
                        loadChildren: () => import('./widgetManagement/hubWidgetContentMaps/hubWidgetContentMap.module').then(m => m.HubWidgetContentMapModule),
                        data: { permission: 'Pages.HubWidgetContentMaps' }
                    },


                    {
                        path: 'widgetManagement/hubWidgetProductMaps',
                        loadChildren: () => import('./widgetManagement/hubWidgetProductMaps/hubWidgetProductMap.module').then(m => m.HubWidgetProductMapModule),
                        data: { permission: 'Pages.HubWidgetProductMaps' }
                    },


                    {
                        path: 'widgetManagement/hubWidgetStoreMaps',
                        loadChildren: () => import('./widgetManagement/hubWidgetStoreMaps/hubWidgetStoreMap.module').then(m => m.HubWidgetStoreMapModule),
                        data: { permission: 'Pages.HubWidgetStoreMaps' }
                    },


                    {
                        path: 'widgetManagement/hubWidgetMaps',
                        loadChildren: () => import('./widgetManagement/hubWidgetMaps/hubWidgetMap.module').then(m => m.HubWidgetMapModule),
                        data: { permission: 'Pages.HubWidgetMaps' }
                    },


                    {
                        path: 'widgetManagement/masterWidgets',
                        loadChildren: () => import('./widgetManagement/masterWidgets/masterWidget.module').then(m => m.MasterWidgetModule),
                        data: { permission: 'Pages.MasterWidgets' }
                    },


                    {
                        path: 'taskManagement/taskWorkItems',
                        loadChildren: () => import('./taskManagement/taskWorkItems/taskWorkItem.module').then(m => m.TaskWorkItemModule),
                        data: { permission: 'Pages.TaskWorkItems' }
                    },


                    {
                        path: 'taskManagement/taskTeams',
                        loadChildren: () => import('./taskManagement/taskTeams/taskTeam.module').then(m => m.TaskTeamModule),
                        data: { permission: 'Pages.TaskTeams' }
                    },


                    {
                        path: 'jobManagement/jobTaskMaps',
                        loadChildren: () => import('./jobManagement/jobTaskMaps/jobTaskMap.module').then(m => m.JobTaskMapModule),
                        data: { permission: 'Pages.JobTaskMaps' }
                    },


                    {
                        path: 'shop/productGigWorkerPortfolios',
                        loadChildren: () => import('./shop/productGigWorkerPortfolios/productGigWorkerPortfolio.module').then(m => m.ProductGigWorkerPortfolioModule),
                        data: { permission: 'Pages.ProductGigWorkerPortfolios' }
                    },

                    {
                        path: 'shop/storeProductServiceLocalityMaps',
                        loadChildren: () => import('./shop/storeProductServiceLocalityMaps/storeProductServiceLocalityMap.module').then(m => m.StoreProductServiceLocalityMapModule),
                        data: { permission: 'Pages.StoreProductServiceLocalityMaps' }
                    },

                    {
                        path: 'shop/productTaskMaps',
                        loadChildren: () => import('./shop/productTaskMaps/productTaskMap.module').then(m => m.ProductTaskMapModule),
                        data: { permission: 'Pages.ProductTaskMaps' }
                    },


                    {
                        path: 'crm/contactTaskMaps',
                        loadChildren: () => import('./crm/contactTaskMaps/contactTaskMap.module').then(m => m.ContactTaskMapModule),
                        data: { permission: 'Pages.ContactTaskMaps' }
                    },
                    {
                        path: 'jobManagement/ApplicantsList',
                        loadChildren: () => import('./jobManagement/applicantList/applicantList.module').then(m => m.ApplicantListModule),
                        data: { permission: 'Pages.JobMasterTagSettings' }
                    },


                    {
                        path: 'jobManagement/jobMasterTagSettings',
                        loadChildren: () => import('./jobManagement/jobMasterTagSettings/jobMasterTagSetting.module').then(m => m.JobMasterTagSettingModule),
                        data: { permission: 'Pages.JobMasterTagSettings' }
                    },

                    {
                        path: 'crm/businessMasterTagSettings',
                        loadChildren: () => import('./crm/businessMasterTagSettings/businessMasterTagSetting.module').then(m => m.BusinessMasterTagSettingModule),
                        data: { permission: 'Pages.BusinessMasterTagSettings' }
                    },


                    {
                        path: 'crm/contactMasterTagSettings',
                        loadChildren: () => import('./crm/contactMasterTagSettings/contactMasterTagSetting.module').then(m => m.ContactMasterTagSettingModule),
                        data: { permission: 'Pages.ContactMasterTagSettings' }
                    },


                    {
                        path: 'shop/productMasterTagSettings',
                        loadChildren: () => import('./shop/productMasterTagSettings/productMasterTagSetting.module').then(m => m.ProductMasterTagSettingModule),
                        data: { permission: 'Pages.ProductMasterTagSettings' }
                    },


                    {
                        path: 'shop/storeMasterTagSettings',
                        loadChildren: () => import('./shop/storeMasterTagSettings/storeMasterTagSetting.module').then(m => m.StoreMasterTagSettingModule),
                        data: { permission: 'Pages.StoreMasterTagSettings' }
                    },


                    {
                        path: 'shop/storeTagSettingCategories',
                        loadChildren: () => import('./shop/storeTagSettingCategories/storeTagSettingCategory.module').then(m => m.StoreTagSettingCategoryModule),
                        data: { permission: 'Pages.StoreTagSettingCategories' }
                    },


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
                        path: 'cms/storeProductBlogs',
                        loadChildren: () => import('./cms/storeProductBlogs/storeProductBlog.module').then(m => m.StoreProductBlogModule),
                        data: { permission: 'Pages.StoreProductBlogs' }
                    },

                    {
                        path: 'cms/siteDefaultContents',
                        component: SiteDefaultContentComponent,
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
                        path: 'orderManagement/deliveryCaptains',
                        component: OrderDeliveryCaptainListComponent,
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
                        path: 'orderManagement/orderDeliveryByCaptains',
                        loadChildren: () => import('./orderManagement/orderDeliveryByCaptains/orderDeliveryByCaptain.module').then(m => m.OrderDeliveryByCaptainModule),
                        data: { permission: 'Pages.OrderDeliveryByCaptains' }
                    },
                    {
                        path: 'orderManagement/orderDeliveryChangeCaptains',
                        loadChildren: () => import('./orderManagement/orderDeliveryChangeCaptains/orderDeliveryChangeCaptain.module').then(m => m.OrderDeliveryChangeCaptainModule),
                        data: { permission: 'Pages.OrderDeliveryChangeCaptains' }
                    },


                    {
                        path: 'lookupData/paymentTypes',
                        loadChildren: () => import('./lookupData/paymentTypes/paymentType.module').then(m => m.PaymentTypeModule),
                        data: { permission: 'Pages.PaymentTypes' }
                    },

                    {
                        path: 'lookupData/deliveryTypes',
                        loadChildren: () => import('./lookupData/deliveryTypes/deliveryType.module').then(m => m.DeliveryTypeModule),
                        data: { permission: 'Pages.DeliveryTypes' }
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
                        path: 'orderManagement/myOrders',
                        component: MyOrdersComponent,
                        data: { permission: 'Pages.Orders' }
                    },

                    {
                        path: 'orderManagement/abandonedCart',
                        component: AbandonedCartComponent,
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
                        path: 'territory/hubJobs',
                        loadChildren: () => import('./territory/hubJobs/hubJob.module').then(m => m.HubJobModule),
                        data: { permission: 'Pages.HubJobs' }
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
                        path: 'taskManagement/myTaskEvents',
                        component: MyTaskEventsComponent,
                        data: { permission: 'Pages.TaskEvents' }
                    },
                    {
                        path: 'taskManagement/timesheets',
                        loadChildren: () => import('./taskManagement/timesheets/timesheet.module').then(m => m.TimesheetModule),
                        data: { permission: 'Pages.TaskEvents' }
                    },
                    {
                        path: 'taskManagement/taskLibrary',
                        data: { permission: 'Pages.TaskEvents' },
                        component: TaskEventsLibraryComponent,
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
                        path: 'ppm/projects',
                        component: ProjectsComponent,
                        data: { permission: 'Pages.Jobs' }
                    },

                    {
                        path: 'ppm/projects/dashboard/:jobId',
                        component: ProjectDashboardComponent,
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
                        path: 'shop/myProducts',
                        data: { permission: 'Pages.Products' },
                        component: MyProductsComponent,
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
                        path: 'crm/connectMessages',
                        loadChildren: () => import('./crm/connectMessages/connectMessage.module').then(m => m.ConnectMessageModule),
                        data: { permission: 'Pages.ConnectMessages' }
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
                        path: 'bookingManagement/bookings',
                        loadChildren: () => import('./bookingManagement/bookings/booking.module').then(m => m.BookingModule),
                        data: { permission: 'Pages.Bookings' }
                    },
                    {
                        path: 'bookingManagement/bookingStatuses',
                        loadChildren: () => import('./bookingManagement/bookingStatuses/bookingStatus.module').then(m => m.BookingStatusModule),
                        data: { permission: 'Pages.BookingStatuses' }
                    },

                    {
                        path: 'bookingManagement/bookingTimeslotSettings',
                        loadChildren: () => import('./bookingManagement/bookingTimeslotSettings/bookingTimeslotSetting.module').then(m => m.BookingTimeslotSettingModule),
                        data: { permission: 'Pages.BookingTimeslotSettings' }
                    },

                    {
                        path: 'bookingManagement/bookingTypes',
                        loadChildren: () => import('./bookingManagement/bookingTypes/bookingType.module').then(m => m.BookingTypeModule),
                        data: { permission: 'Pages.BookingTypes' }
                    },
                    {
                        path: 'contractManagement/contracts',
                        loadChildren: () => import('./contractManagement/contracts/contract.module').then(m => m.ContractModule),
                        data: { permission: 'Pages.Contracts' }
                    },
                    {
                        path: 'contractManagement/contractFinancialTerms',
                        loadChildren: () => import('./contractManagement/contractFinancialTerms/contractFinancialTerm.module').then(m => m.ContractFinancialTermModule),
                        data: { permission: 'Pages.ContractFinancialTerms' }
                    },

                    {
                        path: 'rewardManagement/rewardPointAwardSettings',
                        loadChildren: () => import('./rewardManagement/rewardPointAwardSettings/rewardPointAwardSetting.module').then(m => m.RewardPointAwardSettingModule),
                        data: { permission: 'Pages.RewardPointAwardSettings' }
                    },
                    {
                        path: 'rewardManagement/rewardPointHistories',
                        loadChildren: () => import('./rewardManagement/rewardPointHistories/rewardPointHistory.module').then(m => m.RewardPointHistoryModule),
                        data: { permission: 'Pages.RewardPointHistories' }
                    },
                    {
                        path: 'rewardManagement/rewardPointPriceSettings',
                        loadChildren: () => import('./rewardManagement/rewardPointPriceSettings/rewardPointPriceSetting.module').then(m => m.RewardPointPriceSettingModule),
                        data: { permission: 'Pages.RewardPointPriceSettings' }
                    },
                    {
                        path: 'rewardManagement/rewardPointTypes',
                        loadChildren: () => import('./rewardManagement/rewardPointTypes/rewardPointType.module').then(m => m.RewardPointTypeModule),
                        data: { permission: 'Pages.RewardPointTypes' }
                    },

                    //crm
                    {
                        path: 'crm/contactAddresses',
                        loadChildren: () => import('./crm/contactAddresses/contactAddress.module').then(m => m.ContactAddressModule),
                        data: { permission: 'Pages.ContactAddresses' }
                    },
                    {
                        path: 'crm/contactCertificationLicenses',
                        loadChildren: () => import('./crm/contactCertificationLicenses/contactCertificationLicense.module').then(m => m.ContactCertificationLicenseModule),
                        data: { permission: 'Pages.ContactCertificationLicenses' }
                    },
                    {
                        path: 'crm/contactEducations',
                        loadChildren: () => import('./crm/contactEducations/contactEducation.module').then(m => m.ContactEducationModule),
                        data: { permission: 'Pages.ContactEducations' }
                    },
                    {
                        path: 'crm/contactExperiences',
                        loadChildren: () => import('./crm/contactExperiences/contactExperience.module').then(m => m.ContactExperienceModule),
                        data: { permission: 'Pages.ContactExperiences' }
                    },
                    {
                        path: 'crm/contactMembershipHistories',
                        loadChildren: () => import('./crm/contactMembershipHistories/contactMembershipHistory.module').then(m => m.ContactMembershipHistoryModule),
                        data: { permission: 'Pages.ContactMembershipHistories' }
                    },
                    {
                        path: 'crm/contactOrApplicantTeams',
                        loadChildren: () => import('./crm/contactOrApplicantTeams/contactOrApplicantTeam.module').then(m => m.ContactOrApplicantTeamModule),
                        data: { permission: 'Pages.ContactOrApplicantTeams' }
                    },
                    {
                        path: 'crm/contactProductRecommendations',
                        loadChildren: () => import('./crm/contactProductRecommendations/contactProductRecommendation.module').then(m => m.ContactProductRecommendationModule),
                        data: { permission: 'Pages.ContactProductRecommendations' }
                    },
                    {
                        path: 'crm/contactReferralContacts',
                        loadChildren: () => import('./crm/contactReferralContacts/contactReferralContact.module').then(m => m.ContactReferralContactModule),
                        data: { permission: 'Pages.ContactReferralContacts' }
                    },
                    {
                        path: 'crm/contactVettedInfos',
                        loadChildren: () => import('./crm/contactVettedInfos/contactVettedInfo.module').then(m => m.ContactVettedInfoModule),
                        data: { permission: 'Pages.ContactVettedInfos' }
                    },
                    {
                        path: 'crm/customerLocalitiesZipCodeMaps',
                        loadChildren: () => import('./crm/customerLocalitiesZipCodeMaps/customerLocalitiesZipCodeMap.module').then(m => m.CustomerLocalitiesZipCodeMapModule),
                        data: { permission: 'Pages.CustomerLocalitiesZipCodeMaps' }
                    },
                    {
                        path: 'crm/customerWalletTransactions',
                        loadChildren: () => import('./crm/customerWalletTransactions/customerWalletTransaction.module').then(m => m.CustomerWalletTransactionModule),
                        data: { permission: 'Pages.CustomerWalletTransactions' }
                    },
                    {
                        path: 'crm/employeeTeams',
                        loadChildren: () => import('./crm/employeeTeams/employeeTeam.module').then(m => m.EmployeeTeamModule),
                        data: { permission: 'Pages.EmployeeTeams' }
                    },
                    //job
                    {
                        path: 'jobManagement/jobApplicantHireMatrixes',
                        loadChildren: () => import('./jobManagement/jobApplicantHireMatrixes/jobApplicantHireMatrix.module').then(m => m.JobApplicantHireMatrixModule),
                        data: { permission: 'Pages.JobApplicantHireMatrixes' }
                    },
                    {
                        path: 'jobManagement/jobApplicantHireStatusTypes',
                        loadChildren: () => import('./jobManagement/jobApplicantHireStatusTypes/jobApplicantHireStatusType.module').then(m => m.JobApplicantHireStatusTypeModule),
                        data: { permission: 'Pages.JobApplicantHireStatusTypes' }
                    },
                    {
                        path: 'jobManagement/jobHiringTeams',
                        loadChildren: () => import('./jobManagement/jobHiringTeams/jobHiringTeam.module').then(m => m.JobHiringTeamModule),
                        data: { permission: 'Pages.JobHiringTeams' }
                    },
                    {
                        path: 'jobManagement/jobReferralFeeDetails',
                        loadChildren: () => import('./jobManagement/jobReferralFeeDetails/jobReferralFeeDetail.module').then(m => m.JobReferralFeeDetailModule),
                        data: { permission: 'Pages.JobReferralFeeDetails' }
                    },
                    {
                        path: 'jobManagement/jobReferralFeeSplitPolicies',
                        loadChildren: () => import('./jobManagement/jobReferralFeeSplitPolicies/jobReferralFeeSplitPolicy.module').then(m => m.JobReferralFeeSplitPolicyModule),
                        data: { permission: 'Pages.JobReferralFeeSplitPolicies' }
                    },

                    //task
                    {
                        path: 'taskManagement/employeeTimesheetActivityTrackers',
                        loadChildren: () => import('./taskManagement/employeeTimesheetActivityTrackers/employeeTimesheetActivityTracker.module').then(m => m.EmployeeTimesheetActivityTrackerModule),
                        data: { permission: 'Pages.EmployeeTimesheetActivityTrackers' }
                    },
                    {
                        path: 'taskManagement/employeeTimesheetPolicyMappings',
                        loadChildren: () => import('./taskManagement/employeeTimesheetPolicyMappings/employeeTimesheetPolicyMapping.module').then(m => m.EmployeeTimesheetPolicyMappingModule),
                        data: { permission: 'Pages.EmployeeTimesheetPolicyMappings' }
                    },
                    {
                        path: 'taskManagement/orderTaskMaps',
                        loadChildren: () => import('./taskManagement/orderTaskMaps/orderTaskMap.module').then(m => m.OrderTaskMapModule),
                        data: { permission: 'Pages.OrderTaskMaps' }
                    },
                    {
                        path: 'taskManagement/taskManagerRatings',
                        loadChildren: () => import('./taskManagement/taskManagerRatings/taskManagerRating.module').then(m => m.TaskManagerRatingModule),
                        data: { permission: 'Pages.TaskManagerRatings' }
                    },
                    {
                        path: 'taskManagement/timesheetPolicies',
                        loadChildren: () => import('./taskManagement/timesheetPolicies/timesheetPolicy.module').then(m => m.TimesheetPolicyModule),
                        data: { permission: 'Pages.TimesheetPolicies' }
                    },
                    {
                        path: 'taskManagement/timesheetTaskMaps',
                        loadChildren: () => import('./taskManagement/timesheetTaskMaps/timesheetTaskMap.module').then(m => m.TimesheetTaskMapModule),
                        data: { permission: 'Pages.TimesheetTaskMaps' }
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
