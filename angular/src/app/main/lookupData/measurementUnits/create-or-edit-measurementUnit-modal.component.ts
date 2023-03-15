import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { MeasurementUnitsServiceProxy, CreateOrEditMeasurementUnitDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditMeasurementUnitModal',
    templateUrl: './create-or-edit-measurementUnit-modal.component.html',
})
export class CreateOrEditMeasurementUnitModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    measurementUnit: CreateOrEditMeasurementUnitDto = new CreateOrEditMeasurementUnitDto();

    constructor(
        injector: Injector,
        private _measurementUnitsServiceProxy: MeasurementUnitsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(measurementUnitId?: number): void {
        if (!measurementUnitId) {
            this.measurementUnit = new CreateOrEditMeasurementUnitDto();
            this.measurementUnit.id = measurementUnitId;

            this.active = true;
            this.modal.show();
        } else {
            this._measurementUnitsServiceProxy.getMeasurementUnitForEdit(measurementUnitId).subscribe((result) => {
                this.measurementUnit = result.measurementUnit;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._measurementUnitsServiceProxy
            .createOrEdit(this.measurementUnit)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
