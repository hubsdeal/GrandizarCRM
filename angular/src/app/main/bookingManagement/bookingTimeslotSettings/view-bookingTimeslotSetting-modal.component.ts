import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetBookingTimeslotSettingForViewDto, BookingTimeslotSettingDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewBookingTimeslotSettingModal',
    templateUrl: './view-bookingTimeslotSetting-modal.component.html'
})
export class ViewBookingTimeslotSettingModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetBookingTimeslotSettingForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetBookingTimeslotSettingForViewDto();
        this.item.bookingTimeslotSetting = new BookingTimeslotSettingDto();
    }

    show(item: GetBookingTimeslotSettingForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
