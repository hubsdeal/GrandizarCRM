import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetJobHiringTeamForViewDto, JobHiringTeamDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewJobHiringTeamModal',
    templateUrl: './view-jobHiringTeam-modal.component.html'
})
export class ViewJobHiringTeamModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetJobHiringTeamForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetJobHiringTeamForViewDto();
        this.item.jobHiringTeam = new JobHiringTeamDto();
    }

    show(item: GetJobHiringTeamForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
