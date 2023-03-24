import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    MarketplaceCommissionTypesServiceProxy,
    CreateOrEditMarketplaceCommissionTypeDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditMarketplaceCommissionTypeModal',
    templateUrl: './create-or-edit-marketplaceCommissionType-modal.component.html',
})
export class CreateOrEditMarketplaceCommissionTypeModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    marketplaceCommissionType: CreateOrEditMarketplaceCommissionTypeDto =
        new CreateOrEditMarketplaceCommissionTypeDto();

    constructor(
        injector: Injector,
        private _marketplaceCommissionTypesServiceProxy: MarketplaceCommissionTypesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(marketplaceCommissionTypeId?: number): void {
        if (!marketplaceCommissionTypeId) {
            this.marketplaceCommissionType = new CreateOrEditMarketplaceCommissionTypeDto();
            this.marketplaceCommissionType.id = marketplaceCommissionTypeId;

            this.active = true;
            this.modal.show();
        } else {
            this._marketplaceCommissionTypesServiceProxy
                .getMarketplaceCommissionTypeForEdit(marketplaceCommissionTypeId)
                .subscribe((result) => {
                    this.marketplaceCommissionType = result.marketplaceCommissionType;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._marketplaceCommissionTypesServiceProxy
            .createOrEdit(this.marketplaceCommissionType)
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
