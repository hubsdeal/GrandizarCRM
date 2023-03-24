import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { BusinessContactMapRoutingModule } from './businessContactMap-routing.module';
import { BusinessContactMapsComponent } from './businessContactMaps.component';
import { CreateOrEditBusinessContactMapModalComponent } from './create-or-edit-businessContactMap-modal.component';
import { ViewBusinessContactMapModalComponent } from './view-businessContactMap-modal.component';
import { BusinessContactMapBusinessLookupTableModalComponent } from './businessContactMap-business-lookup-table-modal.component';
import { BusinessContactMapContactLookupTableModalComponent } from './businessContactMap-contact-lookup-table-modal.component';

@NgModule({
    declarations: [
        BusinessContactMapsComponent,
        CreateOrEditBusinessContactMapModalComponent,
        ViewBusinessContactMapModalComponent,

        BusinessContactMapBusinessLookupTableModalComponent,
        BusinessContactMapContactLookupTableModalComponent,
    ],
    imports: [AppSharedModule, BusinessContactMapRoutingModule, AdminSharedModule],
})
export class BusinessContactMapModule {}
