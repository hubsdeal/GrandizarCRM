import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetStoreThemeMapForViewDto, StoreThemeMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreThemeMapModal',
    templateUrl: './view-storeThemeMap-modal.component.html',
})
export class ViewStoreThemeMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreThemeMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreThemeMapForViewDto();
        this.item.storeThemeMap = new StoreThemeMapDto();
    }

    show(item: GetStoreThemeMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
