import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { OrderTaskMapsServiceProxy, CreateOrEditOrderTaskMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { OrderTaskMapOrderLookupTableModalComponent } from './orderTaskMap-order-lookup-table-modal.component';
import { OrderTaskMapTaskEventLookupTableModalComponent } from './orderTaskMap-taskEvent-lookup-table-modal.component';



@Component({
    selector: 'createOrEditOrderTaskMapModal',
    templateUrl: './create-or-edit-orderTaskMap-modal.component.html'
})
export class CreateOrEditOrderTaskMapModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('orderTaskMapOrderLookupTableModal', { static: true }) orderTaskMapOrderLookupTableModal: OrderTaskMapOrderLookupTableModalComponent;
    @ViewChild('orderTaskMapTaskEventLookupTableModal', { static: true }) orderTaskMapTaskEventLookupTableModal: OrderTaskMapTaskEventLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    orderTaskMap: CreateOrEditOrderTaskMapDto = new CreateOrEditOrderTaskMapDto();

    orderInvoiceNumber = '';
    taskEventName = '';



    constructor(
        injector: Injector,
        private _orderTaskMapsServiceProxy: OrderTaskMapsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(orderTaskMapId?: number): void {
    

        if (!orderTaskMapId) {
            this.orderTaskMap = new CreateOrEditOrderTaskMapDto();
            this.orderTaskMap.id = orderTaskMapId;
            this.orderInvoiceNumber = '';
            this.taskEventName = '';


            this.active = true;
            this.modal.show();
        } else {
            this._orderTaskMapsServiceProxy.getOrderTaskMapForEdit(orderTaskMapId).subscribe(result => {
                this.orderTaskMap = result.orderTaskMap;

                this.orderInvoiceNumber = result.orderInvoiceNumber;
                this.taskEventName = result.taskEventName;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._orderTaskMapsServiceProxy.createOrEdit(this.orderTaskMap)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectOrderModal() {
        this.orderTaskMapOrderLookupTableModal.id = this.orderTaskMap.orderId;
        this.orderTaskMapOrderLookupTableModal.displayName = this.orderInvoiceNumber;
        this.orderTaskMapOrderLookupTableModal.show();
    }
    openSelectTaskEventModal() {
        this.orderTaskMapTaskEventLookupTableModal.id = this.orderTaskMap.taskEventId;
        this.orderTaskMapTaskEventLookupTableModal.displayName = this.taskEventName;
        this.orderTaskMapTaskEventLookupTableModal.show();
    }


    setOrderIdNull() {
        this.orderTaskMap.orderId = null;
        this.orderInvoiceNumber = '';
    }
    setTaskEventIdNull() {
        this.orderTaskMap.taskEventId = null;
        this.taskEventName = '';
    }


    getNewOrderId() {
        this.orderTaskMap.orderId = this.orderTaskMapOrderLookupTableModal.id;
        this.orderInvoiceNumber = this.orderTaskMapOrderLookupTableModal.displayName;
    }
    getNewTaskEventId() {
        this.orderTaskMap.taskEventId = this.orderTaskMapTaskEventLookupTableModal.id;
        this.taskEventName = this.orderTaskMapTaskEventLookupTableModal.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
