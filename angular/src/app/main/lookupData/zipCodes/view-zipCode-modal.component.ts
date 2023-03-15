import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetZipCodeForViewDto, ZipCodeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewZipCodeModal',
    templateUrl: './view-zipCode-modal.component.html',
})
export class ViewZipCodeModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetZipCodeForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetZipCodeForViewDto();
        this.item.zipCode = new ZipCodeDto();
    }

    show(item: GetZipCodeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
