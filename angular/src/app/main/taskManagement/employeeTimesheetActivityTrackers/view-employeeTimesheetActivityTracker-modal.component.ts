import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetEmployeeTimesheetActivityTrackerForViewDto, EmployeeTimesheetActivityTrackerDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewEmployeeTimesheetActivityTrackerModal',
    templateUrl: './view-employeeTimesheetActivityTracker-modal.component.html'
})
export class ViewEmployeeTimesheetActivityTrackerModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetEmployeeTimesheetActivityTrackerForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetEmployeeTimesheetActivityTrackerForViewDto();
        this.item.employeeTimesheetActivityTracker = new EmployeeTimesheetActivityTrackerDto();
    }

    show(item: GetEmployeeTimesheetActivityTrackerForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
