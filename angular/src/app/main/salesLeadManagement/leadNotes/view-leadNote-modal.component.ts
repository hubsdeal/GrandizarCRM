import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetLeadNoteForViewDto, LeadNoteDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewLeadNoteModal',
    templateUrl: './view-leadNote-modal.component.html',
})
export class ViewLeadNoteModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetLeadNoteForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetLeadNoteForViewDto();
        this.item.leadNote = new LeadNoteDto();
    }

    show(item: GetLeadNoteForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
