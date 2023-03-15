import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetMasterTagForViewDto, MasterTagDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewMasterTagModal',
    templateUrl: './view-masterTag-modal.component.html',
})
export class ViewMasterTagModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetMasterTagForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetMasterTagForViewDto();
        this.item.masterTag = new MasterTagDto();
    }

    show(item: GetMasterTagForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
