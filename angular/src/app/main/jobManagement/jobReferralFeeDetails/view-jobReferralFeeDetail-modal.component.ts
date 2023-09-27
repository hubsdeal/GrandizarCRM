import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetJobReferralFeeDetailForViewDto, JobReferralFeeDetailDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewJobReferralFeeDetailModal',
    templateUrl: './view-jobReferralFeeDetail-modal.component.html'
})
export class ViewJobReferralFeeDetailModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetJobReferralFeeDetailForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetJobReferralFeeDetailForViewDto();
        this.item.jobReferralFeeDetail = new JobReferralFeeDetailDto();
    }

    show(item: GetJobReferralFeeDetailForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
