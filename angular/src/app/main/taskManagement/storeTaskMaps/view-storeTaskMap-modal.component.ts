import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetStoreTaskMapForViewDto, StoreTaskMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreTaskMapModal',
    templateUrl: './view-storeTaskMap-modal.component.html',
})
export class ViewStoreTaskMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreTaskMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreTaskMapForViewDto();
        this.item.storeTaskMap = new StoreTaskMapDto();
    }

    show(item: GetStoreTaskMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
