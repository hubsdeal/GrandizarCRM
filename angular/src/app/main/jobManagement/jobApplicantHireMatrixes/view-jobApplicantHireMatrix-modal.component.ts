import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetJobApplicantHireMatrixForViewDto, JobApplicantHireMatrixDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewJobApplicantHireMatrixModal',
    templateUrl: './view-jobApplicantHireMatrix-modal.component.html'
})
export class ViewJobApplicantHireMatrixModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetJobApplicantHireMatrixForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetJobApplicantHireMatrixForViewDto();
        this.item.jobApplicantHireMatrix = new JobApplicantHireMatrixDto();
    }

    show(item: GetJobApplicantHireMatrixForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
