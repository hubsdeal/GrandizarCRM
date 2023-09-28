import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {HubJobRoutingModule} from './hubJob-routing.module';
import {HubJobsComponent} from './hubJobs.component';
import {CreateOrEditHubJobModalComponent} from './create-or-edit-hubJob-modal.component';
import {ViewHubJobModalComponent} from './view-hubJob-modal.component';
import {HubJobHubLookupTableModalComponent} from './hubJob-hub-lookup-table-modal.component';
    					import {HubJobJobLookupTableModalComponent} from './hubJob-job-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        HubJobsComponent,
        CreateOrEditHubJobModalComponent,
        ViewHubJobModalComponent,
        
    					HubJobHubLookupTableModalComponent,
    					HubJobJobLookupTableModalComponent,
    ],
    imports: [AppSharedModule, HubJobRoutingModule , AdminSharedModule ],
    
})
export class HubJobModule {
}
