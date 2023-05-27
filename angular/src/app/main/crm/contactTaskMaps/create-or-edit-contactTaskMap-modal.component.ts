import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ContactTaskMapsServiceProxy, CreateOrEditContactTaskMapDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ContactTaskMapContactLookupTableModalComponent } from './contactTaskMap-contact-lookup-table-modal.component';
import { ContactTaskMapTaskEventLookupTableModalComponent } from './contactTaskMap-taskEvent-lookup-table-modal.component';

@Component({
    selector: 'createOrEditContactTaskMapModal',
    templateUrl: './create-or-edit-contactTaskMap-modal.component.html',
})
export class CreateOrEditContactTaskMapModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('contactTaskMapContactLookupTableModal', { static: true })
    contactTaskMapContactLookupTableModal: ContactTaskMapContactLookupTableModalComponent;
    @ViewChild('contactTaskMapTaskEventLookupTableModal', { static: true })
    contactTaskMapTaskEventLookupTableModal: ContactTaskMapTaskEventLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    contactTaskMap: CreateOrEditContactTaskMapDto = new CreateOrEditContactTaskMapDto();

    contactFullName = '';
    taskEventName = '';

    constructor(
        injector: Injector,
        private _contactTaskMapsServiceProxy: ContactTaskMapsServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(contactTaskMapId?: number): void {
        if (!contactTaskMapId) {
            this.contactTaskMap = new CreateOrEditContactTaskMapDto();
            this.contactTaskMap.id = contactTaskMapId;
            this.contactFullName = '';
            this.taskEventName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._contactTaskMapsServiceProxy.getContactTaskMapForEdit(contactTaskMapId).subscribe((result) => {
                this.contactTaskMap = result.contactTaskMap;

                this.contactFullName = result.contactFullName;
                this.taskEventName = result.taskEventName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._contactTaskMapsServiceProxy
            .createOrEdit(this.contactTaskMap)
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

    openSelectContactModal() {
        this.contactTaskMapContactLookupTableModal.id = this.contactTaskMap.contactId;
        this.contactTaskMapContactLookupTableModal.displayName = this.contactFullName;
        this.contactTaskMapContactLookupTableModal.show();
    }
    openSelectTaskEventModal() {
        this.contactTaskMapTaskEventLookupTableModal.id = this.contactTaskMap.taskEventId;
        this.contactTaskMapTaskEventLookupTableModal.displayName = this.taskEventName;
        this.contactTaskMapTaskEventLookupTableModal.show();
    }

    setContactIdNull() {
        this.contactTaskMap.contactId = null;
        this.contactFullName = '';
    }
    setTaskEventIdNull() {
        this.contactTaskMap.taskEventId = null;
        this.taskEventName = '';
    }

    getNewContactId() {
        this.contactTaskMap.contactId = this.contactTaskMapContactLookupTableModal.id;
        this.contactFullName = this.contactTaskMapContactLookupTableModal.displayName;
    }
    getNewTaskEventId() {
        this.contactTaskMap.taskEventId = this.contactTaskMapTaskEventLookupTableModal.id;
        this.taskEventName = this.contactTaskMapTaskEventLookupTableModal.displayName;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
