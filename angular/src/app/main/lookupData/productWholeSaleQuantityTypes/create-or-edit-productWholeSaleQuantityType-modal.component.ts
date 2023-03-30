import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductWholeSaleQuantityTypesServiceProxy,
    CreateOrEditProductWholeSaleQuantityTypeDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditProductWholeSaleQuantityTypeModal',
    templateUrl: './create-or-edit-productWholeSaleQuantityType-modal.component.html',
})
export class CreateOrEditProductWholeSaleQuantityTypeModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productWholeSaleQuantityType: CreateOrEditProductWholeSaleQuantityTypeDto =
        new CreateOrEditProductWholeSaleQuantityTypeDto();

    constructor(
        injector: Injector,
        private _productWholeSaleQuantityTypesServiceProxy: ProductWholeSaleQuantityTypesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productWholeSaleQuantityTypeId?: number): void {
        if (!productWholeSaleQuantityTypeId) {
            this.productWholeSaleQuantityType = new CreateOrEditProductWholeSaleQuantityTypeDto();
            this.productWholeSaleQuantityType.id = productWholeSaleQuantityTypeId;

            this.active = true;
            this.modal.show();
        } else {
            this._productWholeSaleQuantityTypesServiceProxy
                .getProductWholeSaleQuantityTypeForEdit(productWholeSaleQuantityTypeId)
                .subscribe((result) => {
                    this.productWholeSaleQuantityType = result.productWholeSaleQuantityType;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._productWholeSaleQuantityTypesServiceProxy
            .createOrEdit(this.productWholeSaleQuantityType)
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
