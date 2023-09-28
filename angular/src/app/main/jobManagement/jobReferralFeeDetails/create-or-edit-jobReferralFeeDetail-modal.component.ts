import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { JobReferralFeeDetailsServiceProxy, CreateOrEditJobReferralFeeDetailDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { JobReferralFeeDetailJobLookupTableModalComponent } from './jobReferralFeeDetail-job-lookup-table-modal.component';
import { JobReferralFeeDetailStoreLookupTableModalComponent } from './jobReferralFeeDetail-store-lookup-table-modal.component';
import { JobReferralFeeDetailBusinessLookupTableModalComponent } from './jobReferralFeeDetail-business-lookup-table-modal.component';
import { JobReferralFeeDetailOrderLookupTableModalComponent } from './jobReferralFeeDetail-order-lookup-table-modal.component';
import { JobReferralFeeDetailJobReferralFeeSplitPolicyLookupTableModalComponent } from './jobReferralFeeDetail-jobReferralFeeSplitPolicy-lookup-table-modal.component';
import { JobReferralFeeDetailCurrencyLookupTableModalComponent } from './jobReferralFeeDetail-currency-lookup-table-modal.component';
import { JobReferralFeeDetailContactLookupTableModalComponent } from './jobReferralFeeDetail-contact-lookup-table-modal.component';
import { JobReferralFeeDetailEmployeeLookupTableModalComponent } from './jobReferralFeeDetail-employee-lookup-table-modal.component';



@Component({
    selector: 'createOrEditJobReferralFeeDetailModal',
    templateUrl: './create-or-edit-jobReferralFeeDetail-modal.component.html'
})
export class CreateOrEditJobReferralFeeDetailModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('jobReferralFeeDetailJobLookupTableModal', { static: true }) jobReferralFeeDetailJobLookupTableModal: JobReferralFeeDetailJobLookupTableModalComponent;
    @ViewChild('jobReferralFeeDetailStoreLookupTableModal', { static: true }) jobReferralFeeDetailStoreLookupTableModal: JobReferralFeeDetailStoreLookupTableModalComponent;
    @ViewChild('jobReferralFeeDetailBusinessLookupTableModal', { static: true }) jobReferralFeeDetailBusinessLookupTableModal: JobReferralFeeDetailBusinessLookupTableModalComponent;
    @ViewChild('jobReferralFeeDetailOrderLookupTableModal', { static: true }) jobReferralFeeDetailOrderLookupTableModal: JobReferralFeeDetailOrderLookupTableModalComponent;
    @ViewChild('jobReferralFeeDetailJobReferralFeeSplitPolicyLookupTableModal', { static: true }) jobReferralFeeDetailJobReferralFeeSplitPolicyLookupTableModal: JobReferralFeeDetailJobReferralFeeSplitPolicyLookupTableModalComponent;
    @ViewChild('jobReferralFeeDetailCurrencyLookupTableModal', { static: true }) jobReferralFeeDetailCurrencyLookupTableModal: JobReferralFeeDetailCurrencyLookupTableModalComponent;
    @ViewChild('jobReferralFeeDetailContactLookupTableModal', { static: true }) jobReferralFeeDetailContactLookupTableModal: JobReferralFeeDetailContactLookupTableModalComponent;
    @ViewChild('jobReferralFeeDetailContactLookupTableModal2', { static: true }) jobReferralFeeDetailContactLookupTableModal2: JobReferralFeeDetailContactLookupTableModalComponent;
    @ViewChild('jobReferralFeeDetailEmployeeLookupTableModal', { static: true }) jobReferralFeeDetailEmployeeLookupTableModal: JobReferralFeeDetailEmployeeLookupTableModalComponent;
    @ViewChild('jobReferralFeeDetailEmployeeLookupTableModal2', { static: true }) jobReferralFeeDetailEmployeeLookupTableModal2: JobReferralFeeDetailEmployeeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    jobReferralFeeDetail: CreateOrEditJobReferralFeeDetailDto = new CreateOrEditJobReferralFeeDetailDto();

    jobTitle = '';
    storeName = '';
    businessName = '';
    orderInvoiceNumber = '';
    jobReferralFeeSplitPolicyTitleName = '';
    currencyName = '';
    contactFullName = '';
    contactFullName2 = '';
    employeeName = '';
    employeeName2 = '';



    constructor(
        injector: Injector,
        private _jobReferralFeeDetailsServiceProxy: JobReferralFeeDetailsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(jobReferralFeeDetailId?: number): void {
    

        if (!jobReferralFeeDetailId) {
            this.jobReferralFeeDetail = new CreateOrEditJobReferralFeeDetailDto();
            this.jobReferralFeeDetail.id = jobReferralFeeDetailId;
            this.jobTitle = '';
            this.storeName = '';
            this.businessName = '';
            this.orderInvoiceNumber = '';
            this.jobReferralFeeSplitPolicyTitleName = '';
            this.currencyName = '';
            this.contactFullName = '';
            this.contactFullName2 = '';
            this.employeeName = '';
            this.employeeName2 = '';


            this.active = true;
            this.modal.show();
        } else {
            this._jobReferralFeeDetailsServiceProxy.getJobReferralFeeDetailForEdit(jobReferralFeeDetailId).subscribe(result => {
                this.jobReferralFeeDetail = result.jobReferralFeeDetail;

                this.jobTitle = result.jobTitle;
                this.storeName = result.storeName;
                this.businessName = result.businessName;
                this.orderInvoiceNumber = result.orderInvoiceNumber;
                this.jobReferralFeeSplitPolicyTitleName = result.jobReferralFeeSplitPolicyTitleName;
                this.currencyName = result.currencyName;
                this.contactFullName = result.contactFullName;
                this.contactFullName2 = result.contactFullName2;
                this.employeeName = result.employeeName;
                this.employeeName2 = result.employeeName2;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._jobReferralFeeDetailsServiceProxy.createOrEdit(this.jobReferralFeeDetail)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectJobModal() {
        this.jobReferralFeeDetailJobLookupTableModal.id = this.jobReferralFeeDetail.jobId;
        this.jobReferralFeeDetailJobLookupTableModal.displayName = this.jobTitle;
        this.jobReferralFeeDetailJobLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.jobReferralFeeDetailStoreLookupTableModal.id = this.jobReferralFeeDetail.storeId;
        this.jobReferralFeeDetailStoreLookupTableModal.displayName = this.storeName;
        this.jobReferralFeeDetailStoreLookupTableModal.show();
    }
    openSelectBusinessModal() {
        this.jobReferralFeeDetailBusinessLookupTableModal.id = this.jobReferralFeeDetail.businessId;
        this.jobReferralFeeDetailBusinessLookupTableModal.displayName = this.businessName;
        this.jobReferralFeeDetailBusinessLookupTableModal.show();
    }
    openSelectOrderModal() {
        this.jobReferralFeeDetailOrderLookupTableModal.id = this.jobReferralFeeDetail.orderId;
        this.jobReferralFeeDetailOrderLookupTableModal.displayName = this.orderInvoiceNumber;
        this.jobReferralFeeDetailOrderLookupTableModal.show();
    }
    openSelectJobReferralFeeSplitPolicyModal() {
        this.jobReferralFeeDetailJobReferralFeeSplitPolicyLookupTableModal.id = this.jobReferralFeeDetail.jobReferralFeeSplitPolicyId;
        this.jobReferralFeeDetailJobReferralFeeSplitPolicyLookupTableModal.displayName = this.jobReferralFeeSplitPolicyTitleName;
        this.jobReferralFeeDetailJobReferralFeeSplitPolicyLookupTableModal.show();
    }
    openSelectCurrencyModal() {
        this.jobReferralFeeDetailCurrencyLookupTableModal.id = this.jobReferralFeeDetail.currencyId;
        this.jobReferralFeeDetailCurrencyLookupTableModal.displayName = this.currencyName;
        this.jobReferralFeeDetailCurrencyLookupTableModal.show();
    }
    openSelectContactModal() {
        this.jobReferralFeeDetailContactLookupTableModal.id = this.jobReferralFeeDetail.applicantContactId;
        this.jobReferralFeeDetailContactLookupTableModal.displayName = this.contactFullName;
        this.jobReferralFeeDetailContactLookupTableModal.show();
    }
    openSelectContactModal2() {
        this.jobReferralFeeDetailContactLookupTableModal2.id = this.jobReferralFeeDetail.referralContactId;
        this.jobReferralFeeDetailContactLookupTableModal2.displayName = this.contactFullName;
        this.jobReferralFeeDetailContactLookupTableModal2.show();
    }
    openSelectEmployeeModal() {
        this.jobReferralFeeDetailEmployeeLookupTableModal.id = this.jobReferralFeeDetail.hubPartnerEmployeeId;
        this.jobReferralFeeDetailEmployeeLookupTableModal.displayName = this.employeeName;
        this.jobReferralFeeDetailEmployeeLookupTableModal.show();
    }
    openSelectEmployeeModal2() {
        this.jobReferralFeeDetailEmployeeLookupTableModal2.id = this.jobReferralFeeDetail.employeeId;
        this.jobReferralFeeDetailEmployeeLookupTableModal2.displayName = this.employeeName;
        this.jobReferralFeeDetailEmployeeLookupTableModal2.show();
    }


    setJobIdNull() {
        this.jobReferralFeeDetail.jobId = null;
        this.jobTitle = '';
    }
    setStoreIdNull() {
        this.jobReferralFeeDetail.storeId = null;
        this.storeName = '';
    }
    setBusinessIdNull() {
        this.jobReferralFeeDetail.businessId = null;
        this.businessName = '';
    }
    setOrderIdNull() {
        this.jobReferralFeeDetail.orderId = null;
        this.orderInvoiceNumber = '';
    }
    setJobReferralFeeSplitPolicyIdNull() {
        this.jobReferralFeeDetail.jobReferralFeeSplitPolicyId = null;
        this.jobReferralFeeSplitPolicyTitleName = '';
    }
    setCurrencyIdNull() {
        this.jobReferralFeeDetail.currencyId = null;
        this.currencyName = '';
    }
    setApplicantContactIdNull() {
        this.jobReferralFeeDetail.applicantContactId = null;
        this.contactFullName = '';
    }
    setReferralContactIdNull() {
        this.jobReferralFeeDetail.referralContactId = null;
        this.contactFullName2 = '';
    }
    setHubPartnerEmployeeIdNull() {
        this.jobReferralFeeDetail.hubPartnerEmployeeId = null;
        this.employeeName = '';
    }
    setEmployeeIdNull() {
        this.jobReferralFeeDetail.employeeId = null;
        this.employeeName2 = '';
    }


    getNewJobId() {
        this.jobReferralFeeDetail.jobId = this.jobReferralFeeDetailJobLookupTableModal.id;
        this.jobTitle = this.jobReferralFeeDetailJobLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.jobReferralFeeDetail.storeId = this.jobReferralFeeDetailStoreLookupTableModal.id;
        this.storeName = this.jobReferralFeeDetailStoreLookupTableModal.displayName;
    }
    getNewBusinessId() {
        this.jobReferralFeeDetail.businessId = this.jobReferralFeeDetailBusinessLookupTableModal.id;
        this.businessName = this.jobReferralFeeDetailBusinessLookupTableModal.displayName;
    }
    getNewOrderId() {
        this.jobReferralFeeDetail.orderId = this.jobReferralFeeDetailOrderLookupTableModal.id;
        this.orderInvoiceNumber = this.jobReferralFeeDetailOrderLookupTableModal.displayName;
    }
    getNewJobReferralFeeSplitPolicyId() {
        this.jobReferralFeeDetail.jobReferralFeeSplitPolicyId = this.jobReferralFeeDetailJobReferralFeeSplitPolicyLookupTableModal.id;
        this.jobReferralFeeSplitPolicyTitleName = this.jobReferralFeeDetailJobReferralFeeSplitPolicyLookupTableModal.displayName;
    }
    getNewCurrencyId() {
        this.jobReferralFeeDetail.currencyId = this.jobReferralFeeDetailCurrencyLookupTableModal.id;
        this.currencyName = this.jobReferralFeeDetailCurrencyLookupTableModal.displayName;
    }
    getNewApplicantContactId() {
        this.jobReferralFeeDetail.applicantContactId = this.jobReferralFeeDetailContactLookupTableModal.id;
        this.contactFullName = this.jobReferralFeeDetailContactLookupTableModal.displayName;
    }
    getNewReferralContactId() {
        this.jobReferralFeeDetail.referralContactId = this.jobReferralFeeDetailContactLookupTableModal2.id;
        this.contactFullName2 = this.jobReferralFeeDetailContactLookupTableModal2.displayName;
    }
    getNewHubPartnerEmployeeId() {
        this.jobReferralFeeDetail.hubPartnerEmployeeId = this.jobReferralFeeDetailEmployeeLookupTableModal.id;
        this.employeeName = this.jobReferralFeeDetailEmployeeLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.jobReferralFeeDetail.employeeId = this.jobReferralFeeDetailEmployeeLookupTableModal2.id;
        this.employeeName2 = this.jobReferralFeeDetailEmployeeLookupTableModal2.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
