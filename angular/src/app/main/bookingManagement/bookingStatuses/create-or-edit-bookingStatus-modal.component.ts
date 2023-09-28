import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { BookingStatusesServiceProxy, CreateOrEditBookingStatusDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

             import { DateTimeService } from '@app/shared/common/timing/date-time.service';



@Component({
    selector: 'createOrEditBookingStatusModal',
    templateUrl: './create-or-edit-bookingStatus-modal.component.html'
})
export class CreateOrEditBookingStatusModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    bookingStatus: CreateOrEditBookingStatusDto = new CreateOrEditBookingStatusDto();




    constructor(
        injector: Injector,
        private _bookingStatusesServiceProxy: BookingStatusesServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(bookingStatusId?: number): void {
    

        if (!bookingStatusId) {
            this.bookingStatus = new CreateOrEditBookingStatusDto();
            this.bookingStatus.id = bookingStatusId;


            this.active = true;
            this.modal.show();
        } else {
            this._bookingStatusesServiceProxy.getBookingStatusForEdit(bookingStatusId).subscribe(result => {
                this.bookingStatus = result.bookingStatus;



                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._bookingStatusesServiceProxy.createOrEdit(this.bookingStatus)
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
