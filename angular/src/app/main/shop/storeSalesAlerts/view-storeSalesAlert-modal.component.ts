import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetStoreSalesAlertForViewDto, StoreSalesAlertDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreSalesAlertModal',
    templateUrl: './view-storeSalesAlert-modal.component.html',
})
export class ViewStoreSalesAlertModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreSalesAlertForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreSalesAlertForViewDto();
        this.item.storeSalesAlert = new StoreSalesAlertDto();
    }

    show(item: GetStoreSalesAlertForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
