import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetJobTaskMapForViewDto, JobTaskMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewJobTaskMapModal',
    templateUrl: './view-jobTaskMap-modal.component.html',
})
export class ViewJobTaskMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetJobTaskMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetJobTaskMapForViewDto();
        this.item.jobTaskMap = new JobTaskMapDto();
    }

    show(item: GetJobTaskMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
