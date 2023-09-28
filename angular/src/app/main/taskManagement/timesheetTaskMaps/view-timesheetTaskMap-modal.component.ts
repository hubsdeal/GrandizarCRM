import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetTimesheetTaskMapForViewDto, TimesheetTaskMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewTimesheetTaskMapModal',
    templateUrl: './view-timesheetTaskMap-modal.component.html'
})
export class ViewTimesheetTaskMapModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetTimesheetTaskMapForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetTimesheetTaskMapForViewDto();
        this.item.timesheetTaskMap = new TimesheetTaskMapDto();
    }

    show(item: GetTimesheetTaskMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
