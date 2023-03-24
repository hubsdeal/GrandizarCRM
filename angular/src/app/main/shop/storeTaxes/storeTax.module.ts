import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreTaxRoutingModule } from './storeTax-routing.module';
import { StoreTaxesComponent } from './storeTaxes.component';
import { CreateOrEditStoreTaxModalComponent } from './create-or-edit-storeTax-modal.component';
import { ViewStoreTaxModalComponent } from './view-storeTax-modal.component';
import { StoreTaxStoreLookupTableModalComponent } from './storeTax-store-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreTaxesComponent,
        CreateOrEditStoreTaxModalComponent,
        ViewStoreTaxModalComponent,

        StoreTaxStoreLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreTaxRoutingModule, AdminSharedModule],
})
export class StoreTaxModule {}
