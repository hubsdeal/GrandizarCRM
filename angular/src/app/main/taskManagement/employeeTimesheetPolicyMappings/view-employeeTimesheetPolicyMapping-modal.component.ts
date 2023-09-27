import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetEmployeeTimesheetPolicyMappingForViewDto, EmployeeTimesheetPolicyMappingDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewEmployeeTimesheetPolicyMappingModal',
    templateUrl: './view-employeeTimesheetPolicyMapping-modal.component.html'
})
export class ViewEmployeeTimesheetPolicyMappingModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEmployeeTimesheetPolicyMappingForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEmployeeTimesheetPolicyMappingForViewDto();
        this.item.employeeTimesheetPolicyMapping = new EmployeeTimesheetPolicyMappingDto();
    }

    show(item: GetEmployeeTimesheetPolicyMappingForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
