import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { OrderDeliveryByCaptainsServiceProxy, CreateOrEditOrderDeliveryByCaptainDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { OrderDeliveryByCaptainOrderLookupTableModalComponent } from './orderDeliveryByCaptain-order-lookup-table-modal.component';
import { OrderDeliveryByCaptainStoreLookupTableModalComponent } from './orderDeliveryByCaptain-store-lookup-table-modal.component';
import { OrderDeliveryByCaptainContactLookupTableModalComponent } from './orderDeliveryByCaptain-contact-lookup-table-modal.component';
import { OrderDeliveryByCaptainEmployeeLookupTableModalComponent } from './orderDeliveryByCaptain-employee-lookup-table-modal.component';
import { SelectItem } from 'primeng/api';



@Component({
    selector: 'createOrEditOrderDeliveryByCaptainModal',
    templateUrl: './create-or-edit-orderDeliveryByCaptain-modal.component.html'
})
export class CreateOrEditOrderDeliveryByCaptainModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('orderDeliveryByCaptainOrderLookupTableModal', { static: true }) orderDeliveryByCaptainOrderLookupTableModal: OrderDeliveryByCaptainOrderLookupTableModalComponent;
    @ViewChild('orderDeliveryByCaptainStoreLookupTableModal', { static: true }) orderDeliveryByCaptainStoreLookupTableModal: OrderDeliveryByCaptainStoreLookupTableModalComponent;
    @ViewChild('orderDeliveryByCaptainContactLookupTableModal', { static: true }) orderDeliveryByCaptainContactLookupTableModal: OrderDeliveryByCaptainContactLookupTableModalComponent;
    @ViewChild('orderDeliveryByCaptainEmployeeLookupTableModal', { static: true }) orderDeliveryByCaptainEmployeeLookupTableModal: OrderDeliveryByCaptainEmployeeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    orderDeliveryByCaptain: CreateOrEditOrderDeliveryByCaptainDto = new CreateOrEditOrderDeliveryByCaptainDto();

    orderInvoiceNumber = '';
    storeName = '';
    contactFullName = '';
    employeeName = '';

    captainSelectionOptions: SelectItem[];

    constructor(
        injector: Injector,
        private _orderDeliveryByCaptainsServiceProxy: OrderDeliveryByCaptainsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(orderDeliveryByCaptainId?: number): void {
    

        if (!orderDeliveryByCaptainId) {
            this.orderDeliveryByCaptain = new CreateOrEditOrderDeliveryByCaptainDto();
            this.orderDeliveryByCaptain.captainSelectionAutoOrManual=true;
            this.orderDeliveryByCaptain.id = orderDeliveryByCaptainId;
            this.orderDeliveryByCaptain.captainOrderAcceptedDateTime = this._dateTimeService.getStartOfDay();
            this.orderDeliveryByCaptain.captainOrderPickedupDateTime = this._dateTimeService.getStartOfDay();
            this.orderDeliveryByCaptain.captainOrderDeliveredToCustomerDateTime = this._dateTimeService.getStartOfDay();
            this.orderInvoiceNumber = '';
            this.storeName = '';
            this.contactFullName = '';
            this.employeeName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._orderDeliveryByCaptainsServiceProxy.getOrderDeliveryByCaptainForEdit(orderDeliveryByCaptainId).subscribe(result => {
                this.orderDeliveryByCaptain = result.orderDeliveryByCaptain;

                this.orderInvoiceNumber = result.orderInvoiceNumber;
                this.storeName = result.storeName;
                this.contactFullName = result.contactFullName;
                this.employeeName = result.employeeName;


                this.active = true;
                this.modal.show();
            });
        }
        
        this.captainSelectionOptions = [{ label: 'Auto Select Delivery Captain', value: true }, { label: 'Manual Select Delivery Captain', value: false }];
    }

    save(): void {
            this.saving = true;
            
			
			
            this._orderDeliveryByCaptainsServiceProxy.createOrEdit(this.orderDeliveryByCaptain)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectOrderModal() {
        this.orderDeliveryByCaptainOrderLookupTableModal.id = this.orderDeliveryByCaptain.orderId;
        this.orderDeliveryByCaptainOrderLookupTableModal.displayName = this.orderInvoiceNumber;
        this.orderDeliveryByCaptainOrderLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.orderDeliveryByCaptainStoreLookupTableModal.id = this.orderDeliveryByCaptain.storeId;
        this.orderDeliveryByCaptainStoreLookupTableModal.displayName = this.storeName;
        this.orderDeliveryByCaptainStoreLookupTableModal.show();
    }
    openSelectContactModal() {
        this.orderDeliveryByCaptainContactLookupTableModal.id = this.orderDeliveryByCaptain.contactId;
        this.orderDeliveryByCaptainContactLookupTableModal.displayName = this.contactFullName;
        this.orderDeliveryByCaptainContactLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.orderDeliveryByCaptainEmployeeLookupTableModal.id = this.orderDeliveryByCaptain.employeeCaptainId;
        this.orderDeliveryByCaptainEmployeeLookupTableModal.displayName = this.employeeName;
        this.orderDeliveryByCaptainEmployeeLookupTableModal.show();
    }


    setOrderIdNull() {
        this.orderDeliveryByCaptain.orderId = null;
        this.orderInvoiceNumber = '';
    }
    setStoreIdNull() {
        this.orderDeliveryByCaptain.storeId = null;
        this.storeName = '';
    }
    setContactIdNull() {
        this.orderDeliveryByCaptain.contactId = null;
        this.contactFullName = '';
    }
    setEmployeeCaptainIdNull() {
        this.orderDeliveryByCaptain.employeeCaptainId = null;
        this.employeeName = '';
    }


    getNewOrderId() {
        this.orderDeliveryByCaptain.orderId = this.orderDeliveryByCaptainOrderLookupTableModal.id;
        this.orderInvoiceNumber = this.orderDeliveryByCaptainOrderLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.orderDeliveryByCaptain.storeId = this.orderDeliveryByCaptainStoreLookupTableModal.id;
        this.storeName = this.orderDeliveryByCaptainStoreLookupTableModal.displayName;
    }
    getNewContactId() {
        this.orderDeliveryByCaptain.contactId = this.orderDeliveryByCaptainContactLookupTableModal.id;
        this.contactFullName = this.orderDeliveryByCaptainContactLookupTableModal.displayName;
    }
    getNewEmployeeCaptainId() {
        this.orderDeliveryByCaptain.employeeCaptainId = this.orderDeliveryByCaptainEmployeeLookupTableModal.id;
        this.employeeName = this.orderDeliveryByCaptainEmployeeLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
