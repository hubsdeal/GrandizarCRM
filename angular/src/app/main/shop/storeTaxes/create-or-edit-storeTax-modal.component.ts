import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StoreTaxesServiceProxy, CreateOrEditStoreTaxDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreTaxStoreLookupTableModalComponent } from './storeTax-store-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreTaxModal',
    templateUrl: './create-or-edit-storeTax-modal.component.html',
})
export class CreateOrEditStoreTaxModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeTaxStoreLookupTableModal', { static: true })
    storeTaxStoreLookupTableModal: StoreTaxStoreLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeTax: CreateOrEditStoreTaxDto = new CreateOrEditStoreTaxDto();

    storeName = '';

    constructor(
        injector: Injector,
        private _storeTaxesServiceProxy: StoreTaxesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeTaxId?: number): void {
        if (!storeTaxId) {
            this.storeTax = new CreateOrEditStoreTaxDto();
            this.storeTax.id = storeTaxId;
            this.storeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeTaxesServiceProxy.getStoreTaxForEdit(storeTaxId).subscribe((result) => {
                this.storeTax = result.storeTax;

                this.storeName = result.storeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._storeTaxesServiceProxy
            .createOrEdit(this.storeTax)
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
        this.storeTaxStoreLookupTableModal.id = this.storeTax.storeId;
        this.storeTaxStoreLookupTableModal.displayName = this.storeName;
        this.storeTaxStoreLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeTax.storeId = null;
        this.storeName = '';
    }

    getNewStoreId() {
        this.storeTax.storeId = this.storeTaxStoreLookupTableModal.id;
        this.storeName = this.storeTaxStoreLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
