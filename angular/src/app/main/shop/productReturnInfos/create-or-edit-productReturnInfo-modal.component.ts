import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductReturnInfosServiceProxy,
    CreateOrEditProductReturnInfoDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductReturnInfoProductLookupTableModalComponent } from './productReturnInfo-product-lookup-table-modal.component';
import { ProductReturnInfoReturnTypeLookupTableModalComponent } from './productReturnInfo-returnType-lookup-table-modal.component';
import { ProductReturnInfoReturnStatusLookupTableModalComponent } from './productReturnInfo-returnStatus-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductReturnInfoModal',
    templateUrl: './create-or-edit-productReturnInfo-modal.component.html',
})
export class CreateOrEditProductReturnInfoModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productReturnInfoProductLookupTableModal', { static: true })
    productReturnInfoProductLookupTableModal: ProductReturnInfoProductLookupTableModalComponent;
    @ViewChild('productReturnInfoReturnTypeLookupTableModal', { static: true })
    productReturnInfoReturnTypeLookupTableModal: ProductReturnInfoReturnTypeLookupTableModalComponent;
    @ViewChild('productReturnInfoReturnStatusLookupTableModal', { static: true })
    productReturnInfoReturnStatusLookupTableModal: ProductReturnInfoReturnStatusLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productReturnInfo: CreateOrEditProductReturnInfoDto = new CreateOrEditProductReturnInfoDto();

    productName = '';
    returnTypeName = '';
    returnStatusName = '';

    constructor(
        injector: Injector,
        private _productReturnInfosServiceProxy: ProductReturnInfosServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productReturnInfoId?: number): void {
        if (!productReturnInfoId) {
            this.productReturnInfo = new CreateOrEditProductReturnInfoDto();
            this.productReturnInfo.id = productReturnInfoId;
            this.productName = '';
            this.returnTypeName = '';
            this.returnStatusName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productReturnInfosServiceProxy
                .getProductReturnInfoForEdit(productReturnInfoId)
                .subscribe((result) => {
                    this.productReturnInfo = result.productReturnInfo;

                    this.productName = result.productName;
                    this.returnTypeName = result.returnTypeName;
                    this.returnStatusName = result.returnStatusName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._productReturnInfosServiceProxy
            .createOrEdit(this.productReturnInfo)
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

    openSelectProductModal() {
        this.productReturnInfoProductLookupTableModal.id = this.productReturnInfo.productId;
        this.productReturnInfoProductLookupTableModal.displayName = this.productName;
        this.productReturnInfoProductLookupTableModal.show();
    }
    openSelectReturnTypeModal() {
        this.productReturnInfoReturnTypeLookupTableModal.id = this.productReturnInfo.returnTypeId;
        this.productReturnInfoReturnTypeLookupTableModal.displayName = this.returnTypeName;
        this.productReturnInfoReturnTypeLookupTableModal.show();
    }
    openSelectReturnStatusModal() {
        this.productReturnInfoReturnStatusLookupTableModal.id = this.productReturnInfo.returnStatusId;
        this.productReturnInfoReturnStatusLookupTableModal.displayName = this.returnStatusName;
        this.productReturnInfoReturnStatusLookupTableModal.show();
    }

    setProductIdNull() {
        this.productReturnInfo.productId = null;
        this.productName = '';
    }
    setReturnTypeIdNull() {
        this.productReturnInfo.returnTypeId = null;
        this.returnTypeName = '';
    }
    setReturnStatusIdNull() {
        this.productReturnInfo.returnStatusId = null;
        this.returnStatusName = '';
    }

    getNewProductId() {
        this.productReturnInfo.productId = this.productReturnInfoProductLookupTableModal.id;
        this.productName = this.productReturnInfoProductLookupTableModal.displayName;
    }
    getNewReturnTypeId() {
        this.productReturnInfo.returnTypeId = this.productReturnInfoReturnTypeLookupTableModal.id;
        this.returnTypeName = this.productReturnInfoReturnTypeLookupTableModal.displayName;
    }
    getNewReturnStatusId() {
        this.productReturnInfo.returnStatusId = this.productReturnInfoReturnStatusLookupTableModal.id;
        this.returnStatusName = this.productReturnInfoReturnStatusLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
