import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetProductTagForViewDto, ProductTagDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductTagModal',
    templateUrl: './view-productTag-modal.component.html',
})
export class ViewProductTagModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductTagForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductTagForViewDto();
        this.item.productTag = new ProductTagDto();
    }

    show(item: GetProductTagForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
