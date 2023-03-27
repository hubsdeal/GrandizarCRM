import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetStoreZipCodeMapForViewDto, StoreZipCodeMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreZipCodeMapModal',
    templateUrl: './view-storeZipCodeMap-modal.component.html',
})
export class ViewStoreZipCodeMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreZipCodeMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreZipCodeMapForViewDto();
        this.item.storeZipCodeMap = new StoreZipCodeMapDto();
    }

    show(item: GetStoreZipCodeMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
