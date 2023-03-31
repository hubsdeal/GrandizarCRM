import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    GetProductWholeSaleQuantityTypeForViewDto,
    ProductWholeSaleQuantityTypeDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductWholeSaleQuantityTypeModal',
    templateUrl: './view-productWholeSaleQuantityType-modal.component.html',
})
export class ViewProductWholeSaleQuantityTypeModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductWholeSaleQuantityTypeForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductWholeSaleQuantityTypeForViewDto();
        this.item.productWholeSaleQuantityType = new ProductWholeSaleQuantityTypeDto();
    }

    show(item: GetProductWholeSaleQuantityTypeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
