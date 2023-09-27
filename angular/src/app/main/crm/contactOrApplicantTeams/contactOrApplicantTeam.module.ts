import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {ContactOrApplicantTeamRoutingModule} from './contactOrApplicantTeam-routing.module';
import {ContactOrApplicantTeamsComponent} from './contactOrApplicantTeams.component';
import {CreateOrEditContactOrApplicantTeamModalComponent} from './create-or-edit-contactOrApplicantTeam-modal.component';
import {ViewContactOrApplicantTeamModalComponent} from './view-contactOrApplicantTeam-modal.component';
import {ContactOrApplicantTeamContactLookupTableModalComponent} from './contactOrApplicantTeam-contact-lookup-table-modal.component';
    					import {ContactOrApplicantTeamEmployeeLookupTableModalComponent} from './contactOrApplicantTeam-employee-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        ContactOrApplicantTeamsComponent,
        CreateOrEditContactOrApplicantTeamModalComponent,
        ViewContactOrApplicantTeamModalComponent,
        
    					ContactOrApplicantTeamContactLookupTableModalComponent,
    					ContactOrApplicantTeamEmployeeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, ContactOrApplicantTeamRoutingModule , AdminSharedModule ],
    
})
export class ContactOrApplicantTeamModule {
}
