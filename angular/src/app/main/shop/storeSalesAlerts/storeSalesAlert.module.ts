import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StoreSalesAlertRoutingModule } from './storeSalesAlert-routing.module';
import { StoreSalesAlertsComponent } from './storeSalesAlerts.component';
import { CreateOrEditStoreSalesAlertModalComponent } from './create-or-edit-storeSalesAlert-modal.component';
import { ViewStoreSalesAlertModalComponent } from './view-storeSalesAlert-modal.component';
import { StoreSalesAlertStoreLookupTableModalComponent } from './storeSalesAlert-store-lookup-table-modal.component';

@NgModule({
    declarations: [
        StoreSalesAlertsComponent,
        CreateOrEditStoreSalesAlertModalComponent,
        ViewStoreSalesAlertModalComponent,

        StoreSalesAlertStoreLookupTableModalComponent,
    ],
    imports: [AppSharedModule, StoreSalesAlertRoutingModule, AdminSharedModule],
})
export class StoreSalesAlertModule {}
