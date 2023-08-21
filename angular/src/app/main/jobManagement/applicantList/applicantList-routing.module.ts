import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {
    ApplicantListComponent
} from "@app/main/jobManagement/applicantList/applicationListComponent/applicantList.component";
import {
    ApplicantDashboardComponent
} from "@app/main/jobManagement/applicantList/applicantDashboardComponent/applicantDashboard.component";

const routes: Routes = [
    {
        path: '',
        component: ApplicantListComponent,
        pathMatch: 'full',
    },
    {
        path: 'dashboard/:contactApplicantId',
        component: ApplicantDashboardComponent
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ApplicantListRoutingModule {
}
