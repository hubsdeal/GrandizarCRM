import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    GetStoreTagSettingCategoryForViewDto,
    StoreTagSettingCategoryDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreTagSettingCategoryModal',
    templateUrl: './view-storeTagSettingCategory-modal.component.html',
})
export class ViewStoreTagSettingCategoryModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreTagSettingCategoryForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreTagSettingCategoryForViewDto();
        this.item.storeTagSettingCategory = new StoreTagSettingCategoryDto();
    }

    show(item: GetStoreTagSettingCategoryForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
