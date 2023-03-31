import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    GetProductCrossSellProductForViewDto,
    ProductCrossSellProductDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductCrossSellProductModal',
    templateUrl: './view-productCrossSellProduct-modal.component.html',
})
export class ViewProductCrossSellProductModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductCrossSellProductForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductCrossSellProductForViewDto();
        this.item.productCrossSellProduct = new ProductCrossSellProductDto();
    }

    show(item: GetProductCrossSellProductForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
