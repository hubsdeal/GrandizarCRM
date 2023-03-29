import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ProductSubscriptionMapRoutingModule } from './productSubscriptionMap-routing.module';
import { ProductSubscriptionMapsComponent } from './productSubscriptionMaps.component';
import { CreateOrEditProductSubscriptionMapModalComponent } from './create-or-edit-productSubscriptionMap-modal.component';
import { ViewProductSubscriptionMapModalComponent } from './view-productSubscriptionMap-modal.component';
import { ProductSubscriptionMapProductLookupTableModalComponent } from './productSubscriptionMap-product-lookup-table-modal.component';
import { ProductSubscriptionMapSubscriptionTypeLookupTableModalComponent } from './productSubscriptionMap-subscriptionType-lookup-table-modal.component';

@NgModule({
    declarations: [
        ProductSubscriptionMapsComponent,
        CreateOrEditProductSubscriptionMapModalComponent,
        ViewProductSubscriptionMapModalComponent,

        ProductSubscriptionMapProductLookupTableModalComponent,
        ProductSubscriptionMapSubscriptionTypeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ProductSubscriptionMapRoutingModule, AdminSharedModule],
})
export class ProductSubscriptionMapModule {}
