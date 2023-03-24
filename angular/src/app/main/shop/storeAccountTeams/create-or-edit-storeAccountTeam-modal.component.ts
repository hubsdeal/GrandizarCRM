import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    StoreAccountTeamsServiceProxy,
    CreateOrEditStoreAccountTeamDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreAccountTeamStoreLookupTableModalComponent } from './storeAccountTeam-store-lookup-table-modal.component';
import { StoreAccountTeamEmployeeLookupTableModalComponent } from './storeAccountTeam-employee-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreAccountTeamModal',
    templateUrl: './create-or-edit-storeAccountTeam-modal.component.html',
})
export class CreateOrEditStoreAccountTeamModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeAccountTeamStoreLookupTableModal', { static: true })
    storeAccountTeamStoreLookupTableModal: StoreAccountTeamStoreLookupTableModalComponent;
    @ViewChild('storeAccountTeamEmployeeLookupTableModal', { static: true })
    storeAccountTeamEmployeeLookupTableModal: StoreAccountTeamEmployeeLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeAccountTeam: CreateOrEditStoreAccountTeamDto = new CreateOrEditStoreAccountTeamDto();

    storeName = '';
    employeeName = '';

    constructor(
        injector: Injector,
        private _storeAccountTeamsServiceProxy: StoreAccountTeamsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeAccountTeamId?: number): void {
        if (!storeAccountTeamId) {
            this.storeAccountTeam = new CreateOrEditStoreAccountTeamDto();
            this.storeAccountTeam.id = storeAccountTeamId;
            this.storeName = '';
            this.employeeName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeAccountTeamsServiceProxy.getStoreAccountTeamForEdit(storeAccountTeamId).subscribe((result) => {
                this.storeAccountTeam = result.storeAccountTeam;

                this.storeName = result.storeName;
                this.employeeName = result.employeeName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._storeAccountTeamsServiceProxy
            .createOrEdit(this.storeAccountTeam)
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

    openSelectStoreModal() {
        this.storeAccountTeamStoreLookupTableModal.id = this.storeAccountTeam.storeId;
        this.storeAccountTeamStoreLookupTableModal.displayName = this.storeName;
        this.storeAccountTeamStoreLookupTableModal.show();
    }
    openSelectEmployeeModal() {
        this.storeAccountTeamEmployeeLookupTableModal.id = this.storeAccountTeam.employeeId;
        this.storeAccountTeamEmployeeLookupTableModal.displayName = this.employeeName;
        this.storeAccountTeamEmployeeLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeAccountTeam.storeId = null;
        this.storeName = '';
    }
    setEmployeeIdNull() {
        this.storeAccountTeam.employeeId = null;
        this.employeeName = '';
    }

    getNewStoreId() {
        this.storeAccountTeam.storeId = this.storeAccountTeamStoreLookupTableModal.id;
        this.storeName = this.storeAccountTeamStoreLookupTableModal.displayName;
    }
    getNewEmployeeId() {
        this.storeAccountTeam.employeeId = this.storeAccountTeamEmployeeLookupTableModal.id;
        this.employeeName = this.storeAccountTeamEmployeeLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
