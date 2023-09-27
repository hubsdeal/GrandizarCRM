import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetCustomerLocalitiesZipCodeMapForViewDto, CustomerLocalitiesZipCodeMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewCustomerLocalitiesZipCodeMapModal',
    templateUrl: './view-customerLocalitiesZipCodeMap-modal.component.html'
})
export class ViewCustomerLocalitiesZipCodeMapModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetCustomerLocalitiesZipCodeMapForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetCustomerLocalitiesZipCodeMapForViewDto();
        this.item.customerLocalitiesZipCodeMap = new CustomerLocalitiesZipCodeMapDto();
    }

    show(item: GetCustomerLocalitiesZipCodeMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
