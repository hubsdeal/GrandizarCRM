import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {JobApplicantHireMatrixRoutingModule} from './jobApplicantHireMatrix-routing.module';
import {JobApplicantHireMatrixesComponent} from './jobApplicantHireMatrixes.component';
import {CreateOrEditJobApplicantHireMatrixModalComponent} from './create-or-edit-jobApplicantHireMatrix-modal.component';
import {ViewJobApplicantHireMatrixModalComponent} from './view-jobApplicantHireMatrix-modal.component';
import {JobApplicantHireMatrixJobLookupTableModalComponent} from './jobApplicantHireMatrix-job-lookup-table-modal.component';
    					import {JobApplicantHireMatrixContactLookupTableModalComponent} from './jobApplicantHireMatrix-contact-lookup-table-modal.component';
    					import {JobApplicantHireMatrixJobHiringTeamLookupTableModalComponent} from './jobApplicantHireMatrix-jobHiringTeam-lookup-table-modal.component';
    					import {JobApplicantHireMatrixJobApplicantHireStatusTypeLookupTableModalComponent} from './jobApplicantHireMatrix-jobApplicantHireStatusType-lookup-table-modal.component';
    					


@NgModule({
    declarations: [
        JobApplicantHireMatrixesComponent,
        CreateOrEditJobApplicantHireMatrixModalComponent,
        ViewJobApplicantHireMatrixModalComponent,
        
    					JobApplicantHireMatrixJobLookupTableModalComponent,
    					JobApplicantHireMatrixContactLookupTableModalComponent,
    					JobApplicantHireMatrixJobHiringTeamLookupTableModalComponent,
    					JobApplicantHireMatrixJobApplicantHireStatusTypeLookupTableModalComponent,
    ],
    imports: [AppSharedModule, JobApplicantHireMatrixRoutingModule , AdminSharedModule ],
    
})
export class JobApplicantHireMatrixModule {
}
