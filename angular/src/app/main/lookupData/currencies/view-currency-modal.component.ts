import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetCurrencyForViewDto, CurrencyDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewCurrencyModal',
    templateUrl: './view-currency-modal.component.html',
})
export class ViewCurrencyModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetCurrencyForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetCurrencyForViewDto();
        this.item.currency = new CurrencyDto();
    }

    show(item: GetCurrencyForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
