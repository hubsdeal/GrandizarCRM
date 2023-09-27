import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {JobApplicantHireStatusTypeRoutingModule} from './jobApplicantHireStatusType-routing.module';
import {JobApplicantHireStatusTypesComponent} from './jobApplicantHireStatusTypes.component';
import {CreateOrEditJobApplicantHireStatusTypeModalComponent} from './create-or-edit-jobApplicantHireStatusType-modal.component';
import {ViewJobApplicantHireStatusTypeModalComponent} from './view-jobApplicantHireStatusType-modal.component';



@NgModule({
    declarations: [
        JobApplicantHireStatusTypesComponent,
        CreateOrEditJobApplicantHireStatusTypeModalComponent,
        ViewJobApplicantHireStatusTypeModalComponent,
        
    ],
    imports: [AppSharedModule, JobApplicantHireStatusTypeRoutingModule , AdminSharedModule ],
    
})
export class JobApplicantHireStatusTypeModule {
}
