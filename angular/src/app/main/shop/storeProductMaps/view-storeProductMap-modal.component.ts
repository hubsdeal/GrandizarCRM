import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetStoreProductMapForViewDto, StoreProductMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreProductMapModal',
    templateUrl: './view-storeProductMap-modal.component.html',
})
export class ViewStoreProductMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreProductMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreProductMapForViewDto();
        this.item.storeProductMap = new StoreProductMapDto();
    }

    show(item: GetStoreProductMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
