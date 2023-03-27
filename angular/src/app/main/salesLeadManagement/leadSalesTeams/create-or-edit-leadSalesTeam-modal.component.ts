import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { LeadSalesTeamsServiceProxy, CreateOrEditLeadSalesTeamDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { LeadSalesTeamLeadLookupTableModalComponent } from './leadSalesTeam-lead-lookup-table-modal.component';
import { LeadSalesTeamEmployeeLookupTableModalComponent } from './leadSalesTeam-employee-lookup-table-modal.component';

@Component({
    selector: 'createOrEditLeadSalesTeamModal',
    templateUrl: './create-or-edit-leadSalesTeam-modal.component.html',
})
export class CreateOrEditLeadSalesTeamModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('leadSalesTeamLeadLookupTableModal', { static: true })
    leadSalesTeamLeadLookupTableModal: LeadSalesTeamLeadLookupTableModalComponent;
    @ViewChild('leadSalesTeamEmployeeLookupTableModal', { static: true })
    leadSalesTeamEmployeeLookupTableModal: LeadSalesTeamEmployeeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    leadSalesTeam: CreateOrEditLeadSalesTeamDto = new CreateOrEditLeadSalesTeamDto();

    leadFirstName = '';
    employeeName = '';

    constructor(
        injector: Injector,
        private _leadSalesTeamsServiceProxy: LeadSalesTeamsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(leadSalesTeamId?: number): void {
        if (!leadSalesTeamId) {
            this.leadSalesTeam = new CreateOrEditLeadSalesTeamDto();
            this.leadSalesTeam.id = leadSalesTeamId;
            this.leadSalesTeam.assignedDate = this._dateTimeService.getStartOfDay();
            this.leadFirstName = '';
            this.employeeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._leadSalesTeamsServiceProxy.getLeadSalesTeamForEdit(leadSalesTeamId).subscribe((result) => {
                this.leadSalesTeam = result.leadSalesTeam;

                this.leadFirstName = result.leadFirstName;
                this.employeeName = result.employeeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._leadSalesTeamsServiceProxy
            .createOrEdit(this.leadSalesTeam)
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
        this.leadSalesTeamLeadLookupTableModal.id = this.leadSalesTeam.leadId;
        this.leadSalesTeamLeadLookupTableModal.displayName = this.leadFirstName;
        this.leadSalesTeamLeadLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.leadSalesTeamEmployeeLookupTableModal.id = this.leadSalesTeam.employeeId;
        this.leadSalesTeamEmployeeLookupTableModal.displayName = this.employeeName;
        this.leadSalesTeamEmployeeLookupTableModal.show();
    }

    setLeadIdNull() {
        this.leadSalesTeam.leadId = null;
        this.leadFirstName = '';
    }
    setEmployeeIdNull() {
        this.leadSalesTeam.employeeId = null;
        this.employeeName = '';
    }

    getNewLeadId() {
        this.leadSalesTeam.leadId = this.leadSalesTeamLeadLookupTableModal.id;
        this.leadFirstName = this.leadSalesTeamLeadLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.leadSalesTeam.employeeId = this.leadSalesTeamEmployeeLookupTableModal.id;
        this.employeeName = this.leadSalesTeamEmployeeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
