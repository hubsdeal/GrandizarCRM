import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetStoreTaxForViewDto, StoreTaxDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreTaxModal',
    templateUrl: './view-storeTax-modal.component.html',
})
export class ViewStoreTaxModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreTaxForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreTaxForViewDto();
        this.item.storeTax = new StoreTaxDto();
    }

    show(item: GetStoreTaxForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
