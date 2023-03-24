import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetStoreBusinessHourForViewDto, StoreBusinessHourDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreBusinessHourModal',
    templateUrl: './view-storeBusinessHour-modal.component.html',
})
export class ViewStoreBusinessHourModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreBusinessHourForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreBusinessHourForViewDto();
        this.item.storeBusinessHour = new StoreBusinessHourDto();
    }

    show(item: GetStoreBusinessHourForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
