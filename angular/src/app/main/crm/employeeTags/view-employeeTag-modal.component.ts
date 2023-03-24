import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetEmployeeTagForViewDto, EmployeeTagDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewEmployeeTagModal',
    templateUrl: './view-employeeTag-modal.component.html',
})
export class ViewEmployeeTagModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEmployeeTagForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetEmployeeTagForViewDto();
        this.item.employeeTag = new EmployeeTagDto();
    }

    show(item: GetEmployeeTagForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
