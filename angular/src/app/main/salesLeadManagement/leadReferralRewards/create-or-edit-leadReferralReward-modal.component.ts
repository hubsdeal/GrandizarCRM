import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    LeadReferralRewardsServiceProxy,
    CreateOrEditLeadReferralRewardDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { LeadReferralRewardLeadLookupTableModalComponent } from './leadReferralReward-lead-lookup-table-modal.component';
import { LeadReferralRewardContactLookupTableModalComponent } from './leadReferralReward-contact-lookup-table-modal.component';

@Component({
    selector: 'createOrEditLeadReferralRewardModal',
    templateUrl: './create-or-edit-leadReferralReward-modal.component.html',
})
export class CreateOrEditLeadReferralRewardModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('leadReferralRewardLeadLookupTableModal', { static: true })
    leadReferralRewardLeadLookupTableModal: LeadReferralRewardLeadLookupTableModalComponent;
    @ViewChild('leadReferralRewardContactLookupTableModal', { static: true })
    leadReferralRewardContactLookupTableModal: LeadReferralRewardContactLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    leadReferralReward: CreateOrEditLeadReferralRewardDto = new CreateOrEditLeadReferralRewardDto();

    leadTitle = '';
    contactFullName = '';

    constructor(
        injector: Injector,
        private _leadReferralRewardsServiceProxy: LeadReferralRewardsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(leadReferralRewardId?: number): void {
        if (!leadReferralRewardId) {
            this.leadReferralReward = new CreateOrEditLeadReferralRewardDto();
            this.leadReferralReward.id = leadReferralRewardId;
            this.leadTitle = '';
            this.contactFullName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._leadReferralRewardsServiceProxy
                .getLeadReferralRewardForEdit(leadReferralRewardId)
                .subscribe((result) => {
                    this.leadReferralReward = result.leadReferralReward;

                    this.leadTitle = result.leadTitle;
                    this.contactFullName = result.contactFullName;

                    this.active = true;
                    this.modal.show();
                });
        }
    }

    save(): void {
        this.saving = true;

        this._leadReferralRewardsServiceProxy
            .createOrEdit(this.leadReferralReward)
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
        this.leadReferralRewardLeadLookupTableModal.id = this.leadReferralReward.leadId;
        this.leadReferralRewardLeadLookupTableModal.displayName = this.leadTitle;
        this.leadReferralRewardLeadLookupTableModal.show();
    }
    openSelectContactModal() {
        this.leadReferralRewardContactLookupTableModal.id = this.leadReferralReward.contactId;
        this.leadReferralRewardContactLookupTableModal.displayName = this.contactFullName;
        this.leadReferralRewardContactLookupTableModal.show();
    }

    setLeadIdNull() {
        this.leadReferralReward.leadId = null;
        this.leadTitle = '';
    }
    setContactIdNull() {
        this.leadReferralReward.contactId = null;
        this.contactFullName = '';
    }

    getNewLeadId() {
        this.leadReferralReward.leadId = this.leadReferralRewardLeadLookupTableModal.id;
        this.leadTitle = this.leadReferralRewardLeadLookupTableModal.displayName;
    }
    getNewContactId() {
        this.leadReferralReward.contactId = this.leadReferralRewardContactLookupTableModal.id;
        this.contactFullName = this.leadReferralRewardContactLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
