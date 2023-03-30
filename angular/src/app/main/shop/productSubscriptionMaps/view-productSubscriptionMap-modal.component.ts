import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    GetProductSubscriptionMapForViewDto,
    ProductSubscriptionMapDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductSubscriptionMapModal',
    templateUrl: './view-productSubscriptionMap-modal.component.html',
})
export class ViewProductSubscriptionMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductSubscriptionMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductSubscriptionMapForViewDto();
        this.item.productSubscriptionMap = new ProductSubscriptionMapDto();
    }

    show(item: GetProductSubscriptionMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
