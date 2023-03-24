import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { BusinessProductMapRoutingModule } from './businessProductMap-routing.module';
import { BusinessProductMapsComponent } from './businessProductMaps.component';
import { CreateOrEditBusinessProductMapModalComponent } from './create-or-edit-businessProductMap-modal.component';
import { ViewBusinessProductMapModalComponent } from './view-businessProductMap-modal.component';
import { BusinessProductMapBusinessLookupTableModalComponent } from './businessProductMap-business-lookup-table-modal.component';
import { BusinessProductMapProductLookupTableModalComponent } from './businessProductMap-product-lookup-table-modal.component';

@NgModule({
    declarations: [
        BusinessProductMapsComponent,
        CreateOrEditBusinessProductMapModalComponent,
        ViewBusinessProductMapModalComponent,

        BusinessProductMapBusinessLookupTableModalComponent,
        BusinessProductMapProductLookupTableModalComponent,
    ],
    imports: [AppSharedModule, BusinessProductMapRoutingModule, AdminSharedModule],
})
export class BusinessProductMapModule {}
