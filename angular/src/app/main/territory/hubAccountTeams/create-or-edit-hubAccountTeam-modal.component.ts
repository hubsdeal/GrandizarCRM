import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { HubAccountTeamsServiceProxy, CreateOrEditHubAccountTeamDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { HubAccountTeamHubLookupTableModalComponent } from './hubAccountTeam-hub-lookup-table-modal.component';
import { HubAccountTeamEmployeeLookupTableModalComponent } from './hubAccountTeam-employee-lookup-table-modal.component';
import { HubAccountTeamUserLookupTableModalComponent } from './hubAccountTeam-user-lookup-table-modal.component';

@Component({
    selector: 'createOrEditHubAccountTeamModal',
    templateUrl: './create-or-edit-hubAccountTeam-modal.component.html',
})
export class CreateOrEditHubAccountTeamModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('hubAccountTeamHubLookupTableModal', { static: true })
    hubAccountTeamHubLookupTableModal: HubAccountTeamHubLookupTableModalComponent;
    @ViewChild('hubAccountTeamEmployeeLookupTableModal', { static: true })
    hubAccountTeamEmployeeLookupTableModal: HubAccountTeamEmployeeLookupTableModalComponent;
    @ViewChild('hubAccountTeamUserLookupTableModal', { static: true })
    hubAccountTeamUserLookupTableModal: HubAccountTeamUserLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    hubAccountTeam: CreateOrEditHubAccountTeamDto = new CreateOrEditHubAccountTeamDto();

    hubName = '';
    employeeName = '';
    userName = '';

    hubId:number;
    constructor(
        injector: Injector,
        private _hubAccountTeamsServiceProxy: HubAccountTeamsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(hubAccountTeamId?: number): void {
        if (!hubAccountTeamId) {
            this.hubAccountTeam = new CreateOrEditHubAccountTeamDto();
            this.hubAccountTeam.id = hubAccountTeamId;
            this.hubAccountTeam.startDate = this._dateTimeService.getStartOfDay();
            this.hubAccountTeam.endDate = this._dateTimeService.getStartOfDay();
            this.hubName = '';
            this.employeeName = '';
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._hubAccountTeamsServiceProxy.getHubAccountTeamForEdit(hubAccountTeamId).subscribe((result) => {
                this.hubAccountTeam = result.hubAccountTeam;
                this.hubId = result.hubAccountTeam.hubId;
                this.hubName = result.hubName;
                this.employeeName = result.employeeName;
                this.userName = result.userName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        this.hubAccountTeam.hubId = this.hubId;
        this._hubAccountTeamsServiceProxy
            .createOrEdit(this.hubAccountTeam)
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

    openSelectHubModal() {
        this.hubAccountTeamHubLookupTableModal.id = this.hubAccountTeam.hubId;
        this.hubAccountTeamHubLookupTableModal.displayName = this.hubName;
        this.hubAccountTeamHubLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.hubAccountTeamEmployeeLookupTableModal.id = this.hubAccountTeam.employeeId;
        this.hubAccountTeamEmployeeLookupTableModal.displayName = this.employeeName;
        this.hubAccountTeamEmployeeLookupTableModal.show();
    }
    openSelectUserModal() {
        this.hubAccountTeamUserLookupTableModal.id = this.hubAccountTeam.userId;
        this.hubAccountTeamUserLookupTableModal.displayName = this.userName;
        this.hubAccountTeamUserLookupTableModal.show();
    }

    setHubIdNull() {
        this.hubAccountTeam.hubId = null;
        this.hubName = '';
    }
    setEmployeeIdNull() {
        this.hubAccountTeam.employeeId = null;
        this.employeeName = '';
    }
    setUserIdNull() {
        this.hubAccountTeam.userId = null;
        this.userName = '';
    }

    getNewHubId() {
        this.hubAccountTeam.hubId = this.hubAccountTeamHubLookupTableModal.id;
        this.hubName = this.hubAccountTeamHubLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.hubAccountTeam.employeeId = this.hubAccountTeamEmployeeLookupTableModal.id;
        this.employeeName = this.hubAccountTeamEmployeeLookupTableModal.displayName;
    }
    getNewUserId() {
        this.hubAccountTeam.userId = this.hubAccountTeamUserLookupTableModal.id;
        this.userName = this.hubAccountTeamUserLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
