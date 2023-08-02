import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    ProductAccountTeamsServiceProxy,
    CreateOrEditProductAccountTeamDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ProductAccountTeamEmployeeLookupTableModalComponent } from './productAccountTeam-employee-lookup-table-modal.component';
import { ProductAccountTeamProductLookupTableModalComponent } from './productAccountTeam-product-lookup-table-modal.component';

@Component({
    selector: 'createOrEditProductAccountTeamModal',
    templateUrl: './create-or-edit-productAccountTeam-modal.component.html',
})
export class CreateOrEditProductAccountTeamModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('productAccountTeamEmployeeLookupTableModal', { static: true })
    productAccountTeamEmployeeLookupTableModal: ProductAccountTeamEmployeeLookupTableModalComponent;
    @ViewChild('productAccountTeamProductLookupTableModal', { static: true })
    productAccountTeamProductLookupTableModal: ProductAccountTeamProductLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    productAccountTeam: CreateOrEditProductAccountTeamDto = new CreateOrEditProductAccountTeamDto();

    employeeName = '';
    productName = '';
    productId: number;

    constructor(
        injector: Injector,
        private _productAccountTeamsServiceProxy: ProductAccountTeamsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(productAccountTeamId?: number): void {
        if (!productAccountTeamId) {
            this.productAccountTeam = new CreateOrEditProductAccountTeamDto();
            this.productAccountTeam.id = productAccountTeamId;
            this.productAccountTeam.removeDate = this._dateTimeService.getStartOfDay();
            this.productAccountTeam.assignDate = this._dateTimeService.getStartOfDay();
            this.employeeName = '';
            this.productName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._productAccountTeamsServiceProxy
                .getProductAccountTeamForEdit(productAccountTeamId)
                .subscribe((result) => {
                    this.productAccountTeam = result.productAccountTeam;

                    this.employeeName = result.employeeName;
                    this.productName = result.productName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;
        if(this.productId){
            this.productAccountTeam.productId = this.productId;
        }
        this._productAccountTeamsServiceProxy
            .createOrEdit(this.productAccountTeam)
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
        this.productAccountTeamEmployeeLookupTableModal.id = this.productAccountTeam.employeeId;
        this.productAccountTeamEmployeeLookupTableModal.displayName = this.employeeName;
        this.productAccountTeamEmployeeLookupTableModal.show();
    }
    openSelectProductModal() {
        this.productAccountTeamProductLookupTableModal.id = this.productAccountTeam.productId;
        this.productAccountTeamProductLookupTableModal.displayName = this.productName;
        this.productAccountTeamProductLookupTableModal.show();
    }

    setEmployeeIdNull() {
        this.productAccountTeam.employeeId = null;
        this.employeeName = '';
    }
    setProductIdNull() {
        this.productAccountTeam.productId = null;
        this.productName = '';
    }

    getNewEmployeeId() {
        this.productAccountTeam.employeeId = this.productAccountTeamEmployeeLookupTableModal.id;
        this.employeeName = this.productAccountTeamEmployeeLookupTableModal.displayName;
    }
    getNewProductId() {
        this.productAccountTeam.productId = this.productAccountTeamProductLookupTableModal.id;
        this.productName = this.productAccountTeamProductLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
