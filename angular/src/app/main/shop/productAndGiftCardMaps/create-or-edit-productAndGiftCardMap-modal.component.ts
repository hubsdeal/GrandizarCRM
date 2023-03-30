import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductAndGiftCardMapsServiceProxy,
    CreateOrEditProductAndGiftCardMapDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductAndGiftCardMapProductLookupTableModalComponent } from './productAndGiftCardMap-product-lookup-table-modal.component';
import { ProductAndGiftCardMapCurrencyLookupTableModalComponent } from './productAndGiftCardMap-currency-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductAndGiftCardMapModal',
    templateUrl: './create-or-edit-productAndGiftCardMap-modal.component.html',
})
export class CreateOrEditProductAndGiftCardMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productAndGiftCardMapProductLookupTableModal', { static: true })
    productAndGiftCardMapProductLookupTableModal: ProductAndGiftCardMapProductLookupTableModalComponent;
    @ViewChild('productAndGiftCardMapCurrencyLookupTableModal', { static: true })
    productAndGiftCardMapCurrencyLookupTableModal: ProductAndGiftCardMapCurrencyLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productAndGiftCardMap: CreateOrEditProductAndGiftCardMapDto = new CreateOrEditProductAndGiftCardMapDto();

    productName = '';
    currencyName = '';

    constructor(
        injector: Injector,
        private _productAndGiftCardMapsServiceProxy: ProductAndGiftCardMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productAndGiftCardMapId?: number): void {
        if (!productAndGiftCardMapId) {
            this.productAndGiftCardMap = new CreateOrEditProductAndGiftCardMapDto();
            this.productAndGiftCardMap.id = productAndGiftCardMapId;
            this.productName = '';
            this.currencyName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productAndGiftCardMapsServiceProxy
                .getProductAndGiftCardMapForEdit(productAndGiftCardMapId)
                .subscribe((result) => {
                    this.productAndGiftCardMap = result.productAndGiftCardMap;

                    this.productName = result.productName;
                    this.currencyName = result.currencyName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._productAndGiftCardMapsServiceProxy
            .createOrEdit(this.productAndGiftCardMap)
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
        this.productAndGiftCardMapProductLookupTableModal.id = this.productAndGiftCardMap.productId;
        this.productAndGiftCardMapProductLookupTableModal.displayName = this.productName;
        this.productAndGiftCardMapProductLookupTableModal.show();
    }
    openSelectCurrencyModal() {
        this.productAndGiftCardMapCurrencyLookupTableModal.id = this.productAndGiftCardMap.currencyId;
        this.productAndGiftCardMapCurrencyLookupTableModal.displayName = this.currencyName;
        this.productAndGiftCardMapCurrencyLookupTableModal.show();
    }

    setProductIdNull() {
        this.productAndGiftCardMap.productId = null;
        this.productName = '';
    }
    setCurrencyIdNull() {
        this.productAndGiftCardMap.currencyId = null;
        this.currencyName = '';
    }

    getNewProductId() {
        this.productAndGiftCardMap.productId = this.productAndGiftCardMapProductLookupTableModal.id;
        this.productName = this.productAndGiftCardMapProductLookupTableModal.displayName;
    }
    getNewCurrencyId() {
        this.productAndGiftCardMap.currencyId = this.productAndGiftCardMapCurrencyLookupTableModal.id;
        this.currencyName = this.productAndGiftCardMapCurrencyLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
