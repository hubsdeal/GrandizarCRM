import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProductGigWorkerPortfoliosServiceProxy, CreateOrEditProductGigWorkerPortfolioDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductGigWorkerPortfolioBusinessLookupTableModalComponent } from './productGigWorkerPortfolio-business-lookup-table-modal.component';
import { ProductGigWorkerPortfolioContactLookupTableModalComponent } from './productGigWorkerPortfolio-contact-lookup-table-modal.component';
import { ProductGigWorkerPortfolioProductLookupTableModalComponent } from './productGigWorkerPortfolio-product-lookup-table-modal.component';
import { ProductGigWorkerPortfolioEmployeeLookupTableModalComponent } from './productGigWorkerPortfolio-employee-lookup-table-modal.component';



@Component({
    selector: 'createOrEditProductGigWorkerPortfolioModal',
    templateUrl: './create-or-edit-productGigWorkerPortfolio-modal.component.html'
})
export class CreateOrEditProductGigWorkerPortfolioModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productGigWorkerPortfolioBusinessLookupTableModal', { static: true }) productGigWorkerPortfolioBusinessLookupTableModal: ProductGigWorkerPortfolioBusinessLookupTableModalComponent;
    @ViewChild('productGigWorkerPortfolioContactLookupTableModal', { static: true }) productGigWorkerPortfolioContactLookupTableModal: ProductGigWorkerPortfolioContactLookupTableModalComponent;
    @ViewChild('productGigWorkerPortfolioProductLookupTableModal', { static: true }) productGigWorkerPortfolioProductLookupTableModal: ProductGigWorkerPortfolioProductLookupTableModalComponent;
    @ViewChild('productGigWorkerPortfolioEmployeeLookupTableModal', { static: true }) productGigWorkerPortfolioEmployeeLookupTableModal: ProductGigWorkerPortfolioEmployeeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productGigWorkerPortfolio: CreateOrEditProductGigWorkerPortfolioDto = new CreateOrEditProductGigWorkerPortfolioDto();

    businessName = '';
    contactFullName = '';
    productName = '';
    employeeName = '';



    constructor(
        injector: Injector,
        private _productGigWorkerPortfoliosServiceProxy: ProductGigWorkerPortfoliosServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(productGigWorkerPortfolioId?: number): void {
    

        if (!productGigWorkerPortfolioId) {
            this.productGigWorkerPortfolio = new CreateOrEditProductGigWorkerPortfolioDto();
            this.productGigWorkerPortfolio.id = productGigWorkerPortfolioId;
            this.businessName = '';
            this.contactFullName = '';
            this.productName = '';
            this.employeeName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._productGigWorkerPortfoliosServiceProxy.getProductGigWorkerPortfolioForEdit(productGigWorkerPortfolioId).subscribe(result => {
                this.productGigWorkerPortfolio = result.productGigWorkerPortfolio;

                this.businessName = result.businessName;
                this.contactFullName = result.contactFullName;
                this.productName = result.productName;
                this.employeeName = result.employeeName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._productGigWorkerPortfoliosServiceProxy.createOrEdit(this.productGigWorkerPortfolio)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectBusinessModal() {
        this.productGigWorkerPortfolioBusinessLookupTableModal.id = this.productGigWorkerPortfolio.businessId;
        this.productGigWorkerPortfolioBusinessLookupTableModal.displayName = this.businessName;
        this.productGigWorkerPortfolioBusinessLookupTableModal.show();
    }
    openSelectContactModal() {
        this.productGigWorkerPortfolioContactLookupTableModal.id = this.productGigWorkerPortfolio.contactId;
        this.productGigWorkerPortfolioContactLookupTableModal.displayName = this.contactFullName;
        this.productGigWorkerPortfolioContactLookupTableModal.show();
    }
    openSelectProductModal() {
        this.productGigWorkerPortfolioProductLookupTableModal.id = this.productGigWorkerPortfolio.productId;
        this.productGigWorkerPortfolioProductLookupTableModal.displayName = this.productName;
        this.productGigWorkerPortfolioProductLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.productGigWorkerPortfolioEmployeeLookupTableModal.id = this.productGigWorkerPortfolio.employeeId;
        this.productGigWorkerPortfolioEmployeeLookupTableModal.displayName = this.employeeName;
        this.productGigWorkerPortfolioEmployeeLookupTableModal.show();
    }


    setBusinessIdNull() {
        this.productGigWorkerPortfolio.businessId = null;
        this.businessName = '';
    }
    setContactIdNull() {
        this.productGigWorkerPortfolio.contactId = null;
        this.contactFullName = '';
    }
    setProductIdNull() {
        this.productGigWorkerPortfolio.productId = null;
        this.productName = '';
    }
    setEmployeeIdNull() {
        this.productGigWorkerPortfolio.employeeId = null;
        this.employeeName = '';
    }


    getNewBusinessId() {
        this.productGigWorkerPortfolio.businessId = this.productGigWorkerPortfolioBusinessLookupTableModal.id;
        this.businessName = this.productGigWorkerPortfolioBusinessLookupTableModal.displayName;
    }
    getNewContactId() {
        this.productGigWorkerPortfolio.contactId = this.productGigWorkerPortfolioContactLookupTableModal.id;
        this.contactFullName = this.productGigWorkerPortfolioContactLookupTableModal.displayName;
    }
    getNewProductId() {
        this.productGigWorkerPortfolio.productId = this.productGigWorkerPortfolioProductLookupTableModal.id;
        this.productName = this.productGigWorkerPortfolioProductLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.productGigWorkerPortfolio.employeeId = this.productGigWorkerPortfolioEmployeeLookupTableModal.id;
        this.employeeName = this.productGigWorkerPortfolioEmployeeLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
