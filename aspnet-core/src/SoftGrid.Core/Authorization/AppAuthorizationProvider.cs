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

            var taskDocuments = pages.CreateChildPermission(AppPermissions.Pages_TaskDocuments, L("TaskDocuments"));
            taskDocuments.CreateChildPermission(AppPermissions.Pages_TaskDocuments_Create, L("CreateNewTaskDocument"));
            taskDocuments.CreateChildPermission(AppPermissions.Pages_TaskDocuments_Edit, L("EditTaskDocument"));
            taskDocuments.CreateChildPermission(AppPermissions.Pages_TaskDocuments_Delete, L("DeleteTaskDocument"));

            var jobDocuments = pages.CreateChildPermission(AppPermissions.Pages_JobDocuments, L("JobDocuments"));
            jobDocuments.CreateChildPermission(AppPermissions.Pages_JobDocuments_Create, L("CreateNewJobDocument"));
            jobDocuments.CreateChildPermission(AppPermissions.Pages_JobDocuments_Edit, L("EditJobDocument"));
            jobDocuments.CreateChildPermission(AppPermissions.Pages_JobDocuments_Delete, L("DeleteJobDocument"));

            var hubDocuments = pages.CreateChildPermission(AppPermissions.Pages_HubDocuments, L("HubDocuments"));
            hubDocuments.CreateChildPermission(AppPermissions.Pages_HubDocuments_Create, L("CreateNewHubDocument"));
            hubDocuments.CreateChildPermission(AppPermissions.Pages_HubDocuments_Edit, L("EditHubDocument"));
            hubDocuments.CreateChildPermission(AppPermissions.Pages_HubDocuments_Delete, L("DeleteHubDocument"));

            var productDocuments = pages.CreateChildPermission(AppPermissions.Pages_ProductDocuments, L("ProductDocuments"));
            productDocuments.CreateChildPermission(AppPermissions.Pages_ProductDocuments_Create, L("CreateNewProductDocument"));
            productDocuments.CreateChildPermission(AppPermissions.Pages_ProductDocuments_Edit, L("EditProductDocument"));
            productDocuments.CreateChildPermission(AppPermissions.Pages_ProductDocuments_Delete, L("DeleteProductDocument"));

            var storeDocuments = pages.CreateChildPermission(AppPermissions.Pages_StoreDocuments, L("StoreDocuments"));
            storeDocuments.CreateChildPermission(AppPermissions.Pages_StoreDocuments_Create, L("CreateNewStoreDocument"));
            storeDocuments.CreateChildPermission(AppPermissions.Pages_StoreDocuments_Edit, L("EditStoreDocument"));
            storeDocuments.CreateChildPermission(AppPermissions.Pages_StoreDocuments_Delete, L("DeleteStoreDocument"));

            var employeeDocuments = pages.CreateChildPermission(AppPermissions.Pages_EmployeeDocuments, L("EmployeeDocuments"));
            employeeDocuments.CreateChildPermission(AppPermissions.Pages_EmployeeDocuments_Create, L("CreateNewEmployeeDocument"));
            employeeDocuments.CreateChildPermission(AppPermissions.Pages_EmployeeDocuments_Edit, L("EditEmployeeDocument"));
            employeeDocuments.CreateChildPermission(AppPermissions.Pages_EmployeeDocuments_Delete, L("DeleteEmployeeDocument"));

            var businessDocuments = pages.CreateChildPermission(AppPermissions.Pages_BusinessDocuments, L("BusinessDocuments"));
            businessDocuments.CreateChildPermission(AppPermissions.Pages_BusinessDocuments_Create, L("CreateNewBusinessDocument"));
            businessDocuments.CreateChildPermission(AppPermissions.Pages_BusinessDocuments_Edit, L("EditBusinessDocument"));
            businessDocuments.CreateChildPermission(AppPermissions.Pages_BusinessDocuments_Delete, L("DeleteBusinessDocument"));

            var contactDocuments = pages.CreateChildPermission(AppPermissions.Pages_ContactDocuments, L("ContactDocuments"));
            contactDocuments.CreateChildPermission(AppPermissions.Pages_ContactDocuments_Create, L("CreateNewContactDocument"));
            contactDocuments.CreateChildPermission(AppPermissions.Pages_ContactDocuments_Edit, L("EditContactDocument"));
            contactDocuments.CreateChildPermission(AppPermissions.Pages_ContactDocuments_Delete, L("DeleteContactDocument"));

            var storeTaskMaps = pages.CreateChildPermission(AppPermissions.Pages_StoreTaskMaps, L("StoreTaskMaps"));
            storeTaskMaps.CreateChildPermission(AppPermissions.Pages_StoreTaskMaps_Create, L("CreateNewStoreTaskMap"));
            storeTaskMaps.CreateChildPermission(AppPermissions.Pages_StoreTaskMaps_Edit, L("EditStoreTaskMap"));
            storeTaskMaps.CreateChildPermission(AppPermissions.Pages_StoreTaskMaps_Delete, L("DeleteStoreTaskMap"));

            var contents = pages.CreateChildPermission(AppPermissions.Pages_Contents, L("Contents"));
            contents.CreateChildPermission(AppPermissions.Pages_Contents_Create, L("CreateNewContent"));
            contents.CreateChildPermission(AppPermissions.Pages_Contents_Edit, L("EditContent"));
            contents.CreateChildPermission(AppPermissions.Pages_Contents_Delete, L("DeleteContent"));

            var productTeams = pages.CreateChildPermission(AppPermissions.Pages_ProductTeams, L("ProductTeams"));
            productTeams.CreateChildPermission(AppPermissions.Pages_ProductTeams_Create, L("CreateNewProductTeam"));
            productTeams.CreateChildPermission(AppPermissions.Pages_ProductTeams_Edit, L("EditProductTeam"));
            productTeams.CreateChildPermission(AppPermissions.Pages_ProductTeams_Delete, L("DeleteProductTeam"));

            var shoppingCarts = pages.CreateChildPermission(AppPermissions.Pages_ShoppingCarts, L("ShoppingCarts"));
            shoppingCarts.CreateChildPermission(AppPermissions.Pages_ShoppingCarts_Create, L("CreateNewShoppingCart"));
            shoppingCarts.CreateChildPermission(AppPermissions.Pages_ShoppingCarts_Edit, L("EditShoppingCart"));
            shoppingCarts.CreateChildPermission(AppPermissions.Pages_ShoppingCarts_Delete, L("DeleteShoppingCart"));

            var wishLists = pages.CreateChildPermission(AppPermissions.Pages_WishLists, L("WishLists"));
            wishLists.CreateChildPermission(AppPermissions.Pages_WishLists_Create, L("CreateNewWishList"));
            wishLists.CreateChildPermission(AppPermissions.Pages_WishLists_Edit, L("EditWishList"));
            wishLists.CreateChildPermission(AppPermissions.Pages_WishLists_Delete, L("DeleteWishList"));

            var discountCodeUserHistories = pages.CreateChildPermission(AppPermissions.Pages_DiscountCodeUserHistories, L("DiscountCodeUserHistories"));
            discountCodeUserHistories.CreateChildPermission(AppPermissions.Pages_DiscountCodeUserHistories_Create, L("CreateNewDiscountCodeUserHistory"));
            discountCodeUserHistories.CreateChildPermission(AppPermissions.Pages_DiscountCodeUserHistories_Edit, L("EditDiscountCodeUserHistory"));
            discountCodeUserHistories.CreateChildPermission(AppPermissions.Pages_DiscountCodeUserHistories_Delete, L("DeleteDiscountCodeUserHistory"));

            var discountCodeMaps = pages.CreateChildPermission(AppPermissions.Pages_DiscountCodeMaps, L("DiscountCodeMaps"));
            discountCodeMaps.CreateChildPermission(AppPermissions.Pages_DiscountCodeMaps_Create, L("CreateNewDiscountCodeMap"));
            discountCodeMaps.CreateChildPermission(AppPermissions.Pages_DiscountCodeMaps_Edit, L("EditDiscountCodeMap"));
            discountCodeMaps.CreateChildPermission(AppPermissions.Pages_DiscountCodeMaps_Delete, L("DeleteDiscountCodeMap"));

            var discountCodeByCustomers = pages.CreateChildPermission(AppPermissions.Pages_DiscountCodeByCustomers, L("DiscountCodeByCustomers"));
            discountCodeByCustomers.CreateChildPermission(AppPermissions.Pages_DiscountCodeByCustomers_Create, L("CreateNewDiscountCodeByCustomer"));
            discountCodeByCustomers.CreateChildPermission(AppPermissions.Pages_DiscountCodeByCustomers_Edit, L("EditDiscountCodeByCustomer"));
            discountCodeByCustomers.CreateChildPermission(AppPermissions.Pages_DiscountCodeByCustomers_Delete, L("DeleteDiscountCodeByCustomer"));

            var discountCodeGenerators = pages.CreateChildPermission(AppPermissions.Pages_DiscountCodeGenerators, L("DiscountCodeGenerators"));
            discountCodeGenerators.CreateChildPermission(AppPermissions.Pages_DiscountCodeGenerators_Create, L("CreateNewDiscountCodeGenerator"));
            discountCodeGenerators.CreateChildPermission(AppPermissions.Pages_DiscountCodeGenerators_Edit, L("EditDiscountCodeGenerator"));
            discountCodeGenerators.CreateChildPermission(AppPermissions.Pages_DiscountCodeGenerators_Delete, L("DeleteDiscountCodeGenerator"));

            var customerWallets = pages.CreateChildPermission(AppPermissions.Pages_CustomerWallets, L("CustomerWallets"));
            customerWallets.CreateChildPermission(AppPermissions.Pages_CustomerWallets_Create, L("CreateNewCustomerWallet"));
            customerWallets.CreateChildPermission(AppPermissions.Pages_CustomerWallets_Edit, L("EditCustomerWallet"));
            customerWallets.CreateChildPermission(AppPermissions.Pages_CustomerWallets_Delete, L("DeleteCustomerWallet"));

            var orderTeams = pages.CreateChildPermission(AppPermissions.Pages_OrderTeams, L("OrderTeams"));
            orderTeams.CreateChildPermission(AppPermissions.Pages_OrderTeams_Create, L("CreateNewOrderTeam"));
            orderTeams.CreateChildPermission(AppPermissions.Pages_OrderTeams_Edit, L("EditOrderTeam"));
            orderTeams.CreateChildPermission(AppPermissions.Pages_OrderTeams_Delete, L("DeleteOrderTeam"));

            var orderProductVariants = pages.CreateChildPermission(AppPermissions.Pages_OrderProductVariants, L("OrderProductVariants"));
            orderProductVariants.CreateChildPermission(AppPermissions.Pages_OrderProductVariants_Create, L("CreateNewOrderProductVariant"));
            orderProductVariants.CreateChildPermission(AppPermissions.Pages_OrderProductVariants_Edit, L("EditOrderProductVariant"));
            orderProductVariants.CreateChildPermission(AppPermissions.Pages_OrderProductVariants_Delete, L("DeleteOrderProductVariant"));

            var orderProductInfos = pages.CreateChildPermission(AppPermissions.Pages_OrderProductInfos, L("OrderProductInfos"));
            orderProductInfos.CreateChildPermission(AppPermissions.Pages_OrderProductInfos_Create, L("CreateNewOrderProductInfo"));
            orderProductInfos.CreateChildPermission(AppPermissions.Pages_OrderProductInfos_Edit, L("EditOrderProductInfo"));
            orderProductInfos.CreateChildPermission(AppPermissions.Pages_OrderProductInfos_Delete, L("DeleteOrderProductInfo"));

            var paymentTypes = pages.CreateChildPermission(AppPermissions.Pages_PaymentTypes, L("PaymentTypes"));
            paymentTypes.CreateChildPermission(AppPermissions.Pages_PaymentTypes_Create, L("CreateNewPaymentType"));
            paymentTypes.CreateChildPermission(AppPermissions.Pages_PaymentTypes_Edit, L("EditPaymentType"));
            paymentTypes.CreateChildPermission(AppPermissions.Pages_PaymentTypes_Delete, L("DeletePaymentType"));

            var orderPaymentInfos = pages.CreateChildPermission(AppPermissions.Pages_OrderPaymentInfos, L("OrderPaymentInfos"));
            orderPaymentInfos.CreateChildPermission(AppPermissions.Pages_OrderPaymentInfos_Create, L("CreateNewOrderPaymentInfo"));
            orderPaymentInfos.CreateChildPermission(AppPermissions.Pages_OrderPaymentInfos_Edit, L("EditOrderPaymentInfo"));
            orderPaymentInfos.CreateChildPermission(AppPermissions.Pages_OrderPaymentInfos_Delete, L("DeleteOrderPaymentInfo"));

            var orderfulfillmentTeams = pages.CreateChildPermission(AppPermissions.Pages_OrderfulfillmentTeams, L("OrderfulfillmentTeams"));
            orderfulfillmentTeams.CreateChildPermission(AppPermissions.Pages_OrderfulfillmentTeams_Create, L("CreateNewOrderfulfillmentTeam"));
            orderfulfillmentTeams.CreateChildPermission(AppPermissions.Pages_OrderfulfillmentTeams_Edit, L("EditOrderfulfillmentTeam"));
            orderfulfillmentTeams.CreateChildPermission(AppPermissions.Pages_OrderfulfillmentTeams_Delete, L("DeleteOrderfulfillmentTeam"));

            var orderFulfillmentStatuses = pages.CreateChildPermission(AppPermissions.Pages_OrderFulfillmentStatuses, L("OrderFulfillmentStatuses"));
            orderFulfillmentStatuses.CreateChildPermission(AppPermissions.Pages_OrderFulfillmentStatuses_Create, L("CreateNewOrderFulfillmentStatus"));
            orderFulfillmentStatuses.CreateChildPermission(AppPermissions.Pages_OrderFulfillmentStatuses_Edit, L("EditOrderFulfillmentStatus"));
            orderFulfillmentStatuses.CreateChildPermission(AppPermissions.Pages_OrderFulfillmentStatuses_Delete, L("DeleteOrderFulfillmentStatus"));

            var orderDeliveryInfos = pages.CreateChildPermission(AppPermissions.Pages_OrderDeliveryInfos, L("OrderDeliveryInfos"));
            orderDeliveryInfos.CreateChildPermission(AppPermissions.Pages_OrderDeliveryInfos_Create, L("CreateNewOrderDeliveryInfo"));
            orderDeliveryInfos.CreateChildPermission(AppPermissions.Pages_OrderDeliveryInfos_Edit, L("EditOrderDeliveryInfo"));
            orderDeliveryInfos.CreateChildPermission(AppPermissions.Pages_OrderDeliveryInfos_Delete, L("DeleteOrderDeliveryInfo"));

            var orders = pages.CreateChildPermission(AppPermissions.Pages_Orders, L("Orders"));
            orders.CreateChildPermission(AppPermissions.Pages_Orders_Create, L("CreateNewOrder"));
            orders.CreateChildPermission(AppPermissions.Pages_Orders_Edit, L("EditOrder"));
            orders.CreateChildPermission(AppPermissions.Pages_Orders_Delete, L("DeleteOrder"));

            var orderStatuses = pages.CreateChildPermission(AppPermissions.Pages_OrderStatuses, L("OrderStatuses"));
            orderStatuses.CreateChildPermission(AppPermissions.Pages_OrderStatuses_Create, L("CreateNewOrderStatus"));
            orderStatuses.CreateChildPermission(AppPermissions.Pages_OrderStatuses_Edit, L("EditOrderStatus"));
            orderStatuses.CreateChildPermission(AppPermissions.Pages_OrderStatuses_Delete, L("DeleteOrderStatus"));

            var orderSalesChannels = pages.CreateChildPermission(AppPermissions.Pages_OrderSalesChannels, L("OrderSalesChannels"));
            orderSalesChannels.CreateChildPermission(AppPermissions.Pages_OrderSalesChannels_Create, L("CreateNewOrderSalesChannel"));
            orderSalesChannels.CreateChildPermission(AppPermissions.Pages_OrderSalesChannels_Edit, L("EditOrderSalesChannel"));
            orderSalesChannels.CreateChildPermission(AppPermissions.Pages_OrderSalesChannels_Delete, L("DeleteOrderSalesChannel"));

            var productWholeSalePrices = pages.CreateChildPermission(AppPermissions.Pages_ProductWholeSalePrices, L("ProductWholeSalePrices"));
            productWholeSalePrices.CreateChildPermission(AppPermissions.Pages_ProductWholeSalePrices_Create, L("CreateNewProductWholeSalePrice"));
            productWholeSalePrices.CreateChildPermission(AppPermissions.Pages_ProductWholeSalePrices_Edit, L("EditProductWholeSalePrice"));
            productWholeSalePrices.CreateChildPermission(AppPermissions.Pages_ProductWholeSalePrices_Delete, L("DeleteProductWholeSalePrice"));

            var productWholeSaleQuantityTypes = pages.CreateChildPermission(AppPermissions.Pages_ProductWholeSaleQuantityTypes, L("ProductWholeSaleQuantityTypes"));
            productWholeSaleQuantityTypes.CreateChildPermission(AppPermissions.Pages_ProductWholeSaleQuantityTypes_Create, L("CreateNewProductWholeSaleQuantityType"));
            productWholeSaleQuantityTypes.CreateChildPermission(AppPermissions.Pages_ProductWholeSaleQuantityTypes_Edit, L("EditProductWholeSaleQuantityType"));
            productWholeSaleQuantityTypes.CreateChildPermission(AppPermissions.Pages_ProductWholeSaleQuantityTypes_Delete, L("DeleteProductWholeSaleQuantityType"));

            var productFlashSaleProductMaps = pages.CreateChildPermission(AppPermissions.Pages_ProductFlashSaleProductMaps, L("ProductFlashSaleProductMaps"));
            productFlashSaleProductMaps.CreateChildPermission(AppPermissions.Pages_ProductFlashSaleProductMaps_Create, L("CreateNewProductFlashSaleProductMap"));
            productFlashSaleProductMaps.CreateChildPermission(AppPermissions.Pages_ProductFlashSaleProductMaps_Edit, L("EditProductFlashSaleProductMap"));
            productFlashSaleProductMaps.CreateChildPermission(AppPermissions.Pages_ProductFlashSaleProductMaps_Delete, L("DeleteProductFlashSaleProductMap"));

            var productAndGiftCardMaps = pages.CreateChildPermission(AppPermissions.Pages_ProductAndGiftCardMaps, L("ProductAndGiftCardMaps"));
            productAndGiftCardMaps.CreateChildPermission(AppPermissions.Pages_ProductAndGiftCardMaps_Create, L("CreateNewProductAndGiftCardMap"));
            productAndGiftCardMaps.CreateChildPermission(AppPermissions.Pages_ProductAndGiftCardMaps_Edit, L("EditProductAndGiftCardMap"));
            productAndGiftCardMaps.CreateChildPermission(AppPermissions.Pages_ProductAndGiftCardMaps_Delete, L("DeleteProductAndGiftCardMap"));

            var productCategoryVariantMaps = pages.CreateChildPermission(AppPermissions.Pages_ProductCategoryVariantMaps, L("ProductCategoryVariantMaps"));
            productCategoryVariantMaps.CreateChildPermission(AppPermissions.Pages_ProductCategoryVariantMaps_Create, L("CreateNewProductCategoryVariantMap"));
            productCategoryVariantMaps.CreateChildPermission(AppPermissions.Pages_ProductCategoryVariantMaps_Edit, L("EditProductCategoryVariantMap"));
            productCategoryVariantMaps.CreateChildPermission(AppPermissions.Pages_ProductCategoryVariantMaps_Delete, L("DeleteProductCategoryVariantMap"));

            var productByVariants = pages.CreateChildPermission(AppPermissions.Pages_ProductByVariants, L("ProductByVariants"));
            productByVariants.CreateChildPermission(AppPermissions.Pages_ProductByVariants_Create, L("CreateNewProductByVariant"));
            productByVariants.CreateChildPermission(AppPermissions.Pages_ProductByVariants_Edit, L("EditProductByVariant"));
            productByVariants.CreateChildPermission(AppPermissions.Pages_ProductByVariants_Delete, L("DeleteProductByVariant"));

            var productOwnerPublicContactInfos = pages.CreateChildPermission(AppPermissions.Pages_ProductOwnerPublicContactInfos, L("ProductOwnerPublicContactInfos"));
            productOwnerPublicContactInfos.CreateChildPermission(AppPermissions.Pages_ProductOwnerPublicContactInfos_Create, L("CreateNewProductOwnerPublicContactInfo"));
            productOwnerPublicContactInfos.CreateChildPermission(AppPermissions.Pages_ProductOwnerPublicContactInfos_Edit, L("EditProductOwnerPublicContactInfo"));
            productOwnerPublicContactInfos.CreateChildPermission(AppPermissions.Pages_ProductOwnerPublicContactInfos_Delete, L("DeleteProductOwnerPublicContactInfo"));

            var productReviewFeedbacks = pages.CreateChildPermission(AppPermissions.Pages_ProductReviewFeedbacks, L("ProductReviewFeedbacks"));
            productReviewFeedbacks.CreateChildPermission(AppPermissions.Pages_ProductReviewFeedbacks_Create, L("CreateNewProductReviewFeedback"));
            productReviewFeedbacks.CreateChildPermission(AppPermissions.Pages_ProductReviewFeedbacks_Edit, L("EditProductReviewFeedback"));
            productReviewFeedbacks.CreateChildPermission(AppPermissions.Pages_ProductReviewFeedbacks_Delete, L("DeleteProductReviewFeedback"));

            var productReviews = pages.CreateChildPermission(AppPermissions.Pages_ProductReviews, L("ProductReviews"));
            productReviews.CreateChildPermission(AppPermissions.Pages_ProductReviews_Create, L("CreateNewProductReview"));
            productReviews.CreateChildPermission(AppPermissions.Pages_ProductReviews_Edit, L("EditProductReview"));
            productReviews.CreateChildPermission(AppPermissions.Pages_ProductReviews_Delete, L("DeleteProductReview"));

            var productReturnInfos = pages.CreateChildPermission(AppPermissions.Pages_ProductReturnInfos, L("ProductReturnInfos"));
            productReturnInfos.CreateChildPermission(AppPermissions.Pages_ProductReturnInfos_Create, L("CreateNewProductReturnInfo"));
            productReturnInfos.CreateChildPermission(AppPermissions.Pages_ProductReturnInfos_Edit, L("EditProductReturnInfo"));
            productReturnInfos.CreateChildPermission(AppPermissions.Pages_ProductReturnInfos_Delete, L("DeleteProductReturnInfo"));

            var returnStatuses = pages.CreateChildPermission(AppPermissions.Pages_ReturnStatuses, L("ReturnStatuses"));
            returnStatuses.CreateChildPermission(AppPermissions.Pages_ReturnStatuses_Create, L("CreateNewReturnStatus"));
            returnStatuses.CreateChildPermission(AppPermissions.Pages_ReturnStatuses_Edit, L("EditReturnStatus"));
            returnStatuses.CreateChildPermission(AppPermissions.Pages_ReturnStatuses_Delete, L("DeleteReturnStatus"));

            var returnTypes = pages.CreateChildPermission(AppPermissions.Pages_ReturnTypes, L("ReturnTypes"));
            returnTypes.CreateChildPermission(AppPermissions.Pages_ReturnTypes_Create, L("CreateNewReturnType"));
            returnTypes.CreateChildPermission(AppPermissions.Pages_ReturnTypes_Edit, L("EditReturnType"));
            returnTypes.CreateChildPermission(AppPermissions.Pages_ReturnTypes_Delete, L("DeleteReturnType"));

            var productUpsellRelatedProducts = pages.CreateChildPermission(AppPermissions.Pages_ProductUpsellRelatedProducts, L("ProductUpsellRelatedProducts"));
            productUpsellRelatedProducts.CreateChildPermission(AppPermissions.Pages_ProductUpsellRelatedProducts_Create, L("CreateNewProductUpsellRelatedProduct"));
            productUpsellRelatedProducts.CreateChildPermission(AppPermissions.Pages_ProductUpsellRelatedProducts_Edit, L("EditProductUpsellRelatedProduct"));
            productUpsellRelatedProducts.CreateChildPermission(AppPermissions.Pages_ProductUpsellRelatedProducts_Delete, L("DeleteProductUpsellRelatedProduct"));

            var productCrossSellProducts = pages.CreateChildPermission(AppPermissions.Pages_ProductCrossSellProducts, L("ProductCrossSellProducts"));
            productCrossSellProducts.CreateChildPermission(AppPermissions.Pages_ProductCrossSellProducts_Create, L("CreateNewProductCrossSellProduct"));
            productCrossSellProducts.CreateChildPermission(AppPermissions.Pages_ProductCrossSellProducts_Edit, L("EditProductCrossSellProduct"));
            productCrossSellProducts.CreateChildPermission(AppPermissions.Pages_ProductCrossSellProducts_Delete, L("DeleteProductCrossSellProduct"));

            var productAccountTeams = pages.CreateChildPermission(AppPermissions.Pages_ProductAccountTeams, L("ProductAccountTeams"));
            productAccountTeams.CreateChildPermission(AppPermissions.Pages_ProductAccountTeams_Create, L("CreateNewProductAccountTeam"));
            productAccountTeams.CreateChildPermission(AppPermissions.Pages_ProductAccountTeams_Edit, L("EditProductAccountTeam"));
            productAccountTeams.CreateChildPermission(AppPermissions.Pages_ProductAccountTeams_Delete, L("DeleteProductAccountTeam"));

            var subscriptionTypes = pages.CreateChildPermission(AppPermissions.Pages_SubscriptionTypes, L("SubscriptionTypes"));
            subscriptionTypes.CreateChildPermission(AppPermissions.Pages_SubscriptionTypes_Create, L("CreateNewSubscriptionType"));
            subscriptionTypes.CreateChildPermission(AppPermissions.Pages_SubscriptionTypes_Edit, L("EditSubscriptionType"));
            subscriptionTypes.CreateChildPermission(AppPermissions.Pages_SubscriptionTypes_Delete, L("DeleteSubscriptionType"));

            var productSubscriptionMaps = pages.CreateChildPermission(AppPermissions.Pages_ProductSubscriptionMaps, L("ProductSubscriptionMaps"));
            productSubscriptionMaps.CreateChildPermission(AppPermissions.Pages_ProductSubscriptionMaps_Create, L("CreateNewProductSubscriptionMap"));
            productSubscriptionMaps.CreateChildPermission(AppPermissions.Pages_ProductSubscriptionMaps_Edit, L("EditProductSubscriptionMap"));
            productSubscriptionMaps.CreateChildPermission(AppPermissions.Pages_ProductSubscriptionMaps_Delete, L("DeleteProductSubscriptionMap"));

            var socialMedias = pages.CreateChildPermission(AppPermissions.Pages_SocialMedias, L("SocialMedias"));
            socialMedias.CreateChildPermission(AppPermissions.Pages_SocialMedias_Create, L("CreateNewSocialMedia"));
            socialMedias.CreateChildPermission(AppPermissions.Pages_SocialMedias_Edit, L("EditSocialMedia"));
            socialMedias.CreateChildPermission(AppPermissions.Pages_SocialMedias_Delete, L("DeleteSocialMedia"));

            var productCustomerStats = pages.CreateChildPermission(AppPermissions.Pages_ProductCustomerStats, L("ProductCustomerStats"));
            productCustomerStats.CreateChildPermission(AppPermissions.Pages_ProductCustomerStats_Create, L("CreateNewProductCustomerStat"));
            productCustomerStats.CreateChildPermission(AppPermissions.Pages_ProductCustomerStats_Edit, L("EditProductCustomerStat"));
            productCustomerStats.CreateChildPermission(AppPermissions.Pages_ProductCustomerStats_Delete, L("DeleteProductCustomerStat"));

            var productFaqs = pages.CreateChildPermission(AppPermissions.Pages_ProductFaqs, L("ProductFaqs"));
            productFaqs.CreateChildPermission(AppPermissions.Pages_ProductFaqs_Create, L("CreateNewProductFaq"));
            productFaqs.CreateChildPermission(AppPermissions.Pages_ProductFaqs_Edit, L("EditProductFaq"));
            productFaqs.CreateChildPermission(AppPermissions.Pages_ProductFaqs_Delete, L("DeleteProductFaq"));

            var productCustomerQueries = pages.CreateChildPermission(AppPermissions.Pages_ProductCustomerQueries, L("ProductCustomerQueries"));
            productCustomerQueries.CreateChildPermission(AppPermissions.Pages_ProductCustomerQueries_Create, L("CreateNewProductCustomerQuery"));
            productCustomerQueries.CreateChildPermission(AppPermissions.Pages_ProductCustomerQueries_Edit, L("EditProductCustomerQuery"));
            productCustomerQueries.CreateChildPermission(AppPermissions.Pages_ProductCustomerQueries_Delete, L("DeleteProductCustomerQuery"));

            var productPackages = pages.CreateChildPermission(AppPermissions.Pages_ProductPackages, L("ProductPackages"));
            productPackages.CreateChildPermission(AppPermissions.Pages_ProductPackages_Create, L("CreateNewProductPackage"));
            productPackages.CreateChildPermission(AppPermissions.Pages_ProductPackages_Edit, L("EditProductPackage"));
            productPackages.CreateChildPermission(AppPermissions.Pages_ProductPackages_Delete, L("DeleteProductPackage"));

            var productNotes = pages.CreateChildPermission(AppPermissions.Pages_ProductNotes, L("ProductNotes"));
            productNotes.CreateChildPermission(AppPermissions.Pages_ProductNotes_Create, L("CreateNewProductNote"));
            productNotes.CreateChildPermission(AppPermissions.Pages_ProductNotes_Edit, L("EditProductNote"));
            productNotes.CreateChildPermission(AppPermissions.Pages_ProductNotes_Delete, L("DeleteProductNote"));

            var productMedias = pages.CreateChildPermission(AppPermissions.Pages_ProductMedias, L("ProductMedias"));
            productMedias.CreateChildPermission(AppPermissions.Pages_ProductMedias_Create, L("CreateNewProductMedia"));
            productMedias.CreateChildPermission(AppPermissions.Pages_ProductMedias_Edit, L("EditProductMedia"));
            productMedias.CreateChildPermission(AppPermissions.Pages_ProductMedias_Delete, L("DeleteProductMedia"));

            var productCategoryTeams = pages.CreateChildPermission(AppPermissions.Pages_ProductCategoryTeams, L("ProductCategoryTeams"));
            productCategoryTeams.CreateChildPermission(AppPermissions.Pages_ProductCategoryTeams_Create, L("CreateNewProductCategoryTeam"));
            productCategoryTeams.CreateChildPermission(AppPermissions.Pages_ProductCategoryTeams_Edit, L("EditProductCategoryTeam"));
            productCategoryTeams.CreateChildPermission(AppPermissions.Pages_ProductCategoryTeams_Delete, L("DeleteProductCategoryTeam"));

            var productCategoryMaps = pages.CreateChildPermission(AppPermissions.Pages_ProductCategoryMaps, L("ProductCategoryMaps"));
            productCategoryMaps.CreateChildPermission(AppPermissions.Pages_ProductCategoryMaps_Create, L("CreateNewProductCategoryMap"));
            productCategoryMaps.CreateChildPermission(AppPermissions.Pages_ProductCategoryMaps_Edit, L("EditProductCategoryMap"));
            productCategoryMaps.CreateChildPermission(AppPermissions.Pages_ProductCategoryMaps_Delete, L("DeleteProductCategoryMap"));

            var productVariants = pages.CreateChildPermission(AppPermissions.Pages_ProductVariants, L("ProductVariants"));
            productVariants.CreateChildPermission(AppPermissions.Pages_ProductVariants_Create, L("CreateNewProductVariant"));
            productVariants.CreateChildPermission(AppPermissions.Pages_ProductVariants_Edit, L("EditProductVariant"));
            productVariants.CreateChildPermission(AppPermissions.Pages_ProductVariants_Delete, L("DeleteProductVariant"));

            var productVariantCategories = pages.CreateChildPermission(AppPermissions.Pages_ProductVariantCategories, L("ProductVariantCategories"));
            productVariantCategories.CreateChildPermission(AppPermissions.Pages_ProductVariantCategories_Create, L("CreateNewProductVariantCategory"));
            productVariantCategories.CreateChildPermission(AppPermissions.Pages_ProductVariantCategories_Edit, L("EditProductVariantCategory"));
            productVariantCategories.CreateChildPermission(AppPermissions.Pages_ProductVariantCategories_Delete, L("DeleteProductVariantCategory"));

            var hubNavigationMenus = pages.CreateChildPermission(AppPermissions.Pages_HubNavigationMenus, L("HubNavigationMenus"));
            hubNavigationMenus.CreateChildPermission(AppPermissions.Pages_HubNavigationMenus_Create, L("CreateNewHubNavigationMenu"));
            hubNavigationMenus.CreateChildPermission(AppPermissions.Pages_HubNavigationMenus_Edit, L("EditHubNavigationMenu"));
            hubNavigationMenus.CreateChildPermission(AppPermissions.Pages_HubNavigationMenus_Delete, L("DeleteHubNavigationMenu"));

            var masterNavigationMenus = pages.CreateChildPermission(AppPermissions.Pages_MasterNavigationMenus, L("MasterNavigationMenus"));
            masterNavigationMenus.CreateChildPermission(AppPermissions.Pages_MasterNavigationMenus_Create, L("CreateNewMasterNavigationMenu"));
            masterNavigationMenus.CreateChildPermission(AppPermissions.Pages_MasterNavigationMenus_Edit, L("EditMasterNavigationMenu"));
            masterNavigationMenus.CreateChildPermission(AppPermissions.Pages_MasterNavigationMenus_Delete, L("DeleteMasterNavigationMenu"));

            var hubAccountTeams = pages.CreateChildPermission(AppPermissions.Pages_HubAccountTeams, L("HubAccountTeams"));
            hubAccountTeams.CreateChildPermission(AppPermissions.Pages_HubAccountTeams_Create, L("CreateNewHubAccountTeam"));
            hubAccountTeams.CreateChildPermission(AppPermissions.Pages_HubAccountTeams_Edit, L("EditHubAccountTeam"));
            hubAccountTeams.CreateChildPermission(AppPermissions.Pages_HubAccountTeams_Delete, L("DeleteHubAccountTeam"));

            var hubSalesProjections = pages.CreateChildPermission(AppPermissions.Pages_HubSalesProjections, L("HubSalesProjections"));
            hubSalesProjections.CreateChildPermission(AppPermissions.Pages_HubSalesProjections_Create, L("CreateNewHubSalesProjection"));
            hubSalesProjections.CreateChildPermission(AppPermissions.Pages_HubSalesProjections_Edit, L("EditHubSalesProjection"));
            hubSalesProjections.CreateChildPermission(AppPermissions.Pages_HubSalesProjections_Delete, L("DeleteHubSalesProjection"));

            var hubZipCodeMaps = pages.CreateChildPermission(AppPermissions.Pages_HubZipCodeMaps, L("HubZipCodeMaps"));
            hubZipCodeMaps.CreateChildPermission(AppPermissions.Pages_HubZipCodeMaps_Create, L("CreateNewHubZipCodeMap"));
            hubZipCodeMaps.CreateChildPermission(AppPermissions.Pages_HubZipCodeMaps_Edit, L("EditHubZipCodeMap"));
            hubZipCodeMaps.CreateChildPermission(AppPermissions.Pages_HubZipCodeMaps_Delete, L("DeleteHubZipCodeMap"));

            var hubContacts = pages.CreateChildPermission(AppPermissions.Pages_HubContacts, L("HubContacts"));
            hubContacts.CreateChildPermission(AppPermissions.Pages_HubContacts_Create, L("CreateNewHubContact"));
            hubContacts.CreateChildPermission(AppPermissions.Pages_HubContacts_Edit, L("EditHubContact"));
            hubContacts.CreateChildPermission(AppPermissions.Pages_HubContacts_Delete, L("DeleteHubContact"));

            var hubBusinesses = pages.CreateChildPermission(AppPermissions.Pages_HubBusinesses, L("HubBusinesses"));
            hubBusinesses.CreateChildPermission(AppPermissions.Pages_HubBusinesses_Create, L("CreateNewHubBusiness"));
            hubBusinesses.CreateChildPermission(AppPermissions.Pages_HubBusinesses_Edit, L("EditHubBusiness"));
            hubBusinesses.CreateChildPermission(AppPermissions.Pages_HubBusinesses_Delete, L("DeleteHubBusiness"));

            var hubStores = pages.CreateChildPermission(AppPermissions.Pages_HubStores, L("HubStores"));
            hubStores.CreateChildPermission(AppPermissions.Pages_HubStores_Create, L("CreateNewHubStore"));
            hubStores.CreateChildPermission(AppPermissions.Pages_HubStores_Edit, L("EditHubStore"));
            hubStores.CreateChildPermission(AppPermissions.Pages_HubStores_Delete, L("DeleteHubStore"));

            var hubProducts = pages.CreateChildPermission(AppPermissions.Pages_HubProducts, L("HubProducts"));
            hubProducts.CreateChildPermission(AppPermissions.Pages_HubProducts_Create, L("CreateNewHubProduct"));
            hubProducts.CreateChildPermission(AppPermissions.Pages_HubProducts_Edit, L("EditHubProduct"));
            hubProducts.CreateChildPermission(AppPermissions.Pages_HubProducts_Delete, L("DeleteHubProduct"));

            var hubProductCategories = pages.CreateChildPermission(AppPermissions.Pages_HubProductCategories, L("HubProductCategories"));
            hubProductCategories.CreateChildPermission(AppPermissions.Pages_HubProductCategories_Create, L("CreateNewHubProductCategory"));
            hubProductCategories.CreateChildPermission(AppPermissions.Pages_HubProductCategories_Edit, L("EditHubProductCategory"));
            hubProductCategories.CreateChildPermission(AppPermissions.Pages_HubProductCategories_Delete, L("DeleteHubProductCategory"));

            var leadReferralRewards = pages.CreateChildPermission(AppPermissions.Pages_LeadReferralRewards, L("LeadReferralRewards"));
            leadReferralRewards.CreateChildPermission(AppPermissions.Pages_LeadReferralRewards_Create, L("CreateNewLeadReferralReward"));
            leadReferralRewards.CreateChildPermission(AppPermissions.Pages_LeadReferralRewards_Edit, L("EditLeadReferralReward"));
            leadReferralRewards.CreateChildPermission(AppPermissions.Pages_LeadReferralRewards_Delete, L("DeleteLeadReferralReward"));

            var leadContacts = pages.CreateChildPermission(AppPermissions.Pages_LeadContacts, L("LeadContacts"));
            leadContacts.CreateChildPermission(AppPermissions.Pages_LeadContacts_Create, L("CreateNewLeadContact"));
            leadContacts.CreateChildPermission(AppPermissions.Pages_LeadContacts_Edit, L("EditLeadContact"));
            leadContacts.CreateChildPermission(AppPermissions.Pages_LeadContacts_Delete, L("DeleteLeadContact"));

            var leadNotes = pages.CreateChildPermission(AppPermissions.Pages_LeadNotes, L("LeadNotes"));
            leadNotes.CreateChildPermission(AppPermissions.Pages_LeadNotes_Create, L("CreateNewLeadNote"));
            leadNotes.CreateChildPermission(AppPermissions.Pages_LeadNotes_Edit, L("EditLeadNote"));
            leadNotes.CreateChildPermission(AppPermissions.Pages_LeadNotes_Delete, L("DeleteLeadNote"));

            var leadTasks = pages.CreateChildPermission(AppPermissions.Pages_LeadTasks, L("LeadTasks"));
            leadTasks.CreateChildPermission(AppPermissions.Pages_LeadTasks_Create, L("CreateNewLeadTask"));
            leadTasks.CreateChildPermission(AppPermissions.Pages_LeadTasks_Edit, L("EditLeadTask"));
            leadTasks.CreateChildPermission(AppPermissions.Pages_LeadTasks_Delete, L("DeleteLeadTask"));

            var leadTags = pages.CreateChildPermission(AppPermissions.Pages_LeadTags, L("LeadTags"));
            leadTags.CreateChildPermission(AppPermissions.Pages_LeadTags_Create, L("CreateNewLeadTag"));
            leadTags.CreateChildPermission(AppPermissions.Pages_LeadTags_Edit, L("EditLeadTag"));
            leadTags.CreateChildPermission(AppPermissions.Pages_LeadTags_Delete, L("DeleteLeadTag"));

            var leadSalesTeams = pages.CreateChildPermission(AppPermissions.Pages_LeadSalesTeams, L("LeadSalesTeams"));
            leadSalesTeams.CreateChildPermission(AppPermissions.Pages_LeadSalesTeams_Create, L("CreateNewLeadSalesTeam"));
            leadSalesTeams.CreateChildPermission(AppPermissions.Pages_LeadSalesTeams_Edit, L("EditLeadSalesTeam"));
            leadSalesTeams.CreateChildPermission(AppPermissions.Pages_LeadSalesTeams_Delete, L("DeleteLeadSalesTeam"));

            var leadPipelineStatuses = pages.CreateChildPermission(AppPermissions.Pages_LeadPipelineStatuses, L("LeadPipelineStatuses"));
            leadPipelineStatuses.CreateChildPermission(AppPermissions.Pages_LeadPipelineStatuses_Create, L("CreateNewLeadPipelineStatus"));
            leadPipelineStatuses.CreateChildPermission(AppPermissions.Pages_LeadPipelineStatuses_Edit, L("EditLeadPipelineStatus"));
            leadPipelineStatuses.CreateChildPermission(AppPermissions.Pages_LeadPipelineStatuses_Delete, L("DeleteLeadPipelineStatus"));

            var leads = pages.CreateChildPermission(AppPermissions.Pages_Leads, L("Leads"));
            leads.CreateChildPermission(AppPermissions.Pages_Leads_Create, L("CreateNewLead"));
            leads.CreateChildPermission(AppPermissions.Pages_Leads_Edit, L("EditLead"));
            leads.CreateChildPermission(AppPermissions.Pages_Leads_Delete, L("DeleteLead"));

            var leadPipelineStages = pages.CreateChildPermission(AppPermissions.Pages_LeadPipelineStages, L("LeadPipelineStages"));
            leadPipelineStages.CreateChildPermission(AppPermissions.Pages_LeadPipelineStages_Create, L("CreateNewLeadPipelineStage"));
            leadPipelineStages.CreateChildPermission(AppPermissions.Pages_LeadPipelineStages_Edit, L("EditLeadPipelineStage"));
            leadPipelineStages.CreateChildPermission(AppPermissions.Pages_LeadPipelineStages_Delete, L("DeleteLeadPipelineStage"));

            var leadSources = pages.CreateChildPermission(AppPermissions.Pages_LeadSources, L("LeadSources"));
            leadSources.CreateChildPermission(AppPermissions.Pages_LeadSources_Create, L("CreateNewLeadSource"));
            leadSources.CreateChildPermission(AppPermissions.Pages_LeadSources_Edit, L("EditLeadSource"));
            leadSources.CreateChildPermission(AppPermissions.Pages_LeadSources_Delete, L("DeleteLeadSource"));

            var storeZipCodeMaps = pages.CreateChildPermission(AppPermissions.Pages_StoreZipCodeMaps, L("StoreZipCodeMaps"));
            storeZipCodeMaps.CreateChildPermission(AppPermissions.Pages_StoreZipCodeMaps_Create, L("CreateNewStoreZipCodeMap"));
            storeZipCodeMaps.CreateChildPermission(AppPermissions.Pages_StoreZipCodeMaps_Edit, L("EditStoreZipCodeMap"));
            storeZipCodeMaps.CreateChildPermission(AppPermissions.Pages_StoreZipCodeMaps_Delete, L("DeleteStoreZipCodeMap"));

            var storeReviewFeedbacks = pages.CreateChildPermission(AppPermissions.Pages_StoreReviewFeedbacks, L("StoreReviewFeedbacks"));
            storeReviewFeedbacks.CreateChildPermission(AppPermissions.Pages_StoreReviewFeedbacks_Create, L("CreateNewStoreReviewFeedback"));
            storeReviewFeedbacks.CreateChildPermission(AppPermissions.Pages_StoreReviewFeedbacks_Edit, L("EditStoreReviewFeedback"));
            storeReviewFeedbacks.CreateChildPermission(AppPermissions.Pages_StoreReviewFeedbacks_Delete, L("DeleteStoreReviewFeedback"));

            var storeReviews = pages.CreateChildPermission(AppPermissions.Pages_StoreReviews, L("StoreReviews"));
            storeReviews.CreateChildPermission(AppPermissions.Pages_StoreReviews_Create, L("CreateNewStoreReview"));
            storeReviews.CreateChildPermission(AppPermissions.Pages_StoreReviews_Edit, L("EditStoreReview"));
            storeReviews.CreateChildPermission(AppPermissions.Pages_StoreReviews_Delete, L("DeleteStoreReview"));

            var storeRelevantStores = pages.CreateChildPermission(AppPermissions.Pages_StoreRelevantStores, L("StoreRelevantStores"));
            storeRelevantStores.CreateChildPermission(AppPermissions.Pages_StoreRelevantStores_Create, L("CreateNewStoreRelevantStore"));
            storeRelevantStores.CreateChildPermission(AppPermissions.Pages_StoreRelevantStores_Edit, L("EditStoreRelevantStore"));
            storeRelevantStores.CreateChildPermission(AppPermissions.Pages_StoreRelevantStores_Delete, L("DeleteStoreRelevantStore"));

            var storeMarketplaceCommissionSettings = pages.CreateChildPermission(AppPermissions.Pages_StoreMarketplaceCommissionSettings, L("StoreMarketplaceCommissionSettings"));
            storeMarketplaceCommissionSettings.CreateChildPermission(AppPermissions.Pages_StoreMarketplaceCommissionSettings_Create, L("CreateNewStoreMarketplaceCommissionSetting"));
            storeMarketplaceCommissionSettings.CreateChildPermission(AppPermissions.Pages_StoreMarketplaceCommissionSettings_Edit, L("EditStoreMarketplaceCommissionSetting"));
            storeMarketplaceCommissionSettings.CreateChildPermission(AppPermissions.Pages_StoreMarketplaceCommissionSettings_Delete, L("DeleteStoreMarketplaceCommissionSetting"));

            var marketplaceCommissionTypes = pages.CreateChildPermission(AppPermissions.Pages_MarketplaceCommissionTypes, L("MarketplaceCommissionTypes"));
            marketplaceCommissionTypes.CreateChildPermission(AppPermissions.Pages_MarketplaceCommissionTypes_Create, L("CreateNewMarketplaceCommissionType"));
            marketplaceCommissionTypes.CreateChildPermission(AppPermissions.Pages_MarketplaceCommissionTypes_Edit, L("EditMarketplaceCommissionType"));
            marketplaceCommissionTypes.CreateChildPermission(AppPermissions.Pages_MarketplaceCommissionTypes_Delete, L("DeleteMarketplaceCommissionType"));

            var storeSalesAlerts = pages.CreateChildPermission(AppPermissions.Pages_StoreSalesAlerts, L("StoreSalesAlerts"));
            storeSalesAlerts.CreateChildPermission(AppPermissions.Pages_StoreSalesAlerts_Create, L("CreateNewStoreSalesAlert"));
            storeSalesAlerts.CreateChildPermission(AppPermissions.Pages_StoreSalesAlerts_Edit, L("EditStoreSalesAlert"));
            storeSalesAlerts.CreateChildPermission(AppPermissions.Pages_StoreSalesAlerts_Delete, L("DeleteStoreSalesAlert"));

            var storeTaxes = pages.CreateChildPermission(AppPermissions.Pages_StoreTaxes, L("StoreTaxes"));
            storeTaxes.CreateChildPermission(AppPermissions.Pages_StoreTaxes_Create, L("CreateNewStoreTax"));
            storeTaxes.CreateChildPermission(AppPermissions.Pages_StoreTaxes_Edit, L("EditStoreTax"));
            storeTaxes.CreateChildPermission(AppPermissions.Pages_StoreTaxes_Delete, L("DeleteStoreTax"));

            var storeBankAccounts = pages.CreateChildPermission(AppPermissions.Pages_StoreBankAccounts, L("StoreBankAccounts"));
            storeBankAccounts.CreateChildPermission(AppPermissions.Pages_StoreBankAccounts_Create, L("CreateNewStoreBankAccount"));
            storeBankAccounts.CreateChildPermission(AppPermissions.Pages_StoreBankAccounts_Edit, L("EditStoreBankAccount"));
            storeBankAccounts.CreateChildPermission(AppPermissions.Pages_StoreBankAccounts_Delete, L("DeleteStoreBankAccount"));

            var storeNotes = pages.CreateChildPermission(AppPermissions.Pages_StoreNotes, L("StoreNotes"));
            storeNotes.CreateChildPermission(AppPermissions.Pages_StoreNotes_Create, L("CreateNewStoreNote"));
            storeNotes.CreateChildPermission(AppPermissions.Pages_StoreNotes_Edit, L("EditStoreNote"));
            storeNotes.CreateChildPermission(AppPermissions.Pages_StoreNotes_Delete, L("DeleteStoreNote"));

            var storeMedias = pages.CreateChildPermission(AppPermissions.Pages_StoreMedias, L("StoreMedias"));
            storeMedias.CreateChildPermission(AppPermissions.Pages_StoreMedias_Create, L("CreateNewStoreMedia"));
            storeMedias.CreateChildPermission(AppPermissions.Pages_StoreMedias_Edit, L("EditStoreMedia"));
            storeMedias.CreateChildPermission(AppPermissions.Pages_StoreMedias_Delete, L("DeleteStoreMedia"));

            var storeLocations = pages.CreateChildPermission(AppPermissions.Pages_StoreLocations, L("StoreLocations"));
            storeLocations.CreateChildPermission(AppPermissions.Pages_StoreLocations_Create, L("CreateNewStoreLocation"));
            storeLocations.CreateChildPermission(AppPermissions.Pages_StoreLocations_Edit, L("EditStoreLocation"));
            storeLocations.CreateChildPermission(AppPermissions.Pages_StoreLocations_Delete, L("DeleteStoreLocation"));

            var storeBusinessHours = pages.CreateChildPermission(AppPermissions.Pages_StoreBusinessHours, L("StoreBusinessHours"));
            storeBusinessHours.CreateChildPermission(AppPermissions.Pages_StoreBusinessHours_Create, L("CreateNewStoreBusinessHour"));
            storeBusinessHours.CreateChildPermission(AppPermissions.Pages_StoreBusinessHours_Edit, L("EditStoreBusinessHour"));
            storeBusinessHours.CreateChildPermission(AppPermissions.Pages_StoreBusinessHours_Delete, L("DeleteStoreBusinessHour"));

            var storeContactMaps = pages.CreateChildPermission(AppPermissions.Pages_StoreContactMaps, L("StoreContactMaps"));
            storeContactMaps.CreateChildPermission(AppPermissions.Pages_StoreContactMaps_Create, L("CreateNewStoreContactMap"));
            storeContactMaps.CreateChildPermission(AppPermissions.Pages_StoreContactMaps_Edit, L("EditStoreContactMap"));
            storeContactMaps.CreateChildPermission(AppPermissions.Pages_StoreContactMaps_Delete, L("DeleteStoreContactMap"));

            var storeBusinessCustomerMaps = pages.CreateChildPermission(AppPermissions.Pages_StoreBusinessCustomerMaps, L("StoreBusinessCustomerMaps"));
            storeBusinessCustomerMaps.CreateChildPermission(AppPermissions.Pages_StoreBusinessCustomerMaps_Create, L("CreateNewStoreBusinessCustomerMap"));
            storeBusinessCustomerMaps.CreateChildPermission(AppPermissions.Pages_StoreBusinessCustomerMaps_Edit, L("EditStoreBusinessCustomerMap"));
            storeBusinessCustomerMaps.CreateChildPermission(AppPermissions.Pages_StoreBusinessCustomerMaps_Delete, L("DeleteStoreBusinessCustomerMap"));

            var storeOwnerTeams = pages.CreateChildPermission(AppPermissions.Pages_StoreOwnerTeams, L("StoreOwnerTeams"));
            storeOwnerTeams.CreateChildPermission(AppPermissions.Pages_StoreOwnerTeams_Create, L("CreateNewStoreOwnerTeam"));
            storeOwnerTeams.CreateChildPermission(AppPermissions.Pages_StoreOwnerTeams_Edit, L("EditStoreOwnerTeam"));
            storeOwnerTeams.CreateChildPermission(AppPermissions.Pages_StoreOwnerTeams_Delete, L("DeleteStoreOwnerTeam"));

            var storeAccountTeams = pages.CreateChildPermission(AppPermissions.Pages_StoreAccountTeams, L("StoreAccountTeams"));
            storeAccountTeams.CreateChildPermission(AppPermissions.Pages_StoreAccountTeams_Create, L("CreateNewStoreAccountTeam"));
            storeAccountTeams.CreateChildPermission(AppPermissions.Pages_StoreAccountTeams_Edit, L("EditStoreAccountTeam"));
            storeAccountTeams.CreateChildPermission(AppPermissions.Pages_StoreAccountTeams_Delete, L("DeleteStoreAccountTeam"));

            var storeProductMaps = pages.CreateChildPermission(AppPermissions.Pages_StoreProductMaps, L("StoreProductMaps"));
            storeProductMaps.CreateChildPermission(AppPermissions.Pages_StoreProductMaps_Create, L("CreateNewStoreProductMap"));
            storeProductMaps.CreateChildPermission(AppPermissions.Pages_StoreProductMaps_Edit, L("EditStoreProductMap"));
            storeProductMaps.CreateChildPermission(AppPermissions.Pages_StoreProductMaps_Delete, L("DeleteStoreProductMap"));

            var storeProductCategoryMaps = pages.CreateChildPermission(AppPermissions.Pages_StoreProductCategoryMaps, L("StoreProductCategoryMaps"));
            storeProductCategoryMaps.CreateChildPermission(AppPermissions.Pages_StoreProductCategoryMaps_Create, L("CreateNewStoreProductCategoryMap"));
            storeProductCategoryMaps.CreateChildPermission(AppPermissions.Pages_StoreProductCategoryMaps_Edit, L("EditStoreProductCategoryMap"));
            storeProductCategoryMaps.CreateChildPermission(AppPermissions.Pages_StoreProductCategoryMaps_Delete, L("DeleteStoreProductCategoryMap"));

            var employeeTags = pages.CreateChildPermission(AppPermissions.Pages_EmployeeTags, L("EmployeeTags"));
            employeeTags.CreateChildPermission(AppPermissions.Pages_EmployeeTags_Create, L("CreateNewEmployeeTag"));
            employeeTags.CreateChildPermission(AppPermissions.Pages_EmployeeTags_Edit, L("EditEmployeeTag"));
            employeeTags.CreateChildPermission(AppPermissions.Pages_EmployeeTags_Delete, L("DeleteEmployeeTag"));

            var businessTaskMaps = pages.CreateChildPermission(AppPermissions.Pages_BusinessTaskMaps, L("BusinessTaskMaps"));
            businessTaskMaps.CreateChildPermission(AppPermissions.Pages_BusinessTaskMaps_Create, L("CreateNewBusinessTaskMap"));
            businessTaskMaps.CreateChildPermission(AppPermissions.Pages_BusinessTaskMaps_Edit, L("EditBusinessTaskMap"));
            businessTaskMaps.CreateChildPermission(AppPermissions.Pages_BusinessTaskMaps_Delete, L("DeleteBusinessTaskMap"));

            var businessProductMaps = pages.CreateChildPermission(AppPermissions.Pages_BusinessProductMaps, L("BusinessProductMaps"));
            businessProductMaps.CreateChildPermission(AppPermissions.Pages_BusinessProductMaps_Create, L("CreateNewBusinessProductMap"));
            businessProductMaps.CreateChildPermission(AppPermissions.Pages_BusinessProductMaps_Edit, L("EditBusinessProductMap"));
            businessProductMaps.CreateChildPermission(AppPermissions.Pages_BusinessProductMaps_Delete, L("DeleteBusinessProductMap"));

            var businessStoreMaps = pages.CreateChildPermission(AppPermissions.Pages_BusinessStoreMaps, L("BusinessStoreMaps"));
            businessStoreMaps.CreateChildPermission(AppPermissions.Pages_BusinessStoreMaps_Create, L("CreateNewBusinessStoreMap"));
            businessStoreMaps.CreateChildPermission(AppPermissions.Pages_BusinessStoreMaps_Edit, L("EditBusinessStoreMap"));
            businessStoreMaps.CreateChildPermission(AppPermissions.Pages_BusinessStoreMaps_Delete, L("DeleteBusinessStoreMap"));

            var businessJobMaps = pages.CreateChildPermission(AppPermissions.Pages_BusinessJobMaps, L("BusinessJobMaps"));
            businessJobMaps.CreateChildPermission(AppPermissions.Pages_BusinessJobMaps_Create, L("CreateNewBusinessJobMap"));
            businessJobMaps.CreateChildPermission(AppPermissions.Pages_BusinessJobMaps_Edit, L("EditBusinessJobMap"));
            businessJobMaps.CreateChildPermission(AppPermissions.Pages_BusinessJobMaps_Delete, L("DeleteBusinessJobMap"));

            var businessNotes = pages.CreateChildPermission(AppPermissions.Pages_BusinessNotes, L("BusinessNotes"));
            businessNotes.CreateChildPermission(AppPermissions.Pages_BusinessNotes_Create, L("CreateNewBusinessNote"));
            businessNotes.CreateChildPermission(AppPermissions.Pages_BusinessNotes_Edit, L("EditBusinessNote"));
            businessNotes.CreateChildPermission(AppPermissions.Pages_BusinessNotes_Delete, L("DeleteBusinessNote"));

            var businessUsers = pages.CreateChildPermission(AppPermissions.Pages_BusinessUsers, L("BusinessUsers"));
            businessUsers.CreateChildPermission(AppPermissions.Pages_BusinessUsers_Create, L("CreateNewBusinessUser"));
            businessUsers.CreateChildPermission(AppPermissions.Pages_BusinessUsers_Edit, L("EditBusinessUser"));
            businessUsers.CreateChildPermission(AppPermissions.Pages_BusinessUsers_Delete, L("DeleteBusinessUser"));

            var businessAccountTeams = pages.CreateChildPermission(AppPermissions.Pages_BusinessAccountTeams, L("BusinessAccountTeams"));
            businessAccountTeams.CreateChildPermission(AppPermissions.Pages_BusinessAccountTeams_Create, L("CreateNewBusinessAccountTeam"));
            businessAccountTeams.CreateChildPermission(AppPermissions.Pages_BusinessAccountTeams_Edit, L("EditBusinessAccountTeam"));
            businessAccountTeams.CreateChildPermission(AppPermissions.Pages_BusinessAccountTeams_Delete, L("DeleteBusinessAccountTeam"));

            var businessContactMaps = pages.CreateChildPermission(AppPermissions.Pages_BusinessContactMaps, L("BusinessContactMaps"));
            businessContactMaps.CreateChildPermission(AppPermissions.Pages_BusinessContactMaps_Create, L("CreateNewBusinessContactMap"));
            businessContactMaps.CreateChildPermission(AppPermissions.Pages_BusinessContactMaps_Edit, L("EditBusinessContactMap"));
            businessContactMaps.CreateChildPermission(AppPermissions.Pages_BusinessContactMaps_Delete, L("DeleteBusinessContactMap"));

            var taskTags = pages.CreateChildPermission(AppPermissions.Pages_TaskTags, L("TaskTags"));
            taskTags.CreateChildPermission(AppPermissions.Pages_TaskTags_Create, L("CreateNewTaskTag"));
            taskTags.CreateChildPermission(AppPermissions.Pages_TaskTags_Edit, L("EditTaskTag"));
            taskTags.CreateChildPermission(AppPermissions.Pages_TaskTags_Delete, L("DeleteTaskTag"));

            var taskEvents = pages.CreateChildPermission(AppPermissions.Pages_TaskEvents, L("TaskEvents"));
            taskEvents.CreateChildPermission(AppPermissions.Pages_TaskEvents_Create, L("CreateNewTaskEvent"));
            taskEvents.CreateChildPermission(AppPermissions.Pages_TaskEvents_Edit, L("EditTaskEvent"));
            taskEvents.CreateChildPermission(AppPermissions.Pages_TaskEvents_Delete, L("DeleteTaskEvent"));

            var taskStatuses = pages.CreateChildPermission(AppPermissions.Pages_TaskStatuses, L("TaskStatuses"));
            taskStatuses.CreateChildPermission(AppPermissions.Pages_TaskStatuses_Create, L("CreateNewTaskStatus"));
            taskStatuses.CreateChildPermission(AppPermissions.Pages_TaskStatuses_Edit, L("EditTaskStatus"));
            taskStatuses.CreateChildPermission(AppPermissions.Pages_TaskStatuses_Delete, L("DeleteTaskStatus"));

            var employees = pages.CreateChildPermission(AppPermissions.Pages_Employees, L("Employees"));
            employees.CreateChildPermission(AppPermissions.Pages_Employees_Create, L("CreateNewEmployee"));
            employees.CreateChildPermission(AppPermissions.Pages_Employees_Edit, L("EditEmployee"));
            employees.CreateChildPermission(AppPermissions.Pages_Employees_Delete, L("DeleteEmployee"));

            var jobTags = pages.CreateChildPermission(AppPermissions.Pages_JobTags, L("JobTags"));
            jobTags.CreateChildPermission(AppPermissions.Pages_JobTags_Create, L("CreateNewJobTag"));
            jobTags.CreateChildPermission(AppPermissions.Pages_JobTags_Edit, L("EditJobTag"));
            jobTags.CreateChildPermission(AppPermissions.Pages_JobTags_Delete, L("DeleteJobTag"));

            var jobs = pages.CreateChildPermission(AppPermissions.Pages_Jobs, L("Jobs"));
            jobs.CreateChildPermission(AppPermissions.Pages_Jobs_Create, L("CreateNewJob"));
            jobs.CreateChildPermission(AppPermissions.Pages_Jobs_Edit, L("EditJob"));
            jobs.CreateChildPermission(AppPermissions.Pages_Jobs_Delete, L("DeleteJob"));

            var jobStatusTypes = pages.CreateChildPermission(AppPermissions.Pages_JobStatusTypes, L("JobStatusTypes"));
            jobStatusTypes.CreateChildPermission(AppPermissions.Pages_JobStatusTypes_Create, L("CreateNewJobStatusType"));
            jobStatusTypes.CreateChildPermission(AppPermissions.Pages_JobStatusTypes_Edit, L("EditJobStatusType"));
            jobStatusTypes.CreateChildPermission(AppPermissions.Pages_JobStatusTypes_Delete, L("DeleteJobStatusType"));

            var storeTags = pages.CreateChildPermission(AppPermissions.Pages_StoreTags, L("StoreTags"));
            storeTags.CreateChildPermission(AppPermissions.Pages_StoreTags_Create, L("CreateNewStoreTag"));
            storeTags.CreateChildPermission(AppPermissions.Pages_StoreTags_Edit, L("EditStoreTag"));
            storeTags.CreateChildPermission(AppPermissions.Pages_StoreTags_Delete, L("DeleteStoreTag"));

            var productTags = pages.CreateChildPermission(AppPermissions.Pages_ProductTags, L("ProductTags"));
            productTags.CreateChildPermission(AppPermissions.Pages_ProductTags_Create, L("CreateNewProductTag"));
            productTags.CreateChildPermission(AppPermissions.Pages_ProductTags_Edit, L("EditProductTag"));
            productTags.CreateChildPermission(AppPermissions.Pages_ProductTags_Delete, L("DeleteProductTag"));

            var businessTags = pages.CreateChildPermission(AppPermissions.Pages_BusinessTags, L("BusinessTags"));
            businessTags.CreateChildPermission(AppPermissions.Pages_BusinessTags_Create, L("CreateNewBusinessTag"));
            businessTags.CreateChildPermission(AppPermissions.Pages_BusinessTags_Edit, L("EditBusinessTag"));
            businessTags.CreateChildPermission(AppPermissions.Pages_BusinessTags_Delete, L("DeleteBusinessTag"));

            var contactTags = pages.CreateChildPermission(AppPermissions.Pages_ContactTags, L("ContactTags"));
            contactTags.CreateChildPermission(AppPermissions.Pages_ContactTags_Create, L("CreateNewContactTag"));
            contactTags.CreateChildPermission(AppPermissions.Pages_ContactTags_Edit, L("EditContactTag"));
            contactTags.CreateChildPermission(AppPermissions.Pages_ContactTags_Delete, L("DeleteContactTag"));

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