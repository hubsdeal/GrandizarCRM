import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetHubWidgetProductMapForViewDto, HubWidgetProductMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewHubWidgetProductMapModal',
    templateUrl: './view-hubWidgetProductMap-modal.component.html',
})
export class ViewHubWidgetProductMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetHubWidgetProductMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetHubWidgetProductMapForViewDto();
        this.item.hubWidgetProductMap = new HubWidgetProductMapDto();
    }

    show(item: GetHubWidgetProductMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
