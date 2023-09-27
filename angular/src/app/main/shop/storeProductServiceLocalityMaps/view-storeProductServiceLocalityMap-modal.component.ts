import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetStoreProductServiceLocalityMapForViewDto, StoreProductServiceLocalityMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreProductServiceLocalityMapModal',
    templateUrl: './view-storeProductServiceLocalityMap-modal.component.html'
})
export class ViewStoreProductServiceLocalityMapModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreProductServiceLocalityMapForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetStoreProductServiceLocalityMapForViewDto();
        this.item.storeProductServiceLocalityMap = new StoreProductServiceLocalityMapDto();
    }

    show(item: GetStoreProductServiceLocalityMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
