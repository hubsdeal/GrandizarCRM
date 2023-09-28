import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {JobHiringTeamRoutingModule} from './jobHiringTeam-routing.module';
import {JobHiringTeamsComponent} from './jobHiringTeams.component';
import {CreateOrEditJobHiringTeamModalComponent} from './create-or-edit-jobHiringTeam-modal.component';
import {ViewJobHiringTeamModalComponent} from './view-jobHiringTeam-modal.component';
import {JobHiringTeamBusinessLookupTableModalComponent} from './jobHiringTeam-business-lookup-table-modal.component';
    					import {JobHiringTeamContactLookupTableModalComponent} from './jobHiringTeam-contact-lookup-table-modal.component';
    					import {JobHiringTeamEmployeeLookupTableModalComponent} from './jobHiringTeam-employee-lookup-table-modal.component';
    					import {JobHiringTeamJobLookupTableModalComponent} from './jobHiringTeam-job-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        JobHiringTeamsComponent,
        CreateOrEditJobHiringTeamModalComponent,
        ViewJobHiringTeamModalComponent,
        
    					JobHiringTeamBusinessLookupTableModalComponent,
    					JobHiringTeamContactLookupTableModalComponent,
    					JobHiringTeamEmployeeLookupTableModalComponent,
    					JobHiringTeamJobLookupTableModalComponent,
    ],
    imports: [AppSharedModule, JobHiringTeamRoutingModule , AdminSharedModule ],
    
})
export class JobHiringTeamModule {
}
