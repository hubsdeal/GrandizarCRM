import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetProductAndGiftCardMapForViewDto, ProductAndGiftCardMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductAndGiftCardMapModal',
    templateUrl: './view-productAndGiftCardMap-modal.component.html',
})
export class ViewProductAndGiftCardMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductAndGiftCardMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductAndGiftCardMapForViewDto();
        this.item.productAndGiftCardMap = new ProductAndGiftCardMapDto();
    }

    show(item: GetProductAndGiftCardMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
