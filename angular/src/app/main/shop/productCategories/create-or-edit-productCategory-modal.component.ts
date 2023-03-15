import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProductCategoriesServiceProxy, CreateOrEditProductCategoryDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductCategoryMediaLibraryLookupTableModalComponent } from './productCategory-mediaLibrary-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductCategoryModal',
    templateUrl: './create-or-edit-productCategory-modal.component.html',
})
export class CreateOrEditProductCategoryModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productCategoryMediaLibraryLookupTableModal', { static: true })
    productCategoryMediaLibraryLookupTableModal: ProductCategoryMediaLibraryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productCategory: CreateOrEditProductCategoryDto = new CreateOrEditProductCategoryDto();

    mediaLibraryName = '';

    constructor(
        injector: Injector,
        private _productCategoriesServiceProxy: ProductCategoriesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productCategoryId?: number): void {
        if (!productCategoryId) {
            this.productCategory = new CreateOrEditProductCategoryDto();
            this.productCategory.id = productCategoryId;
            this.mediaLibraryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productCategoriesServiceProxy.getProductCategoryForEdit(productCategoryId).subscribe((result) => {
                this.productCategory = result.productCategory;

                this.mediaLibraryName = result.mediaLibraryName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._productCategoriesServiceProxy
            .createOrEdit(this.productCategory)
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

    openSelectMediaLibraryModal() {
        this.productCategoryMediaLibraryLookupTableModal.id = this.productCategory.mediaLibraryId;
        this.productCategoryMediaLibraryLookupTableModal.displayName = this.mediaLibraryName;
        this.productCategoryMediaLibraryLookupTableModal.show();
    }

    setMediaLibraryIdNull() {
        this.productCategory.mediaLibraryId = null;
        this.mediaLibraryName = '';
    }

    getNewMediaLibraryId() {
        this.productCategory.mediaLibraryId = this.productCategoryMediaLibraryLookupTableModal.id;
        this.mediaLibraryName = this.productCategoryMediaLibraryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
