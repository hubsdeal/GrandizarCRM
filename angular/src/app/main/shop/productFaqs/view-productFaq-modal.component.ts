import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetProductFaqForViewDto, ProductFaqDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductFaqModal',
    templateUrl: './view-productFaq-modal.component.html',
})
export class ViewProductFaqModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductFaqForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductFaqForViewDto();
        this.item.productFaq = new ProductFaqDto();
    }

    show(item: GetProductFaqForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
