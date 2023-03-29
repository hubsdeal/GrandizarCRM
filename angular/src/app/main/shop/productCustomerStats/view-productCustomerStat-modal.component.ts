import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetProductCustomerStatForViewDto, ProductCustomerStatDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductCustomerStatModal',
    templateUrl: './view-productCustomerStat-modal.component.html',
})
export class ViewProductCustomerStatModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductCustomerStatForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductCustomerStatForViewDto();
        this.item.productCustomerStat = new ProductCustomerStatDto();
    }

    show(item: GetProductCustomerStatForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
