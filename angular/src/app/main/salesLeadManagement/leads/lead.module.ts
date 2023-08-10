import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { LeadRoutingModule } from './lead-routing.module';
import { LeadsComponent } from './leads.component';
import { CreateOrEditLeadModalComponent } from './create-or-edit-lead-modal.component';
import { ViewLeadModalComponent } from './view-lead-modal.component';
import { LeadContactLookupTableModalComponent } from './lead-contact-lookup-table-modal.component';
import { LeadBusinessLookupTableModalComponent } from './lead-business-lookup-table-modal.component';
import { LeadProductLookupTableModalComponent } from './lead-product-lookup-table-modal.component';
import { LeadProductCategoryLookupTableModalComponent } from './lead-productCategory-lookup-table-modal.component';
import { LeadStoreLookupTableModalComponent } from './lead-store-lookup-table-modal.component';
import { LeadEmployeeLookupTableModalComponent } from './lead-employee-lookup-table-modal.component';
import { LeadLeadSourceLookupTableModalComponent } from './lead-leadSource-lookup-table-modal.component';
import { LeadLeadPipelineStageLookupTableModalComponent } from './lead-leadPipelineStage-lookup-table-modal.component';
import { LeadHubLookupTableModalComponent } from './lead-hub-lookup-table-modal.component';

@NgModule({
    declarations: [
        LeadsComponent,
        CreateOrEditLeadModalComponent,
        ViewLeadModalComponent,

        LeadContactLookupTableModalComponent,
        LeadBusinessLookupTableModalComponent,
        LeadProductLookupTableModalComponent,
        LeadProductCategoryLookupTableModalComponent,
        LeadStoreLookupTableModalComponent,
        LeadEmployeeLookupTableModalComponent,
        LeadLeadSourceLookupTableModalComponent,
        LeadLeadPipelineStageLookupTableModalComponent,
        LeadHubLookupTableModalComponent,
    ],
    imports: [AppSharedModule, LeadRoutingModule, AdminSharedModule],
    exports: [LeadsComponent,
        CreateOrEditLeadModalComponent,
        ViewLeadModalComponent,

        LeadContactLookupTableModalComponent,
        LeadBusinessLookupTableModalComponent,
        LeadProductLookupTableModalComponent,
        LeadProductCategoryLookupTableModalComponent,
        LeadStoreLookupTableModalComponent,
        LeadEmployeeLookupTableModalComponent,
        LeadLeadSourceLookupTableModalComponent,
        LeadLeadPipelineStageLookupTableModalComponent,
        LeadHubLookupTableModalComponent
    ]
})
export class LeadModule {}
