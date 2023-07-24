import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { HubProductsServiceProxy, CreateOrEditHubProductDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { HubProductHubLookupTableModalComponent } from './hubProduct-hub-lookup-table-modal.component';
import { HubProductProductLookupTableModalComponent } from './hubProduct-product-lookup-table-modal.component';

@Component({
    selector: 'createOrEditHubProductModal',
    templateUrl: './create-or-edit-hubProduct-modal.component.html',
})
export class CreateOrEditHubProductModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('hubProductHubLookupTableModal', { static: true })
    hubProductHubLookupTableModal: HubProductHubLookupTableModalComponent;
    @ViewChild('hubProductProductLookupTableModal', { static: true })
    hubProductProductLookupTableModal: HubProductProductLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    hubProduct: CreateOrEditHubProductDto = new CreateOrEditHubProductDto();

    hubName = '';
    productName = '';
    hubId:number;

    constructor(
        injector: Injector,
        private _hubProductsServiceProxy: HubProductsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(hubProductId?: number): void {
        if (!hubProductId) {
            this.hubProduct = new CreateOrEditHubProductDto();
            this.hubProduct.id = hubProductId;
            this.hubName = '';
            this.productName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._hubProductsServiceProxy.getHubProductForEdit(hubProductId).subscribe((result) => {
                this.hubProduct = result.hubProduct;
                this.hubId = result.hubProduct.hubId;
                this.hubName = result.hubName;
                this.productName = result.productName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        if(this.hubId != null){
            this.hubProduct.hubId = this.hubId;
        }
        this._hubProductsServiceProxy
            .createOrEdit(this.hubProduct)
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
        this.hubProductHubLookupTableModal.id = this.hubProduct.hubId;
        this.hubProductHubLookupTableModal.displayName = this.hubName;
        this.hubProductHubLookupTableModal.show();
    }
    openSelectProductModal() {
        this.hubProductProductLookupTableModal.id = this.hubProduct.productId;
        this.hubProductProductLookupTableModal.displayName = this.productName;
        this.hubProductProductLookupTableModal.show();
    }

    setHubIdNull() {
        this.hubProduct.hubId = null;
        this.hubName = '';
    }
    setProductIdNull() {
        this.hubProduct.productId = null;
        this.productName = '';
    }

    getNewHubId() {
        this.hubProduct.hubId = this.hubProductHubLookupTableModal.id;
        this.hubName = this.hubProductHubLookupTableModal.displayName;
    }
    getNewProductId() {
        this.hubProduct.productId = this.hubProductProductLookupTableModal.id;
        this.productName = this.hubProductProductLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
