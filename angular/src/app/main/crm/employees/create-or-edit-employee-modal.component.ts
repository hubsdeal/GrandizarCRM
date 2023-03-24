import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { EmployeesServiceProxy, CreateOrEditEmployeeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { EmployeeStateLookupTableModalComponent } from './employee-state-lookup-table-modal.component';
import { EmployeeCountryLookupTableModalComponent } from './employee-country-lookup-table-modal.component';
import { EmployeeContactLookupTableModalComponent } from './employee-contact-lookup-table-modal.component';

@Component({
    selector: 'createOrEditEmployeeModal',
    templateUrl: './create-or-edit-employee-modal.component.html',
})
export class CreateOrEditEmployeeModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('employeeStateLookupTableModal', { static: true })
    employeeStateLookupTableModal: EmployeeStateLookupTableModalComponent;
    @ViewChild('employeeCountryLookupTableModal', { static: true })
    employeeCountryLookupTableModal: EmployeeCountryLookupTableModalComponent;
    @ViewChild('employeeContactLookupTableModal', { static: true })
    employeeContactLookupTableModal: EmployeeContactLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    employee: CreateOrEditEmployeeDto = new CreateOrEditEmployeeDto();

    stateName = '';
    countryName = '';
    contactFullName = '';

    constructor(
        injector: Injector,
        private _employeesServiceProxy: EmployeesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(employeeId?: number): void {
        if (!employeeId) {
            this.employee = new CreateOrEditEmployeeDto();
            this.employee.id = employeeId;
            this.employee.dateOfBirth = this._dateTimeService.getStartOfDay();
            this.employee.hireDate = this._dateTimeService.getStartOfDay();
            this.stateName = '';
            this.countryName = '';
            this.contactFullName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._employeesServiceProxy.getEmployeeForEdit(employeeId).subscribe((result) => {
                this.employee = result.employee;

                this.stateName = result.stateName;
                this.countryName = result.countryName;
                this.contactFullName = result.contactFullName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._employeesServiceProxy
            .createOrEdit(this.employee)
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

    openSelectStateModal() {
        this.employeeStateLookupTableModal.id = this.employee.stateId;
        this.employeeStateLookupTableModal.displayName = this.stateName;
        this.employeeStateLookupTableModal.show();
    }
    openSelectCountryModal() {
        this.employeeCountryLookupTableModal.id = this.employee.countryId;
        this.employeeCountryLookupTableModal.displayName = this.countryName;
        this.employeeCountryLookupTableModal.show();
    }
    openSelectContactModal() {
        this.employeeContactLookupTableModal.id = this.employee.contactId;
        this.employeeContactLookupTableModal.displayName = this.contactFullName;
        this.employeeContactLookupTableModal.show();
    }

    setStateIdNull() {
        this.employee.stateId = null;
        this.stateName = '';
    }
    setCountryIdNull() {
        this.employee.countryId = null;
        this.countryName = '';
    }
    setContactIdNull() {
        this.employee.contactId = null;
        this.contactFullName = '';
    }

    getNewStateId() {
        this.employee.stateId = this.employeeStateLookupTableModal.id;
        this.stateName = this.employeeStateLookupTableModal.displayName;
    }
    getNewCountryId() {
        this.employee.countryId = this.employeeCountryLookupTableModal.id;
        this.countryName = this.employeeCountryLookupTableModal.displayName;
    }
    getNewContactId() {
        this.employee.contactId = this.employeeContactLookupTableModal.id;
        this.contactFullName = this.employeeContactLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
