import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetStoreRelevantStoreForViewDto, StoreRelevantStoreDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreRelevantStoreModal',
    templateUrl: './view-storeRelevantStore-modal.component.html',
})
export class ViewStoreRelevantStoreModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreRelevantStoreForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreRelevantStoreForViewDto();
        this.item.storeRelevantStore = new StoreRelevantStoreDto();
    }

    show(item: GetStoreRelevantStoreForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
