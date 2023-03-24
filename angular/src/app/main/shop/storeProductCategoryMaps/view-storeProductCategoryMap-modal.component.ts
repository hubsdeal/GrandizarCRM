import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    GetStoreProductCategoryMapForViewDto,
    StoreProductCategoryMapDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreProductCategoryMapModal',
    templateUrl: './view-storeProductCategoryMap-modal.component.html',
})
export class ViewStoreProductCategoryMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreProductCategoryMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreProductCategoryMapForViewDto();
        this.item.storeProductCategoryMap = new StoreProductCategoryMapDto();
    }

    show(item: GetStoreProductCategoryMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
