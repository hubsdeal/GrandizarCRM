import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetStoreMasterThemeForViewDto, StoreMasterThemeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewStoreMasterThemeModal',
    templateUrl: './view-storeMasterTheme-modal.component.html',
})
export class ViewStoreMasterThemeModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetStoreMasterThemeForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetStoreMasterThemeForViewDto();
        this.item.storeMasterTheme = new StoreMasterThemeDto();
    }

    show(item: GetStoreMasterThemeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
