import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { HubBusinessRoutingModule } from './hubBusiness-routing.module';
import { HubBusinessesComponent } from './hubBusinesses.component';
import { CreateOrEditHubBusinessModalComponent } from './create-or-edit-hubBusiness-modal.component';
import { ViewHubBusinessModalComponent } from './view-hubBusiness-modal.component';
import { HubBusinessHubLookupTableModalComponent } from './hubBusiness-hub-lookup-table-modal.component';
import { HubBusinessBusinessLookupTableModalComponent } from './hubBusiness-business-lookup-table-modal.component';

@NgModule({
    declarations: [
        HubBusinessesComponent,
        CreateOrEditHubBusinessModalComponent,
        ViewHubBusinessModalComponent,

        HubBusinessHubLookupTableModalComponent,
        HubBusinessBusinessLookupTableModalComponent,
    ],
    imports: [AppSharedModule, HubBusinessRoutingModule, AdminSharedModule],
})
export class HubBusinessModule {}
