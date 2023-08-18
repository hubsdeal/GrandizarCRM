import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {ApplicantListRoutingModule} from './applicantList-routing.module';
import {
    ApplicantListComponent
} from "@app/main/jobManagement/applicantList/applicationListComponent/applicantList.component";
import {SubheaderModule} from "@app/shared/common/sub-header/subheader.module";

@NgModule({
    declarations: [
        ApplicantListComponent
    ],
    imports: [
        AppSharedModule,
        ApplicantListRoutingModule,
        SubheaderModule
    ],
})
export class ApplicantListModule {}
