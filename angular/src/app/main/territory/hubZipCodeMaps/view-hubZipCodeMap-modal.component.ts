import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetHubZipCodeMapForViewDto, HubZipCodeMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewHubZipCodeMapModal',
    templateUrl: './view-hubZipCodeMap-modal.component.html',
})
export class ViewHubZipCodeMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetHubZipCodeMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetHubZipCodeMapForViewDto();
        this.item.hubZipCodeMap = new HubZipCodeMapDto();
    }

    show(item: GetHubZipCodeMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
