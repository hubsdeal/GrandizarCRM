import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetMeasurementUnitForViewDto, MeasurementUnitDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewMeasurementUnitModal',
    templateUrl: './view-measurementUnit-modal.component.html',
})
export class ViewMeasurementUnitModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetMeasurementUnitForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetMeasurementUnitForViewDto();
        this.item.measurementUnit = new MeasurementUnitDto();
    }

    show(item: GetMeasurementUnitForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
