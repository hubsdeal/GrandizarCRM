import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    BusinessAccountTeamsServiceProxy,
    CreateOrEditBusinessAccountTeamDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { BusinessAccountTeamBusinessLookupTableModalComponent } from './businessAccountTeam-business-lookup-table-modal.component';
import { BusinessAccountTeamEmployeeLookupTableModalComponent } from './businessAccountTeam-employee-lookup-table-modal.component';

@Component({
    selector: 'createOrEditBusinessAccountTeamModal',
    templateUrl: './create-or-edit-businessAccountTeam-modal.component.html',
})
export class CreateOrEditBusinessAccountTeamModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('businessAccountTeamBusinessLookupTableModal', { static: true })
    businessAccountTeamBusinessLookupTableModal: BusinessAccountTeamBusinessLookupTableModalComponent;
    @ViewChild('businessAccountTeamEmployeeLookupTableModal', { static: true })
    businessAccountTeamEmployeeLookupTableModal: BusinessAccountTeamEmployeeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    businessAccountTeam: CreateOrEditBusinessAccountTeamDto = new CreateOrEditBusinessAccountTeamDto();

    businessName = '';
    employeeName = '';

    constructor(
        injector: Injector,
        private _businessAccountTeamsServiceProxy: BusinessAccountTeamsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(businessAccountTeamId?: number): void {
        if (!businessAccountTeamId) {
            this.businessAccountTeam = new CreateOrEditBusinessAccountTeamDto();
            this.businessAccountTeam.id = businessAccountTeamId;
            this.businessName = '';
            this.employeeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._businessAccountTeamsServiceProxy
                .getBusinessAccountTeamForEdit(businessAccountTeamId)
                .subscribe((result) => {
                    this.businessAccountTeam = result.businessAccountTeam;

                    this.businessName = result.businessName;
                    this.employeeName = result.employeeName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._businessAccountTeamsServiceProxy
            .createOrEdit(this.businessAccountTeam)
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

    openSelectBusinessModal() {
        this.businessAccountTeamBusinessLookupTableModal.id = this.businessAccountTeam.businessId;
        this.businessAccountTeamBusinessLookupTableModal.displayName = this.businessName;
        this.businessAccountTeamBusinessLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.businessAccountTeamEmployeeLookupTableModal.id = this.businessAccountTeam.employeeId;
        this.businessAccountTeamEmployeeLookupTableModal.displayName = this.employeeName;
        this.businessAccountTeamEmployeeLookupTableModal.show();
    }

    setBusinessIdNull() {
        this.businessAccountTeam.businessId = null;
        this.businessName = '';
    }
    setEmployeeIdNull() {
        this.businessAccountTeam.employeeId = null;
        this.employeeName = '';
    }

    getNewBusinessId() {
        this.businessAccountTeam.businessId = this.businessAccountTeamBusinessLookupTableModal.id;
        this.businessName = this.businessAccountTeamBusinessLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.businessAccountTeam.employeeId = this.businessAccountTeamEmployeeLookupTableModal.id;
        this.employeeName = this.businessAccountTeamEmployeeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
