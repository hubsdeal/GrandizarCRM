import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetMasterTagCategoryForViewDto, MasterTagCategoryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewMasterTagCategoryModal',
    templateUrl: './view-masterTagCategory-modal.component.html',
})
export class ViewMasterTagCategoryModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetMasterTagCategoryForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetMasterTagCategoryForViewDto();
        this.item.masterTagCategory = new MasterTagCategoryDto();
    }

    show(item: GetMasterTagCategoryForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
