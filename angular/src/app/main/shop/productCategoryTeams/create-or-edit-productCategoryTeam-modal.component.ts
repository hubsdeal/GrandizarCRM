import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductCategoryTeamsServiceProxy,
    CreateOrEditProductCategoryTeamDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductCategoryTeamProductCategoryLookupTableModalComponent } from './productCategoryTeam-productCategory-lookup-table-modal.component';
import { ProductCategoryTeamEmployeeLookupTableModalComponent } from './productCategoryTeam-employee-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductCategoryTeamModal',
    templateUrl: './create-or-edit-productCategoryTeam-modal.component.html',
})
export class CreateOrEditProductCategoryTeamModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productCategoryTeamProductCategoryLookupTableModal', { static: true })
    productCategoryTeamProductCategoryLookupTableModal: ProductCategoryTeamProductCategoryLookupTableModalComponent;
    @ViewChild('productCategoryTeamEmployeeLookupTableModal', { static: true })
    productCategoryTeamEmployeeLookupTableModal: ProductCategoryTeamEmployeeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productCategoryTeam: CreateOrEditProductCategoryTeamDto = new CreateOrEditProductCategoryTeamDto();

    productCategoryName = '';
    employeeName = '';

    constructor(
        injector: Injector,
        private _productCategoryTeamsServiceProxy: ProductCategoryTeamsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productCategoryTeamId?: number): void {
        if (!productCategoryTeamId) {
            this.productCategoryTeam = new CreateOrEditProductCategoryTeamDto();
            this.productCategoryTeam.id = productCategoryTeamId;
            this.productCategoryName = '';
            this.employeeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productCategoryTeamsServiceProxy
                .getProductCategoryTeamForEdit(productCategoryTeamId)
                .subscribe((result) => {
                    this.productCategoryTeam = result.productCategoryTeam;

                    this.productCategoryName = result.productCategoryName;
                    this.employeeName = result.employeeName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._productCategoryTeamsServiceProxy
            .createOrEdit(this.productCategoryTeam)
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

    openSelectProductCategoryModal() {
        this.productCategoryTeamProductCategoryLookupTableModal.id = this.productCategoryTeam.productCategoryId;
        this.productCategoryTeamProductCategoryLookupTableModal.displayName = this.productCategoryName;
        this.productCategoryTeamProductCategoryLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.productCategoryTeamEmployeeLookupTableModal.id = this.productCategoryTeam.employeeId;
        this.productCategoryTeamEmployeeLookupTableModal.displayName = this.employeeName;
        this.productCategoryTeamEmployeeLookupTableModal.show();
    }

    setProductCategoryIdNull() {
        this.productCategoryTeam.productCategoryId = null;
        this.productCategoryName = '';
    }
    setEmployeeIdNull() {
        this.productCategoryTeam.employeeId = null;
        this.employeeName = '';
    }

    getNewProductCategoryId() {
        this.productCategoryTeam.productCategoryId = this.productCategoryTeamProductCategoryLookupTableModal.id;
        this.productCategoryName = this.productCategoryTeamProductCategoryLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.productCategoryTeam.employeeId = this.productCategoryTeamEmployeeLookupTableModal.id;
        this.employeeName = this.productCategoryTeamEmployeeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
