import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { BusinessJobMapRoutingModule } from './businessJobMap-routing.module';
import { BusinessJobMapsComponent } from './businessJobMaps.component';
import { CreateOrEditBusinessJobMapModalComponent } from './create-or-edit-businessJobMap-modal.component';
import { ViewBusinessJobMapModalComponent } from './view-businessJobMap-modal.component';
import { BusinessJobMapBusinessLookupTableModalComponent } from './businessJobMap-business-lookup-table-modal.component';
import { BusinessJobMapJobLookupTableModalComponent } from './businessJobMap-job-lookup-table-modal.component';

@NgModule({
    declarations: [
        BusinessJobMapsComponent,
        CreateOrEditBusinessJobMapModalComponent,
        ViewBusinessJobMapModalComponent,

        BusinessJobMapBusinessLookupTableModalComponent,
        BusinessJobMapJobLookupTableModalComponent,
    ],
    imports: [AppSharedModule, BusinessJobMapRoutingModule, AdminSharedModule],
})
export class BusinessJobMapModule {}
