import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    LeadPipelineStatusesServiceProxy,
    CreateOrEditLeadPipelineStatusDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { LeadPipelineStatusLeadLookupTableModalComponent } from './leadPipelineStatus-lead-lookup-table-modal.component';
import { LeadPipelineStatusLeadPipelineStageLookupTableModalComponent } from './leadPipelineStatus-leadPipelineStage-lookup-table-modal.component';
import { LeadPipelineStatusEmployeeLookupTableModalComponent } from './leadPipelineStatus-employee-lookup-table-modal.component';

@Component({
    selector: 'createOrEditLeadPipelineStatusModal',
    templateUrl: './create-or-edit-leadPipelineStatus-modal.component.html',
})
export class CreateOrEditLeadPipelineStatusModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('leadPipelineStatusLeadLookupTableModal', { static: true })
    leadPipelineStatusLeadLookupTableModal: LeadPipelineStatusLeadLookupTableModalComponent;
    @ViewChild('leadPipelineStatusLeadPipelineStageLookupTableModal', { static: true })
    leadPipelineStatusLeadPipelineStageLookupTableModal: LeadPipelineStatusLeadPipelineStageLookupTableModalComponent;
    @ViewChild('leadPipelineStatusEmployeeLookupTableModal', { static: true })
    leadPipelineStatusEmployeeLookupTableModal: LeadPipelineStatusEmployeeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    leadPipelineStatus: CreateOrEditLeadPipelineStatusDto = new CreateOrEditLeadPipelineStatusDto();

    leadTitle = '';
    leadPipelineStageName = '';
    employeeName = '';

    constructor(
        injector: Injector,
        private _leadPipelineStatusesServiceProxy: LeadPipelineStatusesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(leadPipelineStatusId?: number): void {
        if (!leadPipelineStatusId) {
            this.leadPipelineStatus = new CreateOrEditLeadPipelineStatusDto();
            this.leadPipelineStatus.id = leadPipelineStatusId;
            this.leadPipelineStatus.entryDate = this._dateTimeService.getStartOfDay();
            this.leadPipelineStatus.enteredAt = this._dateTimeService.getStartOfDay();
            this.leadTitle = '';
            this.leadPipelineStageName = '';
            this.employeeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._leadPipelineStatusesServiceProxy
                .getLeadPipelineStatusForEdit(leadPipelineStatusId)
                .subscribe((result) => {
                    this.leadPipelineStatus = result.leadPipelineStatus;

                    this.leadTitle = result.leadTitle;
                    this.leadPipelineStageName = result.leadPipelineStageName;
                    this.employeeName = result.employeeName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._leadPipelineStatusesServiceProxy
            .createOrEdit(this.leadPipelineStatus)
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

    openSelectLeadModal() {
        this.leadPipelineStatusLeadLookupTableModal.id = this.leadPipelineStatus.leadId;
        this.leadPipelineStatusLeadLookupTableModal.displayName = this.leadTitle;
        this.leadPipelineStatusLeadLookupTableModal.show();
    }
    openSelectLeadPipelineStageModal() {
        this.leadPipelineStatusLeadPipelineStageLookupTableModal.id = this.leadPipelineStatus.leadPipelineStageId;
        this.leadPipelineStatusLeadPipelineStageLookupTableModal.displayName = this.leadPipelineStageName;
        this.leadPipelineStatusLeadPipelineStageLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.leadPipelineStatusEmployeeLookupTableModal.id = this.leadPipelineStatus.employeeId;
        this.leadPipelineStatusEmployeeLookupTableModal.displayName = this.employeeName;
        this.leadPipelineStatusEmployeeLookupTableModal.show();
    }

    setLeadIdNull() {
        this.leadPipelineStatus.leadId = null;
        this.leadTitle = '';
    }
    setLeadPipelineStageIdNull() {
        this.leadPipelineStatus.leadPipelineStageId = null;
        this.leadPipelineStageName = '';
    }
    setEmployeeIdNull() {
        this.leadPipelineStatus.employeeId = null;
        this.employeeName = '';
    }

    getNewLeadId() {
        this.leadPipelineStatus.leadId = this.leadPipelineStatusLeadLookupTableModal.id;
        this.leadTitle = this.leadPipelineStatusLeadLookupTableModal.displayName;
    }
    getNewLeadPipelineStageId() {
        this.leadPipelineStatus.leadPipelineStageId = this.leadPipelineStatusLeadPipelineStageLookupTableModal.id;
        this.leadPipelineStageName = this.leadPipelineStatusLeadPipelineStageLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.leadPipelineStatus.employeeId = this.leadPipelineStatusEmployeeLookupTableModal.id;
        this.employeeName = this.leadPipelineStatusEmployeeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
