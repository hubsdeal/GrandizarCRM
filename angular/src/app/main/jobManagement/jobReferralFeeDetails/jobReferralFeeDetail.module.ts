import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { JobReferralFeeDetailRoutingModule } from './jobReferralFeeDetail-routing.module';
import { JobReferralFeeDetailsComponent } from './jobReferralFeeDetails.component';
import { CreateOrEditJobReferralFeeDetailModalComponent } from './create-or-edit-jobReferralFeeDetail-modal.component';
import { ViewJobReferralFeeDetailModalComponent } from './view-jobReferralFeeDetail-modal.component';
import { JobReferralFeeDetailJobLookupTableModalComponent } from './jobReferralFeeDetail-job-lookup-table-modal.component';
import { JobReferralFeeDetailStoreLookupTableModalComponent } from './jobReferralFeeDetail-store-lookup-table-modal.component';
import { JobReferralFeeDetailBusinessLookupTableModalComponent } from './jobReferralFeeDetail-business-lookup-table-modal.component';
import { JobReferralFeeDetailOrderLookupTableModalComponent } from './jobReferralFeeDetail-order-lookup-table-modal.component';
import { JobReferralFeeDetailJobReferralFeeSplitPolicyLookupTableModalComponent } from './jobReferralFeeDetail-jobReferralFeeSplitPolicy-lookup-table-modal.component';
import { JobReferralFeeDetailCurrencyLookupTableModalComponent } from './jobReferralFeeDetail-currency-lookup-table-modal.component';
import { JobReferralFeeDetailContactLookupTableModalComponent } from './jobReferralFeeDetail-contact-lookup-table-modal.component';
import { JobReferralFeeDetailEmployeeLookupTableModalComponent } from './jobReferralFeeDetail-employee-lookup-table-modal.component';



@NgModule({
	declarations: [
		JobReferralFeeDetailsComponent,
		CreateOrEditJobReferralFeeDetailModalComponent,
		ViewJobReferralFeeDetailModalComponent,

		JobReferralFeeDetailJobLookupTableModalComponent,
		JobReferralFeeDetailStoreLookupTableModalComponent,
		JobReferralFeeDetailBusinessLookupTableModalComponent,
		JobReferralFeeDetailOrderLookupTableModalComponent,
		JobReferralFeeDetailJobReferralFeeSplitPolicyLookupTableModalComponent,
		JobReferralFeeDetailCurrencyLookupTableModalComponent,
		JobReferralFeeDetailContactLookupTableModalComponent,
		JobReferralFeeDetailEmployeeLookupTableModalComponent
	],
	imports: [AppSharedModule, JobReferralFeeDetailRoutingModule, AdminSharedModule],

})
export class JobReferralFeeDetailModule {
}
