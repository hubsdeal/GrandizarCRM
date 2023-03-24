import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { BusinessTaskMapRoutingModule } from './businessTaskMap-routing.module';
import { BusinessTaskMapsComponent } from './businessTaskMaps.component';
import { CreateOrEditBusinessTaskMapModalComponent } from './create-or-edit-businessTaskMap-modal.component';
import { ViewBusinessTaskMapModalComponent } from './view-businessTaskMap-modal.component';
import { BusinessTaskMapBusinessLookupTableModalComponent } from './businessTaskMap-business-lookup-table-modal.component';
import { BusinessTaskMapTaskEventLookupTableModalComponent } from './businessTaskMap-taskEvent-lookup-table-modal.component';

@NgModule({
    declarations: [
        BusinessTaskMapsComponent,
        CreateOrEditBusinessTaskMapModalComponent,
        ViewBusinessTaskMapModalComponent,

        BusinessTaskMapBusinessLookupTableModalComponent,
        BusinessTaskMapTaskEventLookupTableModalComponent,
    ],
    imports: [AppSharedModule, BusinessTaskMapRoutingModule, AdminSharedModule],
})
export class BusinessTaskMapModule {}
