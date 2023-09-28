import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { BookingTypesServiceProxy, CreateOrEditBookingTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';



@Component({
    selector: 'createOrEditBookingTypeModal',
    templateUrl: './create-or-edit-bookingType-modal.component.html'
})
export class CreateOrEditBookingTypeModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    bookingType: CreateOrEditBookingTypeDto = new CreateOrEditBookingTypeDto();




    constructor(
        injector: Injector,
        private _bookingTypesServiceProxy: BookingTypesServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(bookingTypeId?: number): void {
    

        if (!bookingTypeId) {
            this.bookingType = new CreateOrEditBookingTypeDto();
            this.bookingType.id = bookingTypeId;


            this.active = true;
            this.modal.show();
        } else {
            this._bookingTypesServiceProxy.getBookingTypeForEdit(bookingTypeId).subscribe(result => {
                this.bookingType = result.bookingType;



                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._bookingTypesServiceProxy.createOrEdit(this.bookingType)
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
