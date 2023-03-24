import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetStoreContactMapForViewDto, StoreContactMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreContactMapModal',
    templateUrl: './view-storeContactMap-modal.component.html',
})
export class ViewStoreContactMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreContactMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreContactMapForViewDto();
        this.item.storeContactMap = new StoreContactMapDto();
    }

    show(item: GetStoreContactMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
