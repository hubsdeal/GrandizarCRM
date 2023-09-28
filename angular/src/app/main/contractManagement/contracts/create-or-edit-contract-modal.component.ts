import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ContractsServiceProxy, CreateOrEditContractDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ContractContractTypeLookupTableModalComponent } from './contract-contractType-lookup-table-modal.component';
import { ContractStoreLookupTableModalComponent } from './contract-store-lookup-table-modal.component';
import { ContractBusinessLookupTableModalComponent } from './contract-business-lookup-table-modal.component';
import { ContractEmployeeLookupTableModalComponent } from './contract-employee-lookup-table-modal.component';
import { ContractJobLookupTableModalComponent } from './contract-job-lookup-table-modal.component';
import { ContractProductLookupTableModalComponent } from './contract-product-lookup-table-modal.component';
import { ContractHubLookupTableModalComponent } from './contract-hub-lookup-table-modal.component';
import { ContractContactLookupTableModalComponent } from './contract-contact-lookup-table-modal.component';



@Component({
    selector: 'createOrEditContractModal',
    templateUrl: './create-or-edit-contract-modal.component.html'
})
export class CreateOrEditContractModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('contractContractTypeLookupTableModal', { static: true }) contractContractTypeLookupTableModal: ContractContractTypeLookupTableModalComponent;
    @ViewChild('contractStoreLookupTableModal', { static: true }) contractStoreLookupTableModal: ContractStoreLookupTableModalComponent;
    @ViewChild('contractBusinessLookupTableModal', { static: true }) contractBusinessLookupTableModal: ContractBusinessLookupTableModalComponent;
    @ViewChild('contractEmployeeLookupTableModal', { static: true }) contractEmployeeLookupTableModal: ContractEmployeeLookupTableModalComponent;
    @ViewChild('contractJobLookupTableModal', { static: true }) contractJobLookupTableModal: ContractJobLookupTableModalComponent;
    @ViewChild('contractProductLookupTableModal', { static: true }) contractProductLookupTableModal: ContractProductLookupTableModalComponent;
    @ViewChild('contractHubLookupTableModal', { static: true }) contractHubLookupTableModal: ContractHubLookupTableModalComponent;
    @ViewChild('contractContactLookupTableModal', { static: true }) contractContactLookupTableModal: ContractContactLookupTableModalComponent;
    @ViewChild('contractContactLookupTableModal2', { static: true }) contractContactLookupTableModal2: ContractContactLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    contract: CreateOrEditContractDto = new CreateOrEditContractDto();

    contractTypeName = '';
    storeName = '';
    businessName = '';
    employeeName = '';
    jobTitle = '';
    productName = '';
    hubName = '';
    contactFullName = '';
    contactFullName2 = '';



    constructor(
        injector: Injector,
        private _contractsServiceProxy: ContractsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(contractId?: number): void {
    

        if (!contractId) {
            this.contract = new CreateOrEditContractDto();
            this.contract.id = contractId;
            this.contract.startDate = this._dateTimeService.getStartOfDay();
            this.contract.endDate = this._dateTimeService.getStartOfDay();
            this.contract.firstPartySignDate = this._dateTimeService.getStartOfDay();
            this.contract.secondPartySignDate = this._dateTimeService.getStartOfDay();
            this.contractTypeName = '';
            this.storeName = '';
            this.businessName = '';
            this.employeeName = '';
            this.jobTitle = '';
            this.productName = '';
            this.hubName = '';
            this.contactFullName = '';
            this.contactFullName2 = '';


            this.active = true;
            this.modal.show();
        } else {
            this._contractsServiceProxy.getContractForEdit(contractId).subscribe(result => {
                this.contract = result.contract;

                this.contractTypeName = result.contractTypeName;
                this.storeName = result.storeName;
                this.businessName = result.businessName;
                this.employeeName = result.employeeName;
                this.jobTitle = result.jobTitle;
                this.productName = result.productName;
                this.hubName = result.hubName;
                this.contactFullName = result.contactFullName;
                this.contactFullName2 = result.contactFullName2;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._contractsServiceProxy.createOrEdit(this.contract)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectContractTypeModal() {
        this.contractContractTypeLookupTableModal.id = this.contract.contractTypeId;
        this.contractContractTypeLookupTableModal.displayName = this.contractTypeName;
        this.contractContractTypeLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.contractStoreLookupTableModal.id = this.contract.storeId;
        this.contractStoreLookupTableModal.displayName = this.storeName;
        this.contractStoreLookupTableModal.show();
    }
    openSelectBusinessModal() {
        this.contractBusinessLookupTableModal.id = this.contract.businessId;
        this.contractBusinessLookupTableModal.displayName = this.businessName;
        this.contractBusinessLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.contractEmployeeLookupTableModal.id = this.contract.employeeId;
        this.contractEmployeeLookupTableModal.displayName = this.employeeName;
        this.contractEmployeeLookupTableModal.show();
    }
    openSelectJobModal() {
        this.contractJobLookupTableModal.id = this.contract.jobId;
        this.contractJobLookupTableModal.displayName = this.jobTitle;
        this.contractJobLookupTableModal.show();
    }
    openSelectProductModal() {
        this.contractProductLookupTableModal.id = this.contract.productId;
        this.contractProductLookupTableModal.displayName = this.productName;
        this.contractProductLookupTableModal.show();
    }
    openSelectHubModal() {
        this.contractHubLookupTableModal.id = this.contract.hubId;
        this.contractHubLookupTableModal.displayName = this.hubName;
        this.contractHubLookupTableModal.show();
    }
    openSelectContactModal() {
        this.contractContactLookupTableModal.id = this.contract.firstPartyContactId;
        this.contractContactLookupTableModal.displayName = this.contactFullName;
        this.contractContactLookupTableModal.show();
    }
    openSelectContactModal2() {
        this.contractContactLookupTableModal2.id = this.contract.secondPartyContactId;
        this.contractContactLookupTableModal2.displayName = this.contactFullName;
        this.contractContactLookupTableModal2.show();
    }


    setContractTypeIdNull() {
        this.contract.contractTypeId = null;
        this.contractTypeName = '';
    }
    setStoreIdNull() {
        this.contract.storeId = null;
        this.storeName = '';
    }
    setBusinessIdNull() {
        this.contract.businessId = null;
        this.businessName = '';
    }
    setEmployeeIdNull() {
        this.contract.employeeId = null;
        this.employeeName = '';
    }
    setJobIdNull() {
        this.contract.jobId = null;
        this.jobTitle = '';
    }
    setProductIdNull() {
        this.contract.productId = null;
        this.productName = '';
    }
    setHubIdNull() {
        this.contract.hubId = null;
        this.hubName = '';
    }
    setFirstPartyContactIdNull() {
        this.contract.firstPartyContactId = null;
        this.contactFullName = '';
    }
    setSecondPartyContactIdNull() {
        this.contract.secondPartyContactId = null;
        this.contactFullName2 = '';
    }


    getNewContractTypeId() {
        this.contract.contractTypeId = this.contractContractTypeLookupTableModal.id;
        this.contractTypeName = this.contractContractTypeLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.contract.storeId = this.contractStoreLookupTableModal.id;
        this.storeName = this.contractStoreLookupTableModal.displayName;
    }
    getNewBusinessId() {
        this.contract.businessId = this.contractBusinessLookupTableModal.id;
        this.businessName = this.contractBusinessLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.contract.employeeId = this.contractEmployeeLookupTableModal.id;
        this.employeeName = this.contractEmployeeLookupTableModal.displayName;
    }
    getNewJobId() {
        this.contract.jobId = this.contractJobLookupTableModal.id;
        this.jobTitle = this.contractJobLookupTableModal.displayName;
    }
    getNewProductId() {
        this.contract.productId = this.contractProductLookupTableModal.id;
        this.productName = this.contractProductLookupTableModal.displayName;
    }
    getNewHubId() {
        this.contract.hubId = this.contractHubLookupTableModal.id;
        this.hubName = this.contractHubLookupTableModal.displayName;
    }
    getNewFirstPartyContactId() {
        this.contract.firstPartyContactId = this.contractContactLookupTableModal.id;
        this.contactFullName = this.contractContactLookupTableModal.displayName;
    }
    getNewSecondPartyContactId() {
        this.contract.secondPartyContactId = this.contractContactLookupTableModal2.id;
        this.contactFullName2 = this.contractContactLookupTableModal2.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
