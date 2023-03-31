import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetReturnTypeForViewDto, ReturnTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewReturnTypeModal',
    templateUrl: './view-returnType-modal.component.html',
})
export class ViewReturnTypeModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetReturnTypeForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetReturnTypeForViewDto();
        this.item.returnType = new ReturnTypeDto();
    }

    show(item: GetReturnTypeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
