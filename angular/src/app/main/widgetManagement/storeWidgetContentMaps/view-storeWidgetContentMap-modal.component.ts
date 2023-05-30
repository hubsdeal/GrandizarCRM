import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetStoreWidgetContentMapForViewDto, StoreWidgetContentMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreWidgetContentMapModal',
    templateUrl: './view-storeWidgetContentMap-modal.component.html',
})
export class ViewStoreWidgetContentMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreWidgetContentMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreWidgetContentMapForViewDto();
        this.item.storeWidgetContentMap = new StoreWidgetContentMapDto();
    }

    show(item: GetStoreWidgetContentMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
