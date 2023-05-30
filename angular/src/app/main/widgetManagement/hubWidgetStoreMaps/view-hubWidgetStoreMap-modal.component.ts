import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetHubWidgetStoreMapForViewDto, HubWidgetStoreMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewHubWidgetStoreMapModal',
    templateUrl: './view-hubWidgetStoreMap-modal.component.html',
})
export class ViewHubWidgetStoreMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetHubWidgetStoreMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetHubWidgetStoreMapForViewDto();
        this.item.hubWidgetStoreMap = new HubWidgetStoreMapDto();
    }

    show(item: GetHubWidgetStoreMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
