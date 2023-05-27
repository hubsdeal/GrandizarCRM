import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import {
    GetProductMasterTagSettingForViewDto,
    ProductMasterTagSettingDto,
    AnswerType,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewProductMasterTagSettingModal',
    templateUrl: './view-productMasterTagSetting-modal.component.html',
})
export class ViewProductMasterTagSettingModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetProductMasterTagSettingForViewDto;
    answerType = AnswerType;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetProductMasterTagSettingForViewDto();
        this.item.productMasterTagSetting = new ProductMasterTagSettingDto();
    }

    show(item: GetProductMasterTagSettingForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
