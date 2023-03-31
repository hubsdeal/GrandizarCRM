import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    DiscountCodeGeneratorsServiceProxy,
    CreateOrEditDiscountCodeGeneratorDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditDiscountCodeGeneratorModal',
    templateUrl: './create-or-edit-discountCodeGenerator-modal.component.html',
})
export class CreateOrEditDiscountCodeGeneratorModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    discountCodeGenerator: CreateOrEditDiscountCodeGeneratorDto = new CreateOrEditDiscountCodeGeneratorDto();

    constructor(
        injector: Injector,
        private _discountCodeGeneratorsServiceProxy: DiscountCodeGeneratorsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(discountCodeGeneratorId?: number): void {
        if (!discountCodeGeneratorId) {
            this.discountCodeGenerator = new CreateOrEditDiscountCodeGeneratorDto();
            this.discountCodeGenerator.id = discountCodeGeneratorId;
            this.discountCodeGenerator.startDate = this._dateTimeService.getStartOfDay();
            this.discountCodeGenerator.endDate = this._dateTimeService.getStartOfDay();

            this.active = true;
            this.modal.show();
        } else {
            this._discountCodeGeneratorsServiceProxy
                .getDiscountCodeGeneratorForEdit(discountCodeGeneratorId)
                .subscribe((result) => {
                    this.discountCodeGenerator = result.discountCodeGenerator;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._discountCodeGeneratorsServiceProxy
            .createOrEdit(this.discountCodeGenerator)
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
