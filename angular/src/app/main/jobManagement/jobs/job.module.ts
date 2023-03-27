import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { JobRoutingModule } from './job-routing.module';
import { JobsComponent } from './jobs.component';
import { CreateOrEditJobModalComponent } from './create-or-edit-job-modal.component';
import { ViewJobModalComponent } from './view-job-modal.component';
import { JobMasterTagCategoryLookupTableModalComponent } from './job-masterTagCategory-lookup-table-modal.component';
import { JobMasterTagLookupTableModalComponent } from './job-masterTag-lookup-table-modal.component';
import { JobProductCategoryLookupTableModalComponent } from './job-productCategory-lookup-table-modal.component';
import { JobCurrencyLookupTableModalComponent } from './job-currency-lookup-table-modal.component';
import { JobBusinessLookupTableModalComponent } from './job-business-lookup-table-modal.component';
import { JobCountryLookupTableModalComponent } from './job-country-lookup-table-modal.component';
import { JobStateLookupTableModalComponent } from './job-state-lookup-table-modal.component';
import { JobCityLookupTableModalComponent } from './job-city-lookup-table-modal.component';
import { JobJobStatusTypeLookupTableModalComponent } from './job-jobStatusType-lookup-table-modal.component';
import { JobStoreLookupTableModalComponent } from './job-store-lookup-table-modal.component';
import { JobDashboardComponent } from './job-dashboard/job-dashboard.component';

@NgModule({
    declarations: [
        JobsComponent,
        CreateOrEditJobModalComponent,
        ViewJobModalComponent,

        JobMasterTagCategoryLookupTableModalComponent,
        JobMasterTagLookupTableModalComponent,
        JobProductCategoryLookupTableModalComponent,
        JobCurrencyLookupTableModalComponent,
        JobBusinessLookupTableModalComponent,
        JobCountryLookupTableModalComponent,
        JobStateLookupTableModalComponent,
        JobCityLookupTableModalComponent,
        JobJobStatusTypeLookupTableModalComponent,
        JobStoreLookupTableModalComponent,
        JobDashboardComponent,
    ],
    imports: [AppSharedModule, JobRoutingModule, AdminSharedModule],
})
export class JobModule {}
