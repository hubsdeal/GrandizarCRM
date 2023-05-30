import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetHubWidgetMapForViewDto, HubWidgetMapDto, WidgetType } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewHubWidgetMapModal',
    templateUrl: './view-hubWidgetMap-modal.component.html',
})
export class ViewHubWidgetMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetHubWidgetMapForViewDto;
    widgetType = WidgetType;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetHubWidgetMapForViewDto();
        this.item.hubWidgetMap = new HubWidgetMapDto();
    }

    show(item: GetHubWidgetMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
