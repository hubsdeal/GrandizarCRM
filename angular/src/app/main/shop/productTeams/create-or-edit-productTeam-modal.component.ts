import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ProductTeamsServiceProxy, CreateOrEditProductTeamDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductTeamEmployeeLookupTableModalComponent } from './productTeam-employee-lookup-table-modal.component';
import { ProductTeamProductLookupTableModalComponent } from './productTeam-product-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductTeamModal',
    templateUrl: './create-or-edit-productTeam-modal.component.html',
})
export class CreateOrEditProductTeamModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productTeamEmployeeLookupTableModal', { static: true })
    productTeamEmployeeLookupTableModal: ProductTeamEmployeeLookupTableModalComponent;
    @ViewChild('productTeamProductLookupTableModal', { static: true })
    productTeamProductLookupTableModal: ProductTeamProductLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productTeam: CreateOrEditProductTeamDto = new CreateOrEditProductTeamDto();

    employeeName = '';
    productName = '';

    constructor(
        injector: Injector,
        private _productTeamsServiceProxy: ProductTeamsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productTeamId?: number): void {
        if (!productTeamId) {
            this.productTeam = new CreateOrEditProductTeamDto();
            this.productTeam.id = productTeamId;
            this.employeeName = '';
            this.productName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productTeamsServiceProxy.getProductTeamForEdit(productTeamId).subscribe((result) => {
                this.productTeam = result.productTeam;

                this.employeeName = result.employeeName;
                this.productName = result.productName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._productTeamsServiceProxy
            .createOrEdit(this.productTeam)
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

    openSelectEmployeeModal() {
        this.productTeamEmployeeLookupTableModal.id = this.productTeam.employeeId;
        this.productTeamEmployeeLookupTableModal.displayName = this.employeeName;
        this.productTeamEmployeeLookupTableModal.show();
    }
    openSelectProductModal() {
        this.productTeamProductLookupTableModal.id = this.productTeam.productId;
        this.productTeamProductLookupTableModal.displayName = this.productName;
        this.productTeamProductLookupTableModal.show();
    }

    setEmployeeIdNull() {
        this.productTeam.employeeId = null;
        this.employeeName = '';
    }
    setProductIdNull() {
        this.productTeam.productId = null;
        this.productName = '';
    }

    getNewEmployeeId() {
        this.productTeam.employeeId = this.productTeamEmployeeLookupTableModal.id;
        this.employeeName = this.productTeamEmployeeLookupTableModal.displayName;
    }
    getNewProductId() {
        this.productTeam.productId = this.productTeamProductLookupTableModal.id;
        this.productName = this.productTeamProductLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
