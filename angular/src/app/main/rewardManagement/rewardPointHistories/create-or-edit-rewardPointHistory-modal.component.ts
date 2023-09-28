import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { RewardPointHistoriesServiceProxy, CreateOrEditRewardPointHistoryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { RewardPointHistoryRewardPointTypeLookupTableModalComponent } from './rewardPointHistory-rewardPointType-lookup-table-modal.component';
import { RewardPointHistoryOrderLookupTableModalComponent } from './rewardPointHistory-order-lookup-table-modal.component';
import { RewardPointHistoryContactLookupTableModalComponent } from './rewardPointHistory-contact-lookup-table-modal.component';
import { RewardPointHistoryJobLookupTableModalComponent } from './rewardPointHistory-job-lookup-table-modal.component';



@Component({
    selector: 'createOrEditRewardPointHistoryModal',
    templateUrl: './create-or-edit-rewardPointHistory-modal.component.html'
})
export class CreateOrEditRewardPointHistoryModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('rewardPointHistoryRewardPointTypeLookupTableModal', { static: true }) rewardPointHistoryRewardPointTypeLookupTableModal: RewardPointHistoryRewardPointTypeLookupTableModalComponent;
    @ViewChild('rewardPointHistoryOrderLookupTableModal', { static: true }) rewardPointHistoryOrderLookupTableModal: RewardPointHistoryOrderLookupTableModalComponent;
    @ViewChild('rewardPointHistoryContactLookupTableModal', { static: true }) rewardPointHistoryContactLookupTableModal: RewardPointHistoryContactLookupTableModalComponent;
    @ViewChild('rewardPointHistoryJobLookupTableModal', { static: true }) rewardPointHistoryJobLookupTableModal: RewardPointHistoryJobLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    rewardPointHistory: CreateOrEditRewardPointHistoryDto = new CreateOrEditRewardPointHistoryDto();

    rewardPointTypeName = '';
    orderInvoiceNumber = '';
    contactFullName = '';
    jobTitle = '';



    constructor(
        injector: Injector,
        private _rewardPointHistoriesServiceProxy: RewardPointHistoriesServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(rewardPointHistoryId?: number): void {
    

        if (!rewardPointHistoryId) {
            this.rewardPointHistory = new CreateOrEditRewardPointHistoryDto();
            this.rewardPointHistory.id = rewardPointHistoryId;
            this.rewardPointHistory.date = this._dateTimeService.getStartOfDay();
            this.rewardPointTypeName = '';
            this.orderInvoiceNumber = '';
            this.contactFullName = '';
            this.jobTitle = '';


            this.active = true;
            this.modal.show();
        } else {
            this._rewardPointHistoriesServiceProxy.getRewardPointHistoryForEdit(rewardPointHistoryId).subscribe(result => {
                this.rewardPointHistory = result.rewardPointHistory;

                this.rewardPointTypeName = result.rewardPointTypeName;
                this.orderInvoiceNumber = result.orderInvoiceNumber;
                this.contactFullName = result.contactFullName;
                this.jobTitle = result.jobTitle;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._rewardPointHistoriesServiceProxy.createOrEdit(this.rewardPointHistory)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectRewardPointTypeModal() {
        this.rewardPointHistoryRewardPointTypeLookupTableModal.id = this.rewardPointHistory.rewardPointTypeId;
        this.rewardPointHistoryRewardPointTypeLookupTableModal.displayName = this.rewardPointTypeName;
        this.rewardPointHistoryRewardPointTypeLookupTableModal.show();
    }
    openSelectOrderModal() {
        this.rewardPointHistoryOrderLookupTableModal.id = this.rewardPointHistory.orderId;
        this.rewardPointHistoryOrderLookupTableModal.displayName = this.orderInvoiceNumber;
        this.rewardPointHistoryOrderLookupTableModal.show();
    }
    openSelectContactModal() {
        this.rewardPointHistoryContactLookupTableModal.id = this.rewardPointHistory.contactId;
        this.rewardPointHistoryContactLookupTableModal.displayName = this.contactFullName;
        this.rewardPointHistoryContactLookupTableModal.show();
    }
    openSelectJobModal() {
        this.rewardPointHistoryJobLookupTableModal.id = this.rewardPointHistory.jobId;
        this.rewardPointHistoryJobLookupTableModal.displayName = this.jobTitle;
        this.rewardPointHistoryJobLookupTableModal.show();
    }


    setRewardPointTypeIdNull() {
        this.rewardPointHistory.rewardPointTypeId = null;
        this.rewardPointTypeName = '';
    }
    setOrderIdNull() {
        this.rewardPointHistory.orderId = null;
        this.orderInvoiceNumber = '';
    }
    setContactIdNull() {
        this.rewardPointHistory.contactId = null;
        this.contactFullName = '';
    }
    setJobIdNull() {
        this.rewardPointHistory.jobId = null;
        this.jobTitle = '';
    }


    getNewRewardPointTypeId() {
        this.rewardPointHistory.rewardPointTypeId = this.rewardPointHistoryRewardPointTypeLookupTableModal.id;
        this.rewardPointTypeName = this.rewardPointHistoryRewardPointTypeLookupTableModal.displayName;
    }
    getNewOrderId() {
        this.rewardPointHistory.orderId = this.rewardPointHistoryOrderLookupTableModal.id;
        this.orderInvoiceNumber = this.rewardPointHistoryOrderLookupTableModal.displayName;
    }
    getNewContactId() {
        this.rewardPointHistory.contactId = this.rewardPointHistoryContactLookupTableModal.id;
        this.contactFullName = this.rewardPointHistoryContactLookupTableModal.displayName;
    }
    getNewJobId() {
        this.rewardPointHistory.jobId = this.rewardPointHistoryJobLookupTableModal.id;
        this.jobTitle = this.rewardPointHistoryJobLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
