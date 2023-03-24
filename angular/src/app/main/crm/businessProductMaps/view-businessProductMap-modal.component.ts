import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetBusinessProductMapForViewDto, BusinessProductMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewBusinessProductMapModal',
    templateUrl: './view-businessProductMap-modal.component.html',
})
export class ViewBusinessProductMapModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetBusinessProductMapForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetBusinessProductMapForViewDto();
        this.item.businessProductMap = new BusinessProductMapDto();
    }

    show(item: GetBusinessProductMapForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
