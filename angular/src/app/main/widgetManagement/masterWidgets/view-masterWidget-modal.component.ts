import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetMasterWidgetForViewDto, MasterWidgetDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewMasterWidgetModal',
    templateUrl: './view-masterWidget-modal.component.html',
})
export class ViewMasterWidgetModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetMasterWidgetForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetMasterWidgetForViewDto();
        this.item.masterWidget = new MasterWidgetDto();
    }

    show(item: GetMasterWidgetForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
