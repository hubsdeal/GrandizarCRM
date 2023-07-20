import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { HubProductRoutingModule } from './hubProduct-routing.module';
import { HubProductsComponent } from './hubProducts.component';
import { CreateOrEditHubProductModalComponent } from './create-or-edit-hubProduct-modal.component';
import { ViewHubProductModalComponent } from './view-hubProduct-modal.component';
import { HubProductHubLookupTableModalComponent } from './hubProduct-hub-lookup-table-modal.component';
import { HubProductProductLookupTableModalComponent } from './hubProduct-product-lookup-table-modal.component';

@NgModule({
    declarations: [
        HubProductsComponent,
        CreateOrEditHubProductModalComponent,
        ViewHubProductModalComponent,

        HubProductHubLookupTableModalComponent,
        HubProductProductLookupTableModalComponent,
    ],
    imports: [AppSharedModule, HubProductRoutingModule, AdminSharedModule],
    exports: [HubProductsComponent, CreateOrEditHubProductModalComponent, ViewHubProductModalComponent],
})
export class HubProductModule {}
