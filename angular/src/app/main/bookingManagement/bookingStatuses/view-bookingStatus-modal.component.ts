﻿import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetBookingStatusForViewDto, BookingStatusDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewBookingStatusModal',
    templateUrl: './view-bookingStatus-modal.component.html'
})
export class ViewBookingStatusModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetBookingStatusForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetBookingStatusForViewDto();
        this.item.bookingStatus = new BookingStatusDto();
    }

    show(item: GetBookingStatusForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
