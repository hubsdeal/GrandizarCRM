import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProductTaskMapsServiceProxy, CreateOrEditProductTaskMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductTaskMapProductLookupTableModalComponent } from './productTaskMap-product-lookup-table-modal.component';
import { ProductTaskMapTaskEventLookupTableModalComponent } from './productTaskMap-taskEvent-lookup-table-modal.component';
import { ProductTaskMapProductCategoryLookupTableModalComponent } from './productTaskMap-productCategory-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductTaskMapModal',
    templateUrl: './create-or-edit-productTaskMap-modal.component.html',
})
export class CreateOrEditProductTaskMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productTaskMapProductLookupTableModal', { static: true })
    productTaskMapProductLookupTableModal: ProductTaskMapProductLookupTableModalComponent;
    @ViewChild('productTaskMapTaskEventLookupTableModal', { static: true })
    productTaskMapTaskEventLookupTableModal: ProductTaskMapTaskEventLookupTableModalComponent;
    @ViewChild('productTaskMapProductCategoryLookupTableModal', { static: true })
    productTaskMapProductCategoryLookupTableModal: ProductTaskMapProductCategoryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productTaskMap: CreateOrEditProductTaskMapDto = new CreateOrEditProductTaskMapDto();

    productName = '';
    taskEventName = '';
    productCategoryName = '';

    constructor(
        injector: Injector,
        private _productTaskMapsServiceProxy: ProductTaskMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productTaskMapId?: number): void {
        if (!productTaskMapId) {
            this.productTaskMap = new CreateOrEditProductTaskMapDto();
            this.productTaskMap.id = productTaskMapId;
            this.productName = '';
            this.taskEventName = '';
            this.productCategoryName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productTaskMapsServiceProxy.getProductTaskMapForEdit(productTaskMapId).subscribe((result) => {
                this.productTaskMap = result.productTaskMap;

                this.productName = result.productName;
                this.taskEventName = result.taskEventName;
                this.productCategoryName = result.productCategoryName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._productTaskMapsServiceProxy
            .createOrEdit(this.productTaskMap)
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
        this.productTaskMapProductLookupTableModal.id = this.productTaskMap.productId;
        this.productTaskMapProductLookupTableModal.displayName = this.productName;
        this.productTaskMapProductLookupTableModal.show();
    }
    openSelectTaskEventModal() {
        this.productTaskMapTaskEventLookupTableModal.id = this.productTaskMap.taskEventId;
        this.productTaskMapTaskEventLookupTableModal.displayName = this.taskEventName;
        this.productTaskMapTaskEventLookupTableModal.show();
    }
    openSelectProductCategoryModal() {
        this.productTaskMapProductCategoryLookupTableModal.id = this.productTaskMap.productCategoryId;
        this.productTaskMapProductCategoryLookupTableModal.displayName = this.productCategoryName;
        this.productTaskMapProductCategoryLookupTableModal.show();
    }

    setProductIdNull() {
        this.productTaskMap.productId = null;
        this.productName = '';
    }
    setTaskEventIdNull() {
        this.productTaskMap.taskEventId = null;
        this.taskEventName = '';
    }
    setProductCategoryIdNull() {
        this.productTaskMap.productCategoryId = null;
        this.productCategoryName = '';
    }

    getNewProductId() {
        this.productTaskMap.productId = this.productTaskMapProductLookupTableModal.id;
        this.productName = this.productTaskMapProductLookupTableModal.displayName;
    }
    getNewTaskEventId() {
        this.productTaskMap.taskEventId = this.productTaskMapTaskEventLookupTableModal.id;
        this.taskEventName = this.productTaskMapTaskEventLookupTableModal.displayName;
    }
    getNewProductCategoryId() {
        this.productTaskMap.productCategoryId = this.productTaskMapProductCategoryLookupTableModal.id;
        this.productCategoryName = this.productTaskMapProductCategoryLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
