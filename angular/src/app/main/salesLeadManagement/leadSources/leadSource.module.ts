import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { LeadSourceRoutingModule } from './leadSource-routing.module';
import { LeadSourcesComponent } from './leadSources.component';
import { CreateOrEditLeadSourceModalComponent } from './create-or-edit-leadSource-modal.component';
import { ViewLeadSourceModalComponent } from './view-leadSource-modal.component';

@NgModule({
    declarations: [LeadSourcesComponent, CreateOrEditLeadSourceModalComponent, ViewLeadSourceModalComponent],
    imports: [AppSharedModule, LeadSourceRoutingModule, AdminSharedModule],
})
export class LeadSourceModule {}
