import {AppConsts} from "@shared/AppConsts";
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetHubJobForViewDto, HubJobDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewHubJobModal',
    templateUrl: './view-hubJob-modal.component.html'
})
export class ViewHubJobModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetHubJobForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetHubJobForViewDto();
        this.item.hubJob = new HubJobDto();
    }

    show(item: GetHubJobForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }
    
    

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
