import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductCustomerQueriesServiceProxy,
    CreateOrEditProductCustomerQueryDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductCustomerQueryProductLookupTableModalComponent } from './productCustomerQuery-product-lookup-table-modal.component';
import { ProductCustomerQueryContactLookupTableModalComponent } from './productCustomerQuery-contact-lookup-table-modal.component';
import { ProductCustomerQueryEmployeeLookupTableModalComponent } from './productCustomerQuery-employee-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductCustomerQueryModal',
    templateUrl: './create-or-edit-productCustomerQuery-modal.component.html',
})
export class CreateOrEditProductCustomerQueryModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productCustomerQueryProductLookupTableModal', { static: true })
    productCustomerQueryProductLookupTableModal: ProductCustomerQueryProductLookupTableModalComponent;
    @ViewChild('productCustomerQueryContactLookupTableModal', { static: true })
    productCustomerQueryContactLookupTableModal: ProductCustomerQueryContactLookupTableModalComponent;
    @ViewChild('productCustomerQueryEmployeeLookupTableModal', { static: true })
    productCustomerQueryEmployeeLookupTableModal: ProductCustomerQueryEmployeeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productCustomerQuery: CreateOrEditProductCustomerQueryDto = new CreateOrEditProductCustomerQueryDto();

    productName = '';
    contactFullName = '';
    employeeName = '';

    constructor(
        injector: Injector,
        private _productCustomerQueriesServiceProxy: ProductCustomerQueriesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productCustomerQueryId?: number): void {
        if (!productCustomerQueryId) {
            this.productCustomerQuery = new CreateOrEditProductCustomerQueryDto();
            this.productCustomerQuery.id = productCustomerQueryId;
            this.productCustomerQuery.answerDate = this._dateTimeService.getStartOfDay();
            this.productName = '';
            this.contactFullName = '';
            this.employeeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productCustomerQueriesServiceProxy
                .getProductCustomerQueryForEdit(productCustomerQueryId)
                .subscribe((result) => {
                    this.productCustomerQuery = result.productCustomerQuery;

                    this.productName = result.productName;
                    this.contactFullName = result.contactFullName;
                    this.employeeName = result.employeeName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._productCustomerQueriesServiceProxy
            .createOrEdit(this.productCustomerQuery)
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
        this.productCustomerQueryProductLookupTableModal.id = this.productCustomerQuery.productId;
        this.productCustomerQueryProductLookupTableModal.displayName = this.productName;
        this.productCustomerQueryProductLookupTableModal.show();
    }
    openSelectContactModal() {
        this.productCustomerQueryContactLookupTableModal.id = this.productCustomerQuery.contactId;
        this.productCustomerQueryContactLookupTableModal.displayName = this.contactFullName;
        this.productCustomerQueryContactLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.productCustomerQueryEmployeeLookupTableModal.id = this.productCustomerQuery.employeeId;
        this.productCustomerQueryEmployeeLookupTableModal.displayName = this.employeeName;
        this.productCustomerQueryEmployeeLookupTableModal.show();
    }

    setProductIdNull() {
        this.productCustomerQuery.productId = null;
        this.productName = '';
    }
    setContactIdNull() {
        this.productCustomerQuery.contactId = null;
        this.contactFullName = '';
    }
    setEmployeeIdNull() {
        this.productCustomerQuery.employeeId = null;
        this.employeeName = '';
    }

    getNewProductId() {
        this.productCustomerQuery.productId = this.productCustomerQueryProductLookupTableModal.id;
        this.productName = this.productCustomerQueryProductLookupTableModal.displayName;
    }
    getNewContactId() {
        this.productCustomerQuery.contactId = this.productCustomerQueryContactLookupTableModal.id;
        this.contactFullName = this.productCustomerQueryContactLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.productCustomerQuery.employeeId = this.productCustomerQueryEmployeeLookupTableModal.id;
        this.employeeName = this.productCustomerQueryEmployeeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
