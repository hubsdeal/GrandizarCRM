import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetJobApplicantHireStatusTypeForViewDto, JobApplicantHireStatusTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewJobApplicantHireStatusTypeModal',
    templateUrl: './view-jobApplicantHireStatusType-modal.component.html'
})
export class ViewJobApplicantHireStatusTypeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetJobApplicantHireStatusTypeForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetJobApplicantHireStatusTypeForViewDto();
        this.item.jobApplicantHireStatusType = new JobApplicantHireStatusTypeDto();
    }

    show(item: GetJobApplicantHireStatusTypeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
