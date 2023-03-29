import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetProductVariantForViewDto, ProductVariantDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductVariantModal',
    templateUrl: './view-productVariant-modal.component.html',
})
export class ViewProductVariantModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductVariantForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductVariantForViewDto();
        this.item.productVariant = new ProductVariantDto();
    }

    show(item: GetProductVariantForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
