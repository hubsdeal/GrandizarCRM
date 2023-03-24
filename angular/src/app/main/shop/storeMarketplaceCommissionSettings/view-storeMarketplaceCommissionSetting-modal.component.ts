import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    GetStoreMarketplaceCommissionSettingForViewDto,
    StoreMarketplaceCommissionSettingDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreMarketplaceCommissionSettingModal',
    templateUrl: './view-storeMarketplaceCommissionSetting-modal.component.html',
})
export class ViewStoreMarketplaceCommissionSettingModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreMarketplaceCommissionSettingForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreMarketplaceCommissionSettingForViewDto();
        this.item.storeMarketplaceCommissionSetting = new StoreMarketplaceCommissionSettingDto();
    }

    show(item: GetStoreMarketplaceCommissionSettingForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
