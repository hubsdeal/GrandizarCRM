import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetTimesheetPolicyForViewDto, TimesheetPolicyDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewTimesheetPolicyModal',
    templateUrl: './view-timesheetPolicy-modal.component.html'
})
export class ViewTimesheetPolicyModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetTimesheetPolicyForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetTimesheetPolicyForViewDto();
        this.item.timesheetPolicy = new TimesheetPolicyDto();
    }

    show(item: GetTimesheetPolicyForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
