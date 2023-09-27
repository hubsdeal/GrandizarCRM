import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { OrderDeliveryChangeCaptainsServiceProxy, CreateOrEditOrderDeliveryChangeCaptainDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { OrderDeliveryChangeCaptainOrderDeliveryByCaptainLookupTableModalComponent } from './orderDeliveryChangeCaptain-orderDeliveryByCaptain-lookup-table-modal.component';
import { OrderDeliveryChangeCaptainEmployeeLookupTableModalComponent } from './orderDeliveryChangeCaptain-employee-lookup-table-modal.component';



@Component({
    selector: 'createOrEditOrderDeliveryChangeCaptainModal',
    templateUrl: './create-or-edit-orderDeliveryChangeCaptain-modal.component.html'
})
export class CreateOrEditOrderDeliveryChangeCaptainModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('orderDeliveryChangeCaptainOrderDeliveryByCaptainLookupTableModal', { static: true }) orderDeliveryChangeCaptainOrderDeliveryByCaptainLookupTableModal: OrderDeliveryChangeCaptainOrderDeliveryByCaptainLookupTableModalComponent;
    @ViewChild('orderDeliveryChangeCaptainEmployeeLookupTableModal', { static: true }) orderDeliveryChangeCaptainEmployeeLookupTableModal: OrderDeliveryChangeCaptainEmployeeLookupTableModalComponent;
    @ViewChild('orderDeliveryChangeCaptainEmployeeLookupTableModal2', { static: true }) orderDeliveryChangeCaptainEmployeeLookupTableModal2: OrderDeliveryChangeCaptainEmployeeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    orderDeliveryChangeCaptain: CreateOrEditOrderDeliveryChangeCaptainDto = new CreateOrEditOrderDeliveryChangeCaptainDto();

    orderDeliveryByCaptainOrderDeliveryRouteData = '';
    employeeName = '';
    employeeName2 = '';



    constructor(
        injector: Injector,
        private _orderDeliveryChangeCaptainsServiceProxy: OrderDeliveryChangeCaptainsServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(orderDeliveryChangeCaptainId?: number): void {
    

        if (!orderDeliveryChangeCaptainId) {
            this.orderDeliveryChangeCaptain = new CreateOrEditOrderDeliveryChangeCaptainDto();
            this.orderDeliveryChangeCaptain.id = orderDeliveryChangeCaptainId;
            this.orderDeliveryChangeCaptain.acceptedOrderDateTime = this._dateTimeService.getStartOfDay();
            this.orderDeliveryChangeCaptain.rejectedOrderDateTime = this._dateTimeService.getStartOfDay();
            this.orderDeliveryByCaptainOrderDeliveryRouteData = '';
            this.employeeName = '';
            this.employeeName2 = '';


            this.active = true;
            this.modal.show();
        } else {
            this._orderDeliveryChangeCaptainsServiceProxy.getOrderDeliveryChangeCaptainForEdit(orderDeliveryChangeCaptainId).subscribe(result => {
                this.orderDeliveryChangeCaptain = result.orderDeliveryChangeCaptain;

                this.orderDeliveryByCaptainOrderDeliveryRouteData = result.orderDeliveryByCaptainOrderDeliveryRouteData;
                this.employeeName = result.employeeName;
                this.employeeName2 = result.employeeName2;


                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._orderDeliveryChangeCaptainsServiceProxy.createOrEdit(this.orderDeliveryChangeCaptain)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }

    openSelectOrderDeliveryByCaptainModal() {
        this.orderDeliveryChangeCaptainOrderDeliveryByCaptainLookupTableModal.id = this.orderDeliveryChangeCaptain.orderDeliveryByCaptainId;
        this.orderDeliveryChangeCaptainOrderDeliveryByCaptainLookupTableModal.displayName = this.orderDeliveryByCaptainOrderDeliveryRouteData;
        this.orderDeliveryChangeCaptainOrderDeliveryByCaptainLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.orderDeliveryChangeCaptainEmployeeLookupTableModal.id = this.orderDeliveryChangeCaptain.newDriverEmployeeCaptainId;
        this.orderDeliveryChangeCaptainEmployeeLookupTableModal.displayName = this.employeeName;
        this.orderDeliveryChangeCaptainEmployeeLookupTableModal.show();
    }
    openSelectEmployeeModal2() {
        this.orderDeliveryChangeCaptainEmployeeLookupTableModal2.id = this.orderDeliveryChangeCaptain.firstEmployeeCaptainID ;
        this.orderDeliveryChangeCaptainEmployeeLookupTableModal2.displayName = this.employeeName;
        this.orderDeliveryChangeCaptainEmployeeLookupTableModal2.show();
    }


    setOrderDeliveryByCaptainIdNull() {
        this.orderDeliveryChangeCaptain.orderDeliveryByCaptainId = null;
        this.orderDeliveryByCaptainOrderDeliveryRouteData = '';
    }
    setNewDriverEmployeeCaptainIdNull() {
        this.orderDeliveryChangeCaptain.newDriverEmployeeCaptainId = null;
        this.employeeName = '';
    }
    setFirstEmployeeCaptainIDNull() {
        this.orderDeliveryChangeCaptain.firstEmployeeCaptainID  = null;
        this.employeeName2 = '';
    }


    getNewOrderDeliveryByCaptainId() {
        this.orderDeliveryChangeCaptain.orderDeliveryByCaptainId = this.orderDeliveryChangeCaptainOrderDeliveryByCaptainLookupTableModal.id;
        this.orderDeliveryByCaptainOrderDeliveryRouteData = this.orderDeliveryChangeCaptainOrderDeliveryByCaptainLookupTableModal.displayName;
    }
    getNewNewDriverEmployeeCaptainId() {
        this.orderDeliveryChangeCaptain.newDriverEmployeeCaptainId = this.orderDeliveryChangeCaptainEmployeeLookupTableModal.id;
        this.employeeName = this.orderDeliveryChangeCaptainEmployeeLookupTableModal.displayName;
    }
    getNewFirstEmployeeCaptainID () {
        this.orderDeliveryChangeCaptain.firstEmployeeCaptainID  = this.orderDeliveryChangeCaptainEmployeeLookupTableModal2.id;
        this.employeeName2 = this.orderDeliveryChangeCaptainEmployeeLookupTableModal2.displayName;
    }








    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
