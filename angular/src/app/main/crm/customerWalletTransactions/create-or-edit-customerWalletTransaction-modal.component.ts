import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { CustomerWalletTransactionsServiceProxy, CreateOrEditCustomerWalletTransactionDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CustomerWalletTransactionOrderLookupTableModalComponent } from './customerWalletTransaction-order-lookup-table-modal.component';



@Component({
    selector: 'createOrEditCustomerWalletTransactionModal',
    templateUrl: './create-or-edit-customerWalletTransaction-modal.component.html'
})
export class CreateOrEditCustomerWalletTransactionModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('customerWalletTransactionOrderLookupTableModal', { static: true }) customerWalletTransactionOrderLookupTableModal: CustomerWalletTransactionOrderLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    customerWalletTransaction: CreateOrEditCustomerWalletTransactionDto = new CreateOrEditCustomerWalletTransactionDto();

    orderInvoiceNumber = '';



    constructor(
        injector: Injector,
        private _customerWalletTransactionsServiceProxy: CustomerWalletTransactionsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(customerWalletTransactionId?: number): void {
    

        if (!customerWalletTransactionId) {
            this.customerWalletTransaction = new CreateOrEditCustomerWalletTransactionDto();
            this.customerWalletTransaction.id = customerWalletTransactionId;
            this.customerWalletTransaction.transactionDate = this._dateTimeService.getStartOfDay();
            this.orderInvoiceNumber = '';


            this.active = true;
            this.modal.show();
        } else {
            this._customerWalletTransactionsServiceProxy.getCustomerWalletTransactionForEdit(customerWalletTransactionId).subscribe(result => {
                this.customerWalletTransaction = result.customerWalletTransaction;

                this.orderInvoiceNumber = result.orderInvoiceNumber;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._customerWalletTransactionsServiceProxy.createOrEdit(this.customerWalletTransaction)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectOrderModal() {
        this.customerWalletTransactionOrderLookupTableModal.id = this.customerWalletTransaction.orderId;
        this.customerWalletTransactionOrderLookupTableModal.displayName = this.orderInvoiceNumber;
        this.customerWalletTransactionOrderLookupTableModal.show();
    }


    setOrderIdNull() {
        this.customerWalletTransaction.orderId = null;
        this.orderInvoiceNumber = '';
    }


    getNewOrderId() {
        this.customerWalletTransaction.orderId = this.customerWalletTransactionOrderLookupTableModal.id;
        this.orderInvoiceNumber = this.customerWalletTransactionOrderLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
