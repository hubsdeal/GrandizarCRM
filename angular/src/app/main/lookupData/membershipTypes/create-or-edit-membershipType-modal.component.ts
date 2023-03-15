import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { MembershipTypesServiceProxy, CreateOrEditMembershipTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditMembershipTypeModal',
    templateUrl: './create-or-edit-membershipType-modal.component.html',
})
export class CreateOrEditMembershipTypeModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    membershipType: CreateOrEditMembershipTypeDto = new CreateOrEditMembershipTypeDto();

    constructor(
        injector: Injector,
        private _membershipTypesServiceProxy: MembershipTypesServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(membershipTypeId?: number): void {
        if (!membershipTypeId) {
            this.membershipType = new CreateOrEditMembershipTypeDto();
            this.membershipType.id = membershipTypeId;

            this.active = true;
            this.modal.show();
        } else {
            this._membershipTypesServiceProxy.getMembershipTypeForEdit(membershipTypeId).subscribe((result) => {
                this.membershipType = result.membershipType;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._membershipTypesServiceProxy
            .createOrEdit(this.membershipType)
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

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
