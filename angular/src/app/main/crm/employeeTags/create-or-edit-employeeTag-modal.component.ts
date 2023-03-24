import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { EmployeeTagsServiceProxy, CreateOrEditEmployeeTagDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { EmployeeTagEmployeeLookupTableModalComponent } from './employeeTag-employee-lookup-table-modal.component';
import { EmployeeTagMasterTagCategoryLookupTableModalComponent } from './employeeTag-masterTagCategory-lookup-table-modal.component';
import { EmployeeTagMasterTagLookupTableModalComponent } from './employeeTag-masterTag-lookup-table-modal.component';

@Component({
    selector: 'createOrEditEmployeeTagModal',
    templateUrl: './create-or-edit-employeeTag-modal.component.html',
})
export class CreateOrEditEmployeeTagModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('employeeTagEmployeeLookupTableModal', { static: true })
    employeeTagEmployeeLookupTableModal: EmployeeTagEmployeeLookupTableModalComponent;
    @ViewChild('employeeTagMasterTagCategoryLookupTableModal', { static: true })
    employeeTagMasterTagCategoryLookupTableModal: EmployeeTagMasterTagCategoryLookupTableModalComponent;
    @ViewChild('employeeTagMasterTagLookupTableModal', { static: true })
    employeeTagMasterTagLookupTableModal: EmployeeTagMasterTagLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    employeeTag: CreateOrEditEmployeeTagDto = new CreateOrEditEmployeeTagDto();

    employeeName = '';
    masterTagCategoryName = '';
    masterTagName = '';

    constructor(
        injector: Injector,
        private _employeeTagsServiceProxy: EmployeeTagsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(employeeTagId?: number): void {
        if (!employeeTagId) {
            this.employeeTag = new CreateOrEditEmployeeTagDto();
            this.employeeTag.id = employeeTagId;
            this.employeeName = '';
            this.masterTagCategoryName = '';
            this.masterTagName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._employeeTagsServiceProxy.getEmployeeTagForEdit(employeeTagId).subscribe((result) => {
                this.employeeTag = result.employeeTag;

                this.employeeName = result.employeeName;
                this.masterTagCategoryName = result.masterTagCategoryName;
                this.masterTagName = result.masterTagName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._employeeTagsServiceProxy
            .createOrEdit(this.employeeTag)
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
        this.employeeTagEmployeeLookupTableModal.id = this.employeeTag.employeeId;
        this.employeeTagEmployeeLookupTableModal.displayName = this.employeeName;
        this.employeeTagEmployeeLookupTableModal.show();
    }
    openSelectMasterTagCategoryModal() {
        this.employeeTagMasterTagCategoryLookupTableModal.id = this.employeeTag.masterTagCategoryId;
        this.employeeTagMasterTagCategoryLookupTableModal.displayName = this.masterTagCategoryName;
        this.employeeTagMasterTagCategoryLookupTableModal.show();
    }
    openSelectMasterTagModal() {
        this.employeeTagMasterTagLookupTableModal.id = this.employeeTag.masterTagId;
        this.employeeTagMasterTagLookupTableModal.displayName = this.masterTagName;
        this.employeeTagMasterTagLookupTableModal.show();
    }

    setEmployeeIdNull() {
        this.employeeTag.employeeId = null;
        this.employeeName = '';
    }
    setMasterTagCategoryIdNull() {
        this.employeeTag.masterTagCategoryId = null;
        this.masterTagCategoryName = '';
    }
    setMasterTagIdNull() {
        this.employeeTag.masterTagId = null;
        this.masterTagName = '';
    }

    getNewEmployeeId() {
        this.employeeTag.employeeId = this.employeeTagEmployeeLookupTableModal.id;
        this.employeeName = this.employeeTagEmployeeLookupTableModal.displayName;
    }
    getNewMasterTagCategoryId() {
        this.employeeTag.masterTagCategoryId = this.employeeTagMasterTagCategoryLookupTableModal.id;
        this.masterTagCategoryName = this.employeeTagMasterTagCategoryLookupTableModal.displayName;
    }
    getNewMasterTagId() {
        this.employeeTag.masterTagId = this.employeeTagMasterTagLookupTableModal.id;
        this.masterTagName = this.employeeTagMasterTagLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
