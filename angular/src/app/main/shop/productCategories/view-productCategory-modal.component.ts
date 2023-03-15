import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetProductCategoryForViewDto, ProductCategoryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductCategoryModal',
    templateUrl: './view-productCategory-modal.component.html',
})
export class ViewProductCategoryModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductCategoryForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductCategoryForViewDto();
        this.item.productCategory = new ProductCategoryDto();
    }

    show(item: GetProductCategoryForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
