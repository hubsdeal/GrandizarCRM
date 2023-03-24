import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StoreSalesAlertsServiceProxy, CreateOrEditStoreSalesAlertDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreSalesAlertStoreLookupTableModalComponent } from './storeSalesAlert-store-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreSalesAlertModal',
    templateUrl: './create-or-edit-storeSalesAlert-modal.component.html',
})
export class CreateOrEditStoreSalesAlertModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeSalesAlertStoreLookupTableModal', { static: true })
    storeSalesAlertStoreLookupTableModal: StoreSalesAlertStoreLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeSalesAlert: CreateOrEditStoreSalesAlertDto = new CreateOrEditStoreSalesAlertDto();

    storeName = '';

    constructor(
        injector: Injector,
        private _storeSalesAlertsServiceProxy: StoreSalesAlertsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeSalesAlertId?: number): void {
        if (!storeSalesAlertId) {
            this.storeSalesAlert = new CreateOrEditStoreSalesAlertDto();
            this.storeSalesAlert.id = storeSalesAlertId;
            this.storeSalesAlert.startDate = this._dateTimeService.getStartOfDay();
            this.storeSalesAlert.endDate = this._dateTimeService.getStartOfDay();
            this.storeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeSalesAlertsServiceProxy.getStoreSalesAlertForEdit(storeSalesAlertId).subscribe((result) => {
                this.storeSalesAlert = result.storeSalesAlert;

                this.storeName = result.storeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._storeSalesAlertsServiceProxy
            .createOrEdit(this.storeSalesAlert)
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

    openSelectStoreModal() {
        this.storeSalesAlertStoreLookupTableModal.id = this.storeSalesAlert.storeId;
        this.storeSalesAlertStoreLookupTableModal.displayName = this.storeName;
        this.storeSalesAlertStoreLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeSalesAlert.storeId = null;
        this.storeName = '';
    }

    getNewStoreId() {
        this.storeSalesAlert.storeId = this.storeSalesAlertStoreLookupTableModal.id;
        this.storeName = this.storeSalesAlertStoreLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
