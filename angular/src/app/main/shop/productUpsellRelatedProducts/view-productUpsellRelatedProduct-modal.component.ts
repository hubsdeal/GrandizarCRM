import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    GetProductUpsellRelatedProductForViewDto,
    ProductUpsellRelatedProductDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductUpsellRelatedProductModal',
    templateUrl: './view-productUpsellRelatedProduct-modal.component.html',
})
export class ViewProductUpsellRelatedProductModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductUpsellRelatedProductForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductUpsellRelatedProductForViewDto();
        this.item.productUpsellRelatedProduct = new ProductUpsellRelatedProductDto();
    }

    show(item: GetProductUpsellRelatedProductForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
