import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetProductTaskMapForViewDto, ProductTaskMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductTaskMapModal',
    templateUrl: './view-productTaskMap-modal.component.html',
})
export class ViewProductTaskMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductTaskMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductTaskMapForViewDto();
        this.item.productTaskMap = new ProductTaskMapDto();
    }

    show(item: GetProductTaskMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
