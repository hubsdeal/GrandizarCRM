import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetDiscountCodeGeneratorForViewDto, DiscountCodeGeneratorDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewDiscountCodeGeneratorModal',
    templateUrl: './view-discountCodeGenerator-modal.component.html',
})
export class ViewDiscountCodeGeneratorModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetDiscountCodeGeneratorForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetDiscountCodeGeneratorForViewDto();
        this.item.discountCodeGenerator = new DiscountCodeGeneratorDto();
    }

    show(item: GetDiscountCodeGeneratorForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
