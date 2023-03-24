import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    BusinessProductMapsServiceProxy,
    CreateOrEditBusinessProductMapDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { BusinessProductMapBusinessLookupTableModalComponent } from './businessProductMap-business-lookup-table-modal.component';
import { BusinessProductMapProductLookupTableModalComponent } from './businessProductMap-product-lookup-table-modal.component';

@Component({
    selector: 'createOrEditBusinessProductMapModal',
    templateUrl: './create-or-edit-businessProductMap-modal.component.html',
})
export class CreateOrEditBusinessProductMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('businessProductMapBusinessLookupTableModal', { static: true })
    businessProductMapBusinessLookupTableModal: BusinessProductMapBusinessLookupTableModalComponent;
    @ViewChild('businessProductMapProductLookupTableModal', { static: true })
    businessProductMapProductLookupTableModal: BusinessProductMapProductLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    businessProductMap: CreateOrEditBusinessProductMapDto = new CreateOrEditBusinessProductMapDto();

    businessName = '';
    productName = '';

    constructor(
        injector: Injector,
        private _businessProductMapsServiceProxy: BusinessProductMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(businessProductMapId?: number): void {
        if (!businessProductMapId) {
            this.businessProductMap = new CreateOrEditBusinessProductMapDto();
            this.businessProductMap.id = businessProductMapId;
            this.businessName = '';
            this.productName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._businessProductMapsServiceProxy
                .getBusinessProductMapForEdit(businessProductMapId)
                .subscribe((result) => {
                    this.businessProductMap = result.businessProductMap;

                    this.businessName = result.businessName;
                    this.productName = result.productName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._businessProductMapsServiceProxy
            .createOrEdit(this.businessProductMap)
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

    openSelectBusinessModal() {
        this.businessProductMapBusinessLookupTableModal.id = this.businessProductMap.businessId;
        this.businessProductMapBusinessLookupTableModal.displayName = this.businessName;
        this.businessProductMapBusinessLookupTableModal.show();
    }
    openSelectProductModal() {
        this.businessProductMapProductLookupTableModal.id = this.businessProductMap.productId;
        this.businessProductMapProductLookupTableModal.displayName = this.productName;
        this.businessProductMapProductLookupTableModal.show();
    }

    setBusinessIdNull() {
        this.businessProductMap.businessId = null;
        this.businessName = '';
    }
    setProductIdNull() {
        this.businessProductMap.productId = null;
        this.productName = '';
    }

    getNewBusinessId() {
        this.businessProductMap.businessId = this.businessProductMapBusinessLookupTableModal.id;
        this.businessName = this.businessProductMapBusinessLookupTableModal.displayName;
    }
    getNewProductId() {
        this.businessProductMap.productId = this.businessProductMapProductLookupTableModal.id;
        this.productName = this.businessProductMapProductLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
