import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    HubProductCategoriesServiceProxy,
    CreateOrEditHubProductCategoryDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { HubProductCategoryHubLookupTableModalComponent } from './hubProductCategory-hub-lookup-table-modal.component';
import { HubProductCategoryProductCategoryLookupTableModalComponent } from './hubProductCategory-productCategory-lookup-table-modal.component';

@Component({
    selector: 'createOrEditHubProductCategoryModal',
    templateUrl: './create-or-edit-hubProductCategory-modal.component.html',
})
export class CreateOrEditHubProductCategoryModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('hubProductCategoryHubLookupTableModal', { static: true })
    hubProductCategoryHubLookupTableModal: HubProductCategoryHubLookupTableModalComponent;
    @ViewChild('hubProductCategoryProductCategoryLookupTableModal', { static: true })
    hubProductCategoryProductCategoryLookupTableModal: HubProductCategoryProductCategoryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    hubProductCategory: CreateOrEditHubProductCategoryDto = new CreateOrEditHubProductCategoryDto();

    hubName = '';
    productCategoryName = '';

    constructor(
        injector: Injector,
        private _hubProductCategoriesServiceProxy: HubProductCategoriesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(hubProductCategoryId?: number): void {
        if (!hubProductCategoryId) {
            this.hubProductCategory = new CreateOrEditHubProductCategoryDto();
            this.hubProductCategory.id = hubProductCategoryId;
            this.hubName = '';
            this.productCategoryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._hubProductCategoriesServiceProxy
                .getHubProductCategoryForEdit(hubProductCategoryId)
                .subscribe((result) => {
                    this.hubProductCategory = result.hubProductCategory;

                    this.hubName = result.hubName;
                    this.productCategoryName = result.productCategoryName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._hubProductCategoriesServiceProxy
            .createOrEdit(this.hubProductCategory)
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
        this.hubProductCategoryHubLookupTableModal.id = this.hubProductCategory.hubId;
        this.hubProductCategoryHubLookupTableModal.displayName = this.hubName;
        this.hubProductCategoryHubLookupTableModal.show();
    }
    openSelectProductCategoryModal() {
        this.hubProductCategoryProductCategoryLookupTableModal.id = this.hubProductCategory.productCategoryId;
        this.hubProductCategoryProductCategoryLookupTableModal.displayName = this.productCategoryName;
        this.hubProductCategoryProductCategoryLookupTableModal.show();
    }

    setHubIdNull() {
        this.hubProductCategory.hubId = null;
        this.hubName = '';
    }
    setProductCategoryIdNull() {
        this.hubProductCategory.productCategoryId = null;
        this.productCategoryName = '';
    }

    getNewHubId() {
        this.hubProductCategory.hubId = this.hubProductCategoryHubLookupTableModal.id;
        this.hubName = this.hubProductCategoryHubLookupTableModal.displayName;
    }
    getNewProductCategoryId() {
        this.hubProductCategory.productCategoryId = this.hubProductCategoryProductCategoryLookupTableModal.id;
        this.productCategoryName = this.hubProductCategoryProductCategoryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
