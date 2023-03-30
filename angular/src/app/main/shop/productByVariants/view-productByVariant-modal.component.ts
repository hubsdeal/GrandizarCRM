import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetProductByVariantForViewDto, ProductByVariantDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductByVariantModal',
    templateUrl: './view-productByVariant-modal.component.html',
})
export class ViewProductByVariantModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductByVariantForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductByVariantForViewDto();
        this.item.productByVariant = new ProductByVariantDto();
    }

    show(item: GetProductByVariantForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
