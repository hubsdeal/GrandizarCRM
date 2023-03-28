import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { HubSalesProjectionRoutingModule } from './hubSalesProjection-routing.module';
import { HubSalesProjectionsComponent } from './hubSalesProjections.component';
import { CreateOrEditHubSalesProjectionModalComponent } from './create-or-edit-hubSalesProjection-modal.component';
import { ViewHubSalesProjectionModalComponent } from './view-hubSalesProjection-modal.component';
import { HubSalesProjectionHubLookupTableModalComponent } from './hubSalesProjection-hub-lookup-table-modal.component';
import { HubSalesProjectionProductCategoryLookupTableModalComponent } from './hubSalesProjection-productCategory-lookup-table-modal.component';
import { HubSalesProjectionStoreLookupTableModalComponent } from './hubSalesProjection-store-lookup-table-modal.component';
import { HubSalesProjectionCurrencyLookupTableModalComponent } from './hubSalesProjection-currency-lookup-table-modal.component';

@NgModule({
    declarations: [
        HubSalesProjectionsComponent,
        CreateOrEditHubSalesProjectionModalComponent,
        ViewHubSalesProjectionModalComponent,

        HubSalesProjectionHubLookupTableModalComponent,
        HubSalesProjectionProductCategoryLookupTableModalComponent,
        HubSalesProjectionStoreLookupTableModalComponent,
        HubSalesProjectionCurrencyLookupTableModalComponent,
    ],
    imports: [AppSharedModule, HubSalesProjectionRoutingModule, AdminSharedModule],
})
export class HubSalesProjectionModule {}
