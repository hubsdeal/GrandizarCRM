import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetBusinessStoreMapForViewDto, BusinessStoreMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewBusinessStoreMapModal',
    templateUrl: './view-businessStoreMap-modal.component.html',
})
export class ViewBusinessStoreMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetBusinessStoreMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetBusinessStoreMapForViewDto();
        this.item.businessStoreMap = new BusinessStoreMapDto();
    }

    show(item: GetBusinessStoreMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
