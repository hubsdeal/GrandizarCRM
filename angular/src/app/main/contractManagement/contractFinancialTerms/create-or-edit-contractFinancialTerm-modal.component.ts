import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ContractFinancialTermsServiceProxy, CreateOrEditContractFinancialTermDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ContractFinancialTermContractLookupTableModalComponent } from './contractFinancialTerm-contract-lookup-table-modal.component';
import { ContractFinancialTermHubLookupTableModalComponent } from './contractFinancialTerm-hub-lookup-table-modal.component';
import { ContractFinancialTermBusinessLookupTableModalComponent } from './contractFinancialTerm-business-lookup-table-modal.component';
import { ContractFinancialTermStoreLookupTableModalComponent } from './contractFinancialTerm-store-lookup-table-modal.component';
import { ContractFinancialTermJobLookupTableModalComponent } from './contractFinancialTerm-job-lookup-table-modal.component';
import { ContractFinancialTermProductCategoryLookupTableModalComponent } from './contractFinancialTerm-productCategory-lookup-table-modal.component';
import { ContractFinancialTermProductLookupTableModalComponent } from './contractFinancialTerm-product-lookup-table-modal.component';



@Component({
    selector: 'createOrEditContractFinancialTermModal',
    templateUrl: './create-or-edit-contractFinancialTerm-modal.component.html'
})
export class CreateOrEditContractFinancialTermModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('contractFinancialTermContractLookupTableModal', { static: true }) contractFinancialTermContractLookupTableModal: ContractFinancialTermContractLookupTableModalComponent;
    @ViewChild('contractFinancialTermHubLookupTableModal', { static: true }) contractFinancialTermHubLookupTableModal: ContractFinancialTermHubLookupTableModalComponent;
    @ViewChild('contractFinancialTermBusinessLookupTableModal', { static: true }) contractFinancialTermBusinessLookupTableModal: ContractFinancialTermBusinessLookupTableModalComponent;
    @ViewChild('contractFinancialTermStoreLookupTableModal', { static: true }) contractFinancialTermStoreLookupTableModal: ContractFinancialTermStoreLookupTableModalComponent;
    @ViewChild('contractFinancialTermJobLookupTableModal', { static: true }) contractFinancialTermJobLookupTableModal: ContractFinancialTermJobLookupTableModalComponent;
    @ViewChild('contractFinancialTermProductCategoryLookupTableModal', { static: true }) contractFinancialTermProductCategoryLookupTableModal: ContractFinancialTermProductCategoryLookupTableModalComponent;
    @ViewChild('contractFinancialTermProductLookupTableModal', { static: true }) contractFinancialTermProductLookupTableModal: ContractFinancialTermProductLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    contractFinancialTerm: CreateOrEditContractFinancialTermDto = new CreateOrEditContractFinancialTermDto();

    contractFirstPartyFullName = '';
    hubName = '';
    businessName = '';
    storeName = '';
    jobTitle = '';
    productCategoryName = '';
    productName = '';



    constructor(
        injector: Injector,
        private _contractFinancialTermsServiceProxy: ContractFinancialTermsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(contractFinancialTermId?: number): void {
    

        if (!contractFinancialTermId) {
            this.contractFinancialTerm = new CreateOrEditContractFinancialTermDto();
            this.contractFinancialTerm.id = contractFinancialTermId;
            this.contractFinancialTerm.startDate = this._dateTimeService.getStartOfDay();
            this.contractFinancialTerm.endDate = this._dateTimeService.getStartOfDay();
            this.contractFirstPartyFullName = '';
            this.hubName = '';
            this.businessName = '';
            this.storeName = '';
            this.jobTitle = '';
            this.productCategoryName = '';
            this.productName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._contractFinancialTermsServiceProxy.getContractFinancialTermForEdit(contractFinancialTermId).subscribe(result => {
                this.contractFinancialTerm = result.contractFinancialTerm;

                this.contractFirstPartyFullName = result.contractFirstPartyFullName;
                this.hubName = result.hubName;
                this.businessName = result.businessName;
                this.storeName = result.storeName;
                this.jobTitle = result.jobTitle;
                this.productCategoryName = result.productCategoryName;
                this.productName = result.productName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._contractFinancialTermsServiceProxy.createOrEdit(this.contractFinancialTerm)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectContractModal() {
        this.contractFinancialTermContractLookupTableModal.id = this.contractFinancialTerm.contractId;
        this.contractFinancialTermContractLookupTableModal.displayName = this.contractFirstPartyFullName;
        this.contractFinancialTermContractLookupTableModal.show();
    }
    openSelectHubModal() {
        this.contractFinancialTermHubLookupTableModal.id = this.contractFinancialTerm.hubId;
        this.contractFinancialTermHubLookupTableModal.displayName = this.hubName;
        this.contractFinancialTermHubLookupTableModal.show();
    }
    openSelectBusinessModal() {
        this.contractFinancialTermBusinessLookupTableModal.id = this.contractFinancialTerm.businessId;
        this.contractFinancialTermBusinessLookupTableModal.displayName = this.businessName;
        this.contractFinancialTermBusinessLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.contractFinancialTermStoreLookupTableModal.id = this.contractFinancialTerm.storeId;
        this.contractFinancialTermStoreLookupTableModal.displayName = this.storeName;
        this.contractFinancialTermStoreLookupTableModal.show();
    }
    openSelectJobModal() {
        this.contractFinancialTermJobLookupTableModal.id = this.contractFinancialTerm.jobId;
        this.contractFinancialTermJobLookupTableModal.displayName = this.jobTitle;
        this.contractFinancialTermJobLookupTableModal.show();
    }
    openSelectProductCategoryModal() {
        this.contractFinancialTermProductCategoryLookupTableModal.id = this.contractFinancialTerm.productCategoryId;
        this.contractFinancialTermProductCategoryLookupTableModal.displayName = this.productCategoryName;
        this.contractFinancialTermProductCategoryLookupTableModal.show();
    }
    openSelectProductModal() {
        this.contractFinancialTermProductLookupTableModal.id = this.contractFinancialTerm.productId;
        this.contractFinancialTermProductLookupTableModal.displayName = this.productName;
        this.contractFinancialTermProductLookupTableModal.show();
    }


    setContractIdNull() {
        this.contractFinancialTerm.contractId = null;
        this.contractFirstPartyFullName = '';
    }
    setHubIdNull() {
        this.contractFinancialTerm.hubId = null;
        this.hubName = '';
    }
    setBusinessIdNull() {
        this.contractFinancialTerm.businessId = null;
        this.businessName = '';
    }
    setStoreIdNull() {
        this.contractFinancialTerm.storeId = null;
        this.storeName = '';
    }
    setJobIdNull() {
        this.contractFinancialTerm.jobId = null;
        this.jobTitle = '';
    }
    setProductCategoryIdNull() {
        this.contractFinancialTerm.productCategoryId = null;
        this.productCategoryName = '';
    }
    setProductIdNull() {
        this.contractFinancialTerm.productId = null;
        this.productName = '';
    }


    getNewContractId() {
        this.contractFinancialTerm.contractId = this.contractFinancialTermContractLookupTableModal.id;
        this.contractFirstPartyFullName = this.contractFinancialTermContractLookupTableModal.displayName;
    }
    getNewHubId() {
        this.contractFinancialTerm.hubId = this.contractFinancialTermHubLookupTableModal.id;
        this.hubName = this.contractFinancialTermHubLookupTableModal.displayName;
    }
    getNewBusinessId() {
        this.contractFinancialTerm.businessId = this.contractFinancialTermBusinessLookupTableModal.id;
        this.businessName = this.contractFinancialTermBusinessLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.contractFinancialTerm.storeId = this.contractFinancialTermStoreLookupTableModal.id;
        this.storeName = this.contractFinancialTermStoreLookupTableModal.displayName;
    }
    getNewJobId() {
        this.contractFinancialTerm.jobId = this.contractFinancialTermJobLookupTableModal.id;
        this.jobTitle = this.contractFinancialTermJobLookupTableModal.displayName;
    }
    getNewProductCategoryId() {
        this.contractFinancialTerm.productCategoryId = this.contractFinancialTermProductCategoryLookupTableModal.id;
        this.productCategoryName = this.contractFinancialTermProductCategoryLookupTableModal.displayName;
    }
    getNewProductId() {
        this.contractFinancialTerm.productId = this.contractFinancialTermProductLookupTableModal.id;
        this.productName = this.contractFinancialTermProductLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
