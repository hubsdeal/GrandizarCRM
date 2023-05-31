import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetStoreWidgetMapForViewDto, StoreWidgetMapDto, WidgetType } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreWidgetMapModal',
    templateUrl: './view-storeWidgetMap-modal.component.html',
})
export class ViewStoreWidgetMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreWidgetMapForViewDto;
    widgetType = WidgetType;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreWidgetMapForViewDto();
        this.item.storeWidgetMap = new StoreWidgetMapDto();
    }

    show(item: GetStoreWidgetMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
