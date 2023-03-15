import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetContractTypeForViewDto, ContractTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewContractTypeModal',
    templateUrl: './view-contractType-modal.component.html',
})
export class ViewContractTypeModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetContractTypeForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetContractTypeForViewDto();
        this.item.contractType = new ContractTypeDto();
    }

    show(item: GetContractTypeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
