import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StoreOwnerTeamsServiceProxy, CreateOrEditStoreOwnerTeamDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { StoreOwnerTeamStoreLookupTableModalComponent } from './storeOwnerTeam-store-lookup-table-modal.component';
import { StoreOwnerTeamUserLookupTableModalComponent } from './storeOwnerTeam-user-lookup-table-modal.component';

@Component({
    selector: 'createOrEditStoreOwnerTeamModal',
    templateUrl: './create-or-edit-storeOwnerTeam-modal.component.html',
})
export class CreateOrEditStoreOwnerTeamModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('storeOwnerTeamStoreLookupTableModal', { static: true })
    storeOwnerTeamStoreLookupTableModal: StoreOwnerTeamStoreLookupTableModalComponent;
    @ViewChild('storeOwnerTeamUserLookupTableModal', { static: true })
    storeOwnerTeamUserLookupTableModal: StoreOwnerTeamUserLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    storeOwnerTeam: CreateOrEditStoreOwnerTeamDto = new CreateOrEditStoreOwnerTeamDto();

    storeName = '';
    userName = '';

    storeId:number;

    constructor(
        injector: Injector,
        private _storeOwnerTeamsServiceProxy: StoreOwnerTeamsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(storeOwnerTeamId?: number): void {
        if (!storeOwnerTeamId) {
            this.storeOwnerTeam = new CreateOrEditStoreOwnerTeamDto();
            this.storeOwnerTeam.id = storeOwnerTeamId;
            this.storeName = '';
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._storeOwnerTeamsServiceProxy.getStoreOwnerTeamForEdit(storeOwnerTeamId).subscribe((result) => {
                this.storeOwnerTeam = result.storeOwnerTeam;
                this.storeId = result.storeOwnerTeam.storeId;
                this.storeName = result.storeName;
                this.userName = result.userName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        this.storeOwnerTeam.storeId = this.storeId;
        this._storeOwnerTeamsServiceProxy
            .createOrEdit(this.storeOwnerTeam)
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
        this.storeOwnerTeamStoreLookupTableModal.id = this.storeOwnerTeam.storeId;
        this.storeOwnerTeamStoreLookupTableModal.displayName = this.storeName;
        this.storeOwnerTeamStoreLookupTableModal.show();
    }
    openSelectUserModal() {
        this.storeOwnerTeamUserLookupTableModal.id = this.storeOwnerTeam.userId;
        this.storeOwnerTeamUserLookupTableModal.displayName = this.userName;
        this.storeOwnerTeamUserLookupTableModal.show();
    }

    setStoreIdNull() {
        this.storeOwnerTeam.storeId = null;
        this.storeName = '';
    }
    setUserIdNull() {
        this.storeOwnerTeam.userId = null;
        this.userName = '';
    }

    getNewStoreId() {
        this.storeOwnerTeam.storeId = this.storeOwnerTeamStoreLookupTableModal.id;
        this.storeName = this.storeOwnerTeamStoreLookupTableModal.displayName;
    }
    getNewUserId() {
        this.storeOwnerTeam.userId = this.storeOwnerTeamUserLookupTableModal.id;
        this.userName = this.storeOwnerTeamUserLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
