import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetHubWidgetContentMapForViewDto, HubWidgetContentMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewHubWidgetContentMapModal',
    templateUrl: './view-hubWidgetContentMap-modal.component.html',
})
export class ViewHubWidgetContentMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetHubWidgetContentMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetHubWidgetContentMapForViewDto();
        this.item.hubWidgetContentMap = new HubWidgetContentMapDto();
    }

    show(item: GetHubWidgetContentMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
