import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { BusinessUsersServiceProxy, CreateOrEditBusinessUserDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { BusinessUserBusinessLookupTableModalComponent } from './businessUser-business-lookup-table-modal.component';
import { BusinessUserUserLookupTableModalComponent } from './businessUser-user-lookup-table-modal.component';

@Component({
    selector: 'createOrEditBusinessUserModal',
    templateUrl: './create-or-edit-businessUser-modal.component.html',
})
export class CreateOrEditBusinessUserModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('businessUserBusinessLookupTableModal', { static: true })
    businessUserBusinessLookupTableModal: BusinessUserBusinessLookupTableModalComponent;
    @ViewChild('businessUserUserLookupTableModal', { static: true })
    businessUserUserLookupTableModal: BusinessUserUserLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    businessUser: CreateOrEditBusinessUserDto = new CreateOrEditBusinessUserDto();

    businessName = '';
    userName = '';

    constructor(
        injector: Injector,
        private _businessUsersServiceProxy: BusinessUsersServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(businessUserId?: number): void {
        if (!businessUserId) {
            this.businessUser = new CreateOrEditBusinessUserDto();
            this.businessUser.id = businessUserId;
            this.businessName = '';
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._businessUsersServiceProxy.getBusinessUserForEdit(businessUserId).subscribe((result) => {
                this.businessUser = result.businessUser;

                this.businessName = result.businessName;
                this.userName = result.userName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._businessUsersServiceProxy
            .createOrEdit(this.businessUser)
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
        this.businessUserBusinessLookupTableModal.id = this.businessUser.businessId;
        this.businessUserBusinessLookupTableModal.displayName = this.businessName;
        this.businessUserBusinessLookupTableModal.show();
    }
    openSelectUserModal() {
        this.businessUserUserLookupTableModal.id = this.businessUser.userId;
        this.businessUserUserLookupTableModal.displayName = this.userName;
        this.businessUserUserLookupTableModal.show();
    }

    setBusinessIdNull() {
        this.businessUser.businessId = null;
        this.businessName = '';
    }
    setUserIdNull() {
        this.businessUser.userId = null;
        this.userName = '';
    }

    getNewBusinessId() {
        this.businessUser.businessId = this.businessUserBusinessLookupTableModal.id;
        this.businessName = this.businessUserBusinessLookupTableModal.displayName;
    }
    getNewUserId() {
        this.businessUser.userId = this.businessUserUserLookupTableModal.id;
        this.userName = this.businessUserUserLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
