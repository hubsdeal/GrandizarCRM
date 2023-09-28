import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetJobReferralFeeSplitPolicyForViewDto, JobReferralFeeSplitPolicyDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewJobReferralFeeSplitPolicyModal',
    templateUrl: './view-jobReferralFeeSplitPolicy-modal.component.html'
})
export class ViewJobReferralFeeSplitPolicyModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetJobReferralFeeSplitPolicyForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetJobReferralFeeSplitPolicyForViewDto();
        this.item.jobReferralFeeSplitPolicy = new JobReferralFeeSplitPolicyDto();
    }

    show(item: GetJobReferralFeeSplitPolicyForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
