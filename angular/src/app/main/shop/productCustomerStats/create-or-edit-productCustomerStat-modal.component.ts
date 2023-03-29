import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductCustomerStatsServiceProxy,
    CreateOrEditProductCustomerStatDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductCustomerStatProductLookupTableModalComponent } from './productCustomerStat-product-lookup-table-modal.component';
import { ProductCustomerStatContactLookupTableModalComponent } from './productCustomerStat-contact-lookup-table-modal.component';
import { ProductCustomerStatStoreLookupTableModalComponent } from './productCustomerStat-store-lookup-table-modal.component';
import { ProductCustomerStatHubLookupTableModalComponent } from './productCustomerStat-hub-lookup-table-modal.component';
import { ProductCustomerStatSocialMediaLookupTableModalComponent } from './productCustomerStat-socialMedia-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductCustomerStatModal',
    templateUrl: './create-or-edit-productCustomerStat-modal.component.html',
})
export class CreateOrEditProductCustomerStatModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productCustomerStatProductLookupTableModal', { static: true })
    productCustomerStatProductLookupTableModal: ProductCustomerStatProductLookupTableModalComponent;
    @ViewChild('productCustomerStatContactLookupTableModal', { static: true })
    productCustomerStatContactLookupTableModal: ProductCustomerStatContactLookupTableModalComponent;
    @ViewChild('productCustomerStatStoreLookupTableModal', { static: true })
    productCustomerStatStoreLookupTableModal: ProductCustomerStatStoreLookupTableModalComponent;
    @ViewChild('productCustomerStatHubLookupTableModal', { static: true })
    productCustomerStatHubLookupTableModal: ProductCustomerStatHubLookupTableModalComponent;
    @ViewChild('productCustomerStatSocialMediaLookupTableModal', { static: true })
    productCustomerStatSocialMediaLookupTableModal: ProductCustomerStatSocialMediaLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productCustomerStat: CreateOrEditProductCustomerStatDto = new CreateOrEditProductCustomerStatDto();

    productName = '';
    contactFullName = '';
    storeName = '';
    hubName = '';
    socialMediaName = '';

    constructor(
        injector: Injector,
        private _productCustomerStatsServiceProxy: ProductCustomerStatsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productCustomerStatId?: number): void {
        if (!productCustomerStatId) {
            this.productCustomerStat = new CreateOrEditProductCustomerStatDto();
            this.productCustomerStat.id = productCustomerStatId;
            this.productName = '';
            this.contactFullName = '';
            this.storeName = '';
            this.hubName = '';
            this.socialMediaName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productCustomerStatsServiceProxy
                .getProductCustomerStatForEdit(productCustomerStatId)
                .subscribe((result) => {
                    this.productCustomerStat = result.productCustomerStat;

                    this.productName = result.productName;
                    this.contactFullName = result.contactFullName;
                    this.storeName = result.storeName;
                    this.hubName = result.hubName;
                    this.socialMediaName = result.socialMediaName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._productCustomerStatsServiceProxy
            .createOrEdit(this.productCustomerStat)
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
        this.productCustomerStatProductLookupTableModal.id = this.productCustomerStat.productId;
        this.productCustomerStatProductLookupTableModal.displayName = this.productName;
        this.productCustomerStatProductLookupTableModal.show();
    }
    openSelectContactModal() {
        this.productCustomerStatContactLookupTableModal.id = this.productCustomerStat.contactId;
        this.productCustomerStatContactLookupTableModal.displayName = this.contactFullName;
        this.productCustomerStatContactLookupTableModal.show();
    }
    openSelectStoreModal() {
        this.productCustomerStatStoreLookupTableModal.id = this.productCustomerStat.storeId;
        this.productCustomerStatStoreLookupTableModal.displayName = this.storeName;
        this.productCustomerStatStoreLookupTableModal.show();
    }
    openSelectHubModal() {
        this.productCustomerStatHubLookupTableModal.id = this.productCustomerStat.hubId;
        this.productCustomerStatHubLookupTableModal.displayName = this.hubName;
        this.productCustomerStatHubLookupTableModal.show();
    }
    openSelectSocialMediaModal() {
        this.productCustomerStatSocialMediaLookupTableModal.id = this.productCustomerStat.socialMediaId;
        this.productCustomerStatSocialMediaLookupTableModal.displayName = this.socialMediaName;
        this.productCustomerStatSocialMediaLookupTableModal.show();
    }

    setProductIdNull() {
        this.productCustomerStat.productId = null;
        this.productName = '';
    }
    setContactIdNull() {
        this.productCustomerStat.contactId = null;
        this.contactFullName = '';
    }
    setStoreIdNull() {
        this.productCustomerStat.storeId = null;
        this.storeName = '';
    }
    setHubIdNull() {
        this.productCustomerStat.hubId = null;
        this.hubName = '';
    }
    setSocialMediaIdNull() {
        this.productCustomerStat.socialMediaId = null;
        this.socialMediaName = '';
    }

    getNewProductId() {
        this.productCustomerStat.productId = this.productCustomerStatProductLookupTableModal.id;
        this.productName = this.productCustomerStatProductLookupTableModal.displayName;
    }
    getNewContactId() {
        this.productCustomerStat.contactId = this.productCustomerStatContactLookupTableModal.id;
        this.contactFullName = this.productCustomerStatContactLookupTableModal.displayName;
    }
    getNewStoreId() {
        this.productCustomerStat.storeId = this.productCustomerStatStoreLookupTableModal.id;
        this.storeName = this.productCustomerStatStoreLookupTableModal.displayName;
    }
    getNewHubId() {
        this.productCustomerStat.hubId = this.productCustomerStatHubLookupTableModal.id;
        this.hubName = this.productCustomerStatHubLookupTableModal.displayName;
    }
    getNewSocialMediaId() {
        this.productCustomerStat.socialMediaId = this.productCustomerStatSocialMediaLookupTableModal.id;
        this.socialMediaName = this.productCustomerStatSocialMediaLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
