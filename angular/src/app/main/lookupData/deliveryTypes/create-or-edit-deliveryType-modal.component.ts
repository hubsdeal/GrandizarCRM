import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { DeliveryTypesServiceProxy, CreateOrEditDeliveryTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';



@Component({
    selector: 'createOrEditDeliveryTypeModal',
    templateUrl: './create-or-edit-deliveryType-modal.component.html'
})
export class CreateOrEditDeliveryTypeModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    deliveryType: CreateOrEditDeliveryTypeDto = new CreateOrEditDeliveryTypeDto();




    constructor(
        injector: Injector,
        private _deliveryTypesServiceProxy: DeliveryTypesServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(deliveryTypeId?: number): void {
    

        if (!deliveryTypeId) {
            this.deliveryType = new CreateOrEditDeliveryTypeDto();
            this.deliveryType.id = deliveryTypeId;


            this.active = true;
            this.modal.show();
        } else {
            this._deliveryTypesServiceProxy.getDeliveryTypeForEdit(deliveryTypeId).subscribe(result => {
                this.deliveryType = result.deliveryType;



                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._deliveryTypesServiceProxy.createOrEdit(this.deliveryType)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }













    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
