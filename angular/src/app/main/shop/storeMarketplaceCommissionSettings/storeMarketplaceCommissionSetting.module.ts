import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreMarketplaceCommissionSettingRoutingModule } from './storeMarketplaceCommissionSetting-routing.module';
import { StoreMarketplaceCommissionSettingsComponent } from './storeMarketplaceCommissionSettings.component';
import { CreateOrEditStoreMarketplaceCommissionSettingModalComponent } from './create-or-edit-storeMarketplaceCommissionSetting-modal.component';
import { ViewStoreMarketplaceCommissionSettingModalComponent } from './view-storeMarketplaceCommissionSetting-modal.component';
import { StoreMarketplaceCommissionSettingStoreLookupTableModalComponent } from './storeMarketplaceCommissionSetting-store-lookup-table-modal.component';
import { StoreMarketplaceCommissionSettingMarketplaceCommissionTypeLookupTableModalComponent } from './storeMarketplaceCommissionSetting-marketplaceCommissionType-lookup-table-modal.component';
import { StoreMarketplaceCommissionSettingProductCategoryLookupTableModalComponent } from './storeMarketplaceCommissionSetting-productCategory-lookup-table-modal.component';
import { StoreMarketplaceCommissionSettingProductLookupTableModalComponent } from './storeMarketplaceCommissionSetting-product-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreMarketplaceCommissionSettingsComponent,
        CreateOrEditStoreMarketplaceCommissionSettingModalComponent,
        ViewStoreMarketplaceCommissionSettingModalComponent,

        StoreMarketplaceCommissionSettingStoreLookupTableModalComponent,
        StoreMarketplaceCommissionSettingMarketplaceCommissionTypeLookupTableModalComponent,
        StoreMarketplaceCommissionSettingProductCategoryLookupTableModalComponent,
        StoreMarketplaceCommissionSettingProductLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreMarketplaceCommissionSettingRoutingModule, AdminSharedModule],
    exports: [
        StoreMarketplaceCommissionSettingsComponent,
        CreateOrEditStoreMarketplaceCommissionSettingModalComponent,
        ViewStoreMarketplaceCommissionSettingModalComponent,

        StoreMarketplaceCommissionSettingStoreLookupTableModalComponent,
        StoreMarketplaceCommissionSettingMarketplaceCommissionTypeLookupTableModalComponent,
        StoreMarketplaceCommissionSettingProductCategoryLookupTableModalComponent,
        StoreMarketplaceCommissionSettingProductLookupTableModalComponent,
    ]
})
export class StoreMarketplaceCommissionSettingModule {}
