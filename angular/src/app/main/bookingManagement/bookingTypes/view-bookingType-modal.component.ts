import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetBookingTypeForViewDto, BookingTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewBookingTypeModal',
    templateUrl: './view-bookingType-modal.component.html'
})
export class ViewBookingTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetBookingTypeForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetBookingTypeForViewDto();
        this.item.bookingType = new BookingTypeDto();
    }

    show(item: GetBookingTypeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
