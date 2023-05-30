import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetStoreWidgetProductMapForViewDto, StoreWidgetProductMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreWidgetProductMapModal',
    templateUrl: './view-storeWidgetProductMap-modal.component.html',
})
export class ViewStoreWidgetProductMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreWidgetProductMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreWidgetProductMapForViewDto();
        this.item.storeWidgetProductMap = new StoreWidgetProductMapDto();
    }

    show(item: GetStoreWidgetProductMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
