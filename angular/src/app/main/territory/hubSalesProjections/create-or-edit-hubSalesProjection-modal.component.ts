import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    HubSalesProjectionsServiceProxy,
    CreateOrEditHubSalesProjectionDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { HubSalesProjectionHubLookupTableModalComponent } from './hubSalesProjection-hub-lookup-table-modal.component';
import { HubSalesProjectionProductCategoryLookupTableModalComponent } from './hubSalesProjection-productCategory-lookup-table-modal.component';
import { HubSalesProjectionStoreLookupTableModalComponent } from './hubSalesProjection-store-lookup-table-modal.component';
import { HubSalesProjectionCurrencyLookupTableModalComponent } from './hubSalesProjection-currency-lookup-table-modal.component';

@Component({
    selector: 'createOrEditHubSalesProjectionModal',
    templateUrl: './create-or-edit-hubSalesProjection-modal.component.html',
})
export class CreateOrEditHubSalesProjectionModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('hubSalesProjectionHubLookupTableModal', { static: true })
    hubSalesProjectionHubLookupTableModal: HubSalesProjectionHubLookupTableModalComponent;
    @ViewChild('hubSalesProjectionProductCategoryLookupTableModal', { static: true })
    hubSalesProjectionProductCategoryLookupTableModal: HubSalesProjectionProductCategoryLookupTableModalComponent;
    @ViewChild('hubSalesProjectionStoreLookupTableModal', { static: true })
    hubSalesProjectionStoreLookupTableModal: HubSalesProjectionStoreLookupTableModalComponent;
    @ViewChild('hubSalesProjectionCurrencyLookupTableModal', { static: true })
    hubSalesProjectionCurrencyLookupTableModal: HubSalesProjectionCurrencyLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    hubSalesProjection: CreateOrEditHubSalesProjectionDto = new CreateOrEditHubSalesProjectionDto();

    hubName = '';
    productCategoryName = '';
    storeName = '';
    currencyName = '';

    constructor(
        injector: Injector,
        private _hubSalesProjectionsServiceProxy: HubSalesProjectionsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(hubSalesProjectionId?: number): void {
        if (!hubSalesProjectionId) {
            this.hubSalesProjection = new CreateOrEditHubSalesProjectionDto();
            this.hubSalesProjection.id = hubSalesProjectionId;
            this.hubSalesProjection.startDate = this._dateTimeService.getStartOfDay();
            this.hubSalesProjection.endDate = this._dateTimeService.getStartOfDay();
            this.hubName = '';
            this.productCategoryName = '';
            this.storeName = '';
            this.currencyName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._hubSalesProjectionsServiceProxy
                .getHubSalesProjectionForEdit(hubSalesProjectionId)
                .subscribe((result) => {
                    this.hubSalesProjection = result.hubSalesProjection;

                    this.hubName = result.hubName;
                    this.productCategoryName = result.productCategoryName;
                    this.storeName = result.storeName;
                    this.currencyName = result.currencyName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._hubSalesProjectionsServiceProxy
            .createOrEdit(this.hubSalesProjection)
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

    openSelectHubModal() {
        this.hubSalesProjectionHubLookupTableModal.id = this.hubSalesProjection.hubId;
        this.hubSalesProjectionHubLookupTableModal.displayName = this.hubName;
        this.hubSalesProjectionHubLookupTableModal.show();
    }
    openSelectProductCategoryModal() {
        this.hubSalesProjectionProductCategoryLookupTableModal.id = this.hubSalesProjection.productCategoryId;
        this.hubSalesProjectionProductCategoryLookupTableModal.displayName = this.productCategoryName;
        this.hubSalesProjectionProductCategoryLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.hubSalesProjectionStoreLookupTableModal.id = this.hubSalesProjection.storeId;
        this.hubSalesProjectionStoreLookupTableModal.displayName = this.storeName;
        this.hubSalesProjectionStoreLookupTableModal.show();
    }
    openSelectCurrencyModal() {
        this.hubSalesProjectionCurrencyLookupTableModal.id = this.hubSalesProjection.currencyId;
        this.hubSalesProjectionCurrencyLookupTableModal.displayName = this.currencyName;
        this.hubSalesProjectionCurrencyLookupTableModal.show();
    }

    setHubIdNull() {
        this.hubSalesProjection.hubId = null;
        this.hubName = '';
    }
    setProductCategoryIdNull() {
        this.hubSalesProjection.productCategoryId = null;
        this.productCategoryName = '';
    }
    setStoreIdNull() {
        this.hubSalesProjection.storeId = null;
        this.storeName = '';
    }
    setCurrencyIdNull() {
        this.hubSalesProjection.currencyId = null;
        this.currencyName = '';
    }

    getNewHubId() {
        this.hubSalesProjection.hubId = this.hubSalesProjectionHubLookupTableModal.id;
        this.hubName = this.hubSalesProjectionHubLookupTableModal.displayName;
    }
    getNewProductCategoryId() {
        this.hubSalesProjection.productCategoryId = this.hubSalesProjectionProductCategoryLookupTableModal.id;
        this.productCategoryName = this.hubSalesProjectionProductCategoryLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.hubSalesProjection.storeId = this.hubSalesProjectionStoreLookupTableModal.id;
        this.storeName = this.hubSalesProjectionStoreLookupTableModal.displayName;
    }
    getNewCurrencyId() {
        this.hubSalesProjection.currencyId = this.hubSalesProjectionCurrencyLookupTableModal.id;
        this.currencyName = this.hubSalesProjectionCurrencyLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
