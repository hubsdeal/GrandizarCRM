import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetContractFinancialTermForViewDto, ContractFinancialTermDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewContractFinancialTermModal',
    templateUrl: './view-contractFinancialTerm-modal.component.html'
})
export class ViewContractFinancialTermModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetContractFinancialTermForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetContractFinancialTermForViewDto();
        this.item.contractFinancialTerm = new ContractFinancialTermDto();
    }

    show(item: GetContractFinancialTermForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
