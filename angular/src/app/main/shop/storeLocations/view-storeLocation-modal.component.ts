import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetStoreLocationForViewDto, StoreLocationDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreLocationModal',
    templateUrl: './view-storeLocation-modal.component.html',
})
export class ViewStoreLocationModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreLocationForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreLocationForViewDto();
        this.item.storeLocation = new StoreLocationDto();
    }

    show(item: GetStoreLocationForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
