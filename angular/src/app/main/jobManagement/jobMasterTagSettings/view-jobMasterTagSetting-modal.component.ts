import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    GetJobMasterTagSettingForViewDto,
    JobMasterTagSettingDto,
    AnswerType,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewJobMasterTagSettingModal',
    templateUrl: './view-jobMasterTagSetting-modal.component.html',
})
export class ViewJobMasterTagSettingModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetJobMasterTagSettingForViewDto;
    answerType = AnswerType;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetJobMasterTagSettingForViewDto();
        this.item.jobMasterTagSetting = new JobMasterTagSettingDto();
    }

    show(item: GetJobMasterTagSettingForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
