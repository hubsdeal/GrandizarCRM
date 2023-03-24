import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    GetMarketplaceCommissionTypeForViewDto,
    MarketplaceCommissionTypeDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewMarketplaceCommissionTypeModal',
    templateUrl: './view-marketplaceCommissionType-modal.component.html',
})
export class ViewMarketplaceCommissionTypeModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetMarketplaceCommissionTypeForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetMarketplaceCommissionTypeForViewDto();
        this.item.marketplaceCommissionType = new MarketplaceCommissionTypeDto();
    }

    show(item: GetMarketplaceCommissionTypeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
