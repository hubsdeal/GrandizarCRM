import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetProductWholeSalePriceForViewDto, ProductWholeSalePriceDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductWholeSalePriceModal',
    templateUrl: './view-productWholeSalePrice-modal.component.html',
})
export class ViewProductWholeSalePriceModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductWholeSalePriceForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductWholeSalePriceForViewDto();
        this.item.productWholeSalePrice = new ProductWholeSalePriceDto();
    }

    show(item: GetProductWholeSalePriceForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
