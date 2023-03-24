import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetBusinessTagForViewDto, BusinessTagDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewBusinessTagModal',
    templateUrl: './view-businessTag-modal.component.html',
})
export class ViewBusinessTagModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetBusinessTagForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetBusinessTagForViewDto();
        this.item.businessTag = new BusinessTagDto();
    }

    show(item: GetBusinessTagForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
